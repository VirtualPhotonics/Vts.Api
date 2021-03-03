using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Vts.Api.Factories;
using Vts.Api.Security;
using Vts.Api.Services;
using Vts.Api.Tools;

namespace Vts.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            Configuration = configuration;
            _logger = logger;
        }

        public IConfiguration Configuration { get; }
        private readonly ILogger _logger;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            _logger.LogInformation("The Angular Application is starting");
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
            services.AddTransient<IAuthorizationHandler, ApiKeyRequirementHandler>();
            services.AddTransient<IForwardSolverService, ForwardSolverService>();
            services.AddTransient<IInverseSolverService, InverseSolverService>();
            services.AddTransient<ISpectralService, SpectralService>();
            services.AddTransient<IParameterTools, ParameterTools>();
            services.AddTransient<PlotSpectralResultsService>();
            services.AddTransient<PlotSolutionDomainResultsService>();
            services.AddTransient<IPlotFactory, PlotFactory>();
            services.AddAuthentication().AddJwtBearer();
            services.AddAuthorization(authConfig =>
            {
                authConfig.AddPolicy("ApiKeyPolicy",
                    policyBuilder => policyBuilder
                        .AddRequirements(new ApiKeyRequirement(new[] { "TESTKEY" }))
                        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
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
