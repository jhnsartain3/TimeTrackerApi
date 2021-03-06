using System;
using System.Text;
using Api.Extensions;
using Consumables;
using Contexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Sartain_Studios_Common.Logging;
using Services;

namespace Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private string ApplicationName => Configuration.GetSection("ApplicationInformation:ApplicationName").Value;
        private string ApplicationVersion => Configuration.GetSection("ApplicationInformation:VersionNumber").Value;

        private static string CorsPolicyName => "CorsOpenPolicy";

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(o => o.AddPolicy(CorsPolicyName, builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));

            SetupServices(services);
            SetupConsumables(services);
            SetupContexts(services);

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(ApplicationVersion,
                    new OpenApiInfo {Title = ApplicationName, Version = ApplicationVersion});
            });

            var authenticationSecret = Configuration.GetSection("AuthenticationSecret").Value;

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Jwt";
                options.DefaultChallengeScheme = "Jwt";
            }).AddJwtBearer("Jwt", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSecret)),

                    ValidateLifetime = true,

                    ClockSkew = TimeSpan.FromMinutes(1)
                };
            });
        }

        private void SetupServices(IServiceCollection services)
        {
            services.AddSingleton<IEventService, EventService>();
            services.AddSingleton<IProjectService, ProjectService>();

            var logPath = Configuration.GetSection("LogWriteLocation").Value;
            services.AddSingleton<ILoggerWrapper>(new LoggerWrapper(logPath));
        }

        private static void SetupConsumables(IServiceCollection services)
        {
            services.AddSingleton<IEventConsumable, EventConsumable>();
            services.AddSingleton<IProjectConsumable, ProjectConsumable>();
        }

        private static void SetupContexts(IServiceCollection services)
        {
            services.AddSingleton<IEventContext, EventContext>();
            services.AddSingleton<IProjectContext, ProjectContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerWrapper loggerWrapper)
        {
            app.UseAuthentication();

            app.UseCors(CorsPolicyName);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();

                var url = "/swagger/" + ApplicationVersion + "/swagger.json";
                var name = ApplicationName + " " + ApplicationVersion;

                app.UseSwaggerUI(swaggerUiOptions => { swaggerUiOptions.SwaggerEndpoint(url, name); });
            }

            app.ConfigureExceptionHandler(loggerWrapper);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}