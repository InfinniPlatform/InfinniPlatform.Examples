using System;

using Infinni.Northwind.Authentication;
using Infinni.Northwind.IoC;

using InfinniPlatform.AspNetCore;
using InfinniPlatform.Auth;
using InfinniPlatform.Http.StaticFiles;
using InfinniPlatform.IoC;
using InfinniPlatform.Logging;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Serilog;
using Serilog.Core;
using Serilog.Filters;

using LogEvent = Serilog.Events.LogEvent;

namespace Infinni.Northwind
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("AppConfig.json", true, true)
                .AddJsonFile($"AppConfig.{env.EnvironmentName}.json", true)
                .AddEnvironmentVariables();

            _configuration = builder.Build();
        }

        private readonly IConfigurationRoot _configuration;

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var serviceProvider = services.AddAuthInternal<CustomUser, AppUserRole>(_configuration)
                                          // Для подключения сторонней реализации хранилища пользователей:
                                          //.AddAuthInternal<CustomUser, AppUserRole>(_configuration, opt => { opt.UserStoreFactory = new CustomUserStoreFactory(); })
                                          .AddAuthHttpService<CustomUser>(_configuration)
                                          .AddInMemoryCache()
                                          .AddMongoDocumentStorage(_configuration)
                                          .AddRabbitMqMessageQueue(_configuration)
                                          .AddQuartzScheduler(_configuration)
                                          .AddPrintView(_configuration)
                                          .AddContainerModule(new NorthwindContainerModule())
                                          .BuildProvider(_configuration);

            return serviceProvider;
        }

        public void Configure(IApplicationBuilder app,
                              IContainerResolver resolver,
                              ILoggerFactory loggerFactory,
                              IApplicationLifetime appLifetime,
                              IHttpContextAccessor httpContextAccessor)
        {
            ConfigureLogger(loggerFactory, appLifetime, httpContextAccessor);

            app.UseStaticFilesMapping(_configuration);
            
            app.UseAppLayers(resolver);
        }

        private static void ConfigureLogger(ILoggerFactory loggerFactory,
                                            IApplicationLifetime appLifetime,
                                            IHttpContextAccessor httpContextAccessor)
        {
            const string outputTemplate
                = "{Timestamp:o}|{Level:u3}|{RequestId}|{UserName}|{SourceContext}|{Message}{NewLine}{Exception}";

            Func<LogEvent, bool> performanceLoggerFilter =
                Matching.WithProperty<string>(
                    Constants.SourceContextPropertyName,
                    p => p.StartsWith(nameof(IPerformanceLogger)));

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .Enrich.With(new HttpContextLogEventEnricher(httpContextAccessor))
                .WriteTo.LiterateConsole(outputTemplate: outputTemplate)
                .WriteTo.Logger(lc => lc.Filter.ByExcluding(performanceLoggerFilter)
                                        .WriteTo.RollingFile("logs/events-{Date}.log",
                                            outputTemplate: outputTemplate))
                .WriteTo.Logger(lc => lc.Filter.ByIncludingOnly(performanceLoggerFilter)
                                        .WriteTo.RollingFile("logs/performance-{Date}.log",
                                            outputTemplate: outputTemplate))
                .CreateLogger();

            // Register Serilog
            loggerFactory.AddSerilog();

            // Ensure any buffered events are sent at shutdown
            appLifetime.ApplicationStopped.Register(Log.CloseAndFlush);
        }
    }
}