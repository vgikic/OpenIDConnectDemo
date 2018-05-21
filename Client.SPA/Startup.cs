using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using Shared;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Client.SPA
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();


            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = "oidc";
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
             {
                 options.ExpireTimeSpan = TimeSpan.FromMinutes(1);
                 options.Cookie.Name = "spa_cookie";
                 options.AccessDeniedPath = "/access-denied";
             })

            .AddOpenIdConnect("oidc", options =>
            {
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.Authority =  Parties.AuthorityUrl; // Demo.IdentityProvider base url
                options.RequireHttpsMetadata = true;
                options.GetClaimsFromUserInfoEndpoint = true;
                options.ClientId = "SPA_APP";
                options.ClientSecret = "password";
                options.ResponseType = "code id_token";

                // Scope
                // - list of Claims that should be return for each user
                // - openid is mandatory
                options.Scope.Clear();
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add(ClaimDeclaration.Roles);
                options.Scope.Add("demoapi"); // For API Scope
                options.Scope.Add(ClaimDeclaration.Subscriptionlevel);
                options.Scope.Add("offline_access"); // Needed by refresh token

                options.SaveTokens = true;
                options.Events = new OpenIdConnectEvents
                {

                    OnUserInformationReceived = context =>
                     {
                         // (@) IdentityServer returns multiple claim values as JSON arrays, which break the authentication handler https://github.com/aspnet/Security/issues/1383
                         if (context.User.TryGetValue(JwtClaimTypes.Role, value: out var roles))
                         {
                             var claims = new List<Claim>();

                             if (roles.Type != JTokenType.Array)
                             {
                                 claims.Add(new Claim(JwtClaimTypes.Role, (string)roles));
                             }
                             else
                             {
                                 claims.AddRange(roles.Select(role => new Claim(JwtClaimTypes.Role, (string)role)));
                             }

                             var id = context.Principal.Identity as ClaimsIdentity;

                             id?.AddClaims(claims);
                         }

                         return Task.CompletedTask;
                     }
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //app.UseCors(builder => builder.WithOrigins(" https://localhost:44331").AllowAnyHeader());
            app.UseAuthentication();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });
        }
    }
}
