using Demo.API.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Shared;

namespace Demo.API
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddMvcCore()
                    .AddAuthorization(options =>
                    {
                        // Authorization policy based on User claims
                        options.AddPolicy(PolicyDeclaration.IsPremiumSubscriber, policy =>
                        {
                            policy.RequireAuthenticatedUser();
                            policy.RequireClaim(ClaimDeclaration.Role, RoleType.User);
                            policy.RequireClaim(ClaimDeclaration.Subscriptionlevel, SubscriptionType.Premium);
                        });

                        options.AddPolicy(PolicyDeclaration.MustOwnRecord, policy =>
                        {
                            policy.RequireAuthenticatedUser();
                            policy.AddRequirements(new MustOwnRecordRequirement());
                        });

                    })
                    .AddJsonFormatters();


            // Custom AuthorizationHandlers need to be registered for DI
            services.AddSingleton<IAuthorizationHandler, MustOwnRecordHandler>();


            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = Parties.AuthorityUrl; // Identity Provider
                    options.RequireHttpsMetadata = true;
                    options.ApiName = "demoapi";
                });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //app.UseCors(options => options.WithOrigins("https://localhost:44347/").AllowAnyHeader().AllowCredentials().AllowAnyMethod());
            app.UseCors(builder => builder.WithOrigins(Parties.WebClientUrl).AllowAnyHeader().AllowAnyMethod().AllowCredentials());
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
