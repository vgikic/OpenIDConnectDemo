using Demo.IdentityProvider.DAL;
using Demo.IdentityProvider.Models;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shared;
using System;
using System.Linq;
using System.Reflection;

namespace Demo.IdentityProvider
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }
        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = Configuration.GetConnectionString("UserStore");
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;


            services.AddDbContext<UserStoreDbContext>(options => options.UseSqlServer(connectionString));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                                .AddEntityFrameworkStores<UserStoreDbContext>()
                                .AddDefaultTokenProviders();

            services.AddMvc();

            services.Configure<IISOptions>(iis =>
            {
                iis.AuthenticationDisplayName = "Windows";
                iis.AutomaticAuthentication = false;
            });

            var builder = services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
            })
              .AddAspNetIdentity<ApplicationUser>()
                // this adds the config data from DB (clients, resources)
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = b =>
                        b.UseSqlServer(connectionString,
                            sql => sql.MigrationsAssembly(migrationsAssembly));
                })
                // this adds the operational data from DB (codes, tokens, consents)
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = b =>
                        b.UseSqlServer(connectionString,
                            sql => sql.MigrationsAssembly(migrationsAssembly));

                    // this enables automatic token cleanup. this is optional.
                    options.EnableTokenCleanup = true;
                    // options.TokenCleanupInterval = 15; // frequency in seconds to cleanup stale grants. 15 is useful during debugging
                });

            if (Environment.IsDevelopment())
            {
                builder.AddDeveloperSigningCredential();
            }
            else
            {
                throw new Exception("need to configure key material");
            }

            //.AddInMemoryApiResources(Config.GetApiResource())
            //.AddInMemoryIdentityResources(Config.GetIdentityResources())
            //.AddInMemoryClients(Config.GetClients());


            /// <summary>
            ///  THIS IS USED FOR IN MEMORY IDP SETUP
            /// </summary>
            //services.AddIdentityServer()
            //        .AddDeveloperSigningCredential()
            //        .AddInMemoryApiResources(Config.GetApiResource())
            //        .AddTestUsers(Config.GetUsers())
            //        .AddInMemoryIdentityResources(Config.GetIdentityResources())
            //        .AddInMemoryClients(Config.GetClients());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, UserManager<ApplicationUser> userManager)
        {
           // InitializeDatabase(app, userManager);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            loggerFactory.AddConsole();
            loggerFactory.AddDebug();
            app.UseCors(builder => builder.WithOrigins(Parties.WebClientUrl).AllowAnyHeader().AllowAnyMethod().AllowCredentials());
            app.UseIdentityServer();
            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
        private void InitializeDatabase(IApplicationBuilder app, UserManager<ApplicationUser> userManager)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                var identityContext = serviceScope.ServiceProvider.GetRequiredService<UserStoreDbContext>();
                //context.Database.Migrate();

                if (!identityContext.Users.Any())
                {
                    foreach (var user in Config.GetUsers())
                    {

                        var appUser = new ApplicationUser
                        {
                            UserName = user.Username
                        };

                        var result = userManager.CreateAsync(appUser, "Password123!").Result;
                        userManager.AddClaimsAsync(appUser, user.Claims).Wait();
                    }
                }

                if (!context.Clients.Any())
                {
                    foreach (var client in Config.GetClients())
                    {
                        context.Clients.Add(client.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.IdentityResources.Any())
                {
                    foreach (var resource in Config.GetIdentityResources())
                    {
                        context.IdentityResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.ApiResources.Any())
                {
                    foreach (var resource in Config.GetApiResource())
                    {
                        context.ApiResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }
            }
        }
    }
}
