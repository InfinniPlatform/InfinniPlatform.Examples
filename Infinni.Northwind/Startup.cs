using System;
using System.Linq;
using System.Reflection;

using Infinni.Northwind.Authentication;
using Infinni.Northwind.ExternalAuthentication;

using InfinniPlatform.AspNetCore;
using InfinniPlatform.Auth;
using InfinniPlatform.Auth.Middlewares;
using InfinniPlatform.Http.Middlewares;
using InfinniPlatform.Http.StaticFiles;
using InfinniPlatform.IoC;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            //var serviceProvider = services.AddAuthInternal<CustomUser, AppUserRole>(_configuration, opt => { opt.UserStoreFactory = new CustomUserStoreFactory(); })
            //                              .AddAuthHttpService<CustomUser>()
            //                              .AddInMemoryCache()
            //                              .AddMongoDocumentStorage(_configuration)
            //                              .AddLog4NetLogging()
            //                              .AddRabbitMqMessageQueue(_configuration)
            //                              .AddQuartzScheduler(_configuration)
            //                              .AddPrintView(_configuration)
            //                              .BuildProvider();

            var serviceProvider = services.AddAuthInternal<CustomUser, AppUserRole>(_configuration)
                                          .AddAuthHttpService<CustomUser>()
                                          .AddInMemoryCache()
                                          .AddMongoDocumentStorage(_configuration)
                                          .AddLog4NetLogging()
                                          .AddRabbitMqMessageQueue(_configuration)
                                          .AddQuartzScheduler(_configuration)
                                          .AddPrintView(_configuration)
                                          .BuildProvider(_configuration);

            return serviceProvider;
        }

        public void Configure(IApplicationBuilder app, IContainerResolver resolver)
        {
            app.UseStaticFilesMapping(_configuration);

            var appLayersMap = new AppLayersMap(resolver);
            appLayersMap.AddErrorHandlingAppLayer<ErrorHandlingAppLayer>();
            appLayersMap.AddAuthenticationBarrierAppLayer<AuthCookieAppLayer>();
            appLayersMap.AddExternalAuthenticationAppLayer<FacebookAuthAppLayer>();
            appLayersMap.AddInternalAuthenticationAppLayer<AuthInternalAppLayer>();
            appLayersMap.AddBusinessAppLayer<NancyAppLayer>();

            app.UseAppLayersMap(appLayersMap);
        }
    }
}