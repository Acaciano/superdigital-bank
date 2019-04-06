using System.Text;
using SuperDigital.DigitalAccount.Api.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SuperDigital.DigitalAccount.Domain.Domain;
using SuperDigital.DigitalAccount.Ioc;

namespace SuperDigital.DigitalAccount.Api
{
    public partial class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                    .SetBasePath(env.ContentRootPath)
                    .AddJsonFile("appsettings.json");

            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvc(config =>
                {
                    config.Filters.Add(typeof(CustomExceptionFilter));
                })
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("8d200898-0e4b-4897-855a-d5d999f6fc1b")),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            // DB CONNECTION STRING
            ModuleConfiguration.ConnectionString = Configuration["ConnectionStrings:DB"];

            Injector.RegisterServices(services);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            ConfigureAuth(app);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseMvc();
        }
    }
}
