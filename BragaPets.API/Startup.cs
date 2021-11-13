using System;
using System.Reflection;
using BragaPets.API.Configurations;
using BragaPets.API.Filters;
using BragaPets.API.AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using BragaPets.Infra.CrossCutting.IoC;
using BragaPets.Infra.CrossCutting.Utils;
using Elastic.Apm.AspNetCore;
using Elastic.Apm.SerilogEnricher;
using Elasticsearch.Net;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace BragaPets.API
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            Configuration = CreateConfigurationRoot(env);
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(option => option.Filters.Add<NotificationFilter>());
            services.AddAutoMapper(typeof(AutoMapperConfiguration));
            services.AddControllers();
            services.ConfigureIoC(Configuration);
            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            });
            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VV";
                options.SubstituteApiVersionInUrl = true;
            });
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, SwaggerGenConfigurationOptions>();
            services.AddSwaggerGen();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider,
            ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint(
                            $"../swagger/{description.GroupName}/swagger.json", 
                            description.GroupName.ToUpperInvariant());
                    }
                });
            }
            
            loggerFactory.AddSerilog(LogConfiguration(), true);
            app.UseElasticApm(Configuration);
            app.UseRouting();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
        
        private IConfiguration CreateConfigurationRoot(IWebHostEnvironment env)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
            return configuration;
        }
        
        private Serilog.ILogger LogConfiguration()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var elkConfiguration = Configuration.GetSection("ElkConfiguration")?.Get<ElkConfiguration>();

            Log.Logger = new LoggerConfiguration()
                .Enrich.WithProperty("ApplicationName", "BragaPets")
                .Enrich.WithProperty("Environment", environment)
                .MinimumLevel.Warning()
                .Enrich.WithElasticApmCorrelationInfo()
                .WriteTo.Console()
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://elastic:BragaP3t2@localhost:9200"))
                {
                    AutoRegisterTemplate = true,
                    AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
                    IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name?.ToLower()}-{0:yyyy-MM}"
                })
                .CreateLogger();

                return Log.Logger;
            
        }
    }
}