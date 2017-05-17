using System;

using Infinni.Northwind.Auth;

using InfinniPlatform.AspNetCore;
using InfinniPlatform.Auth;
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
            var serviceProvider = services.AddAuthInternal<CustomUser, AppUserRole>(_configuration, opt => { opt.UserStoreFactory = new CustomUserStoreFactory(); })
                                          .AddAuthHttpService<CustomUser>()
                                          .AddInMemoryCache()
                                          .AddMongoDocumentStorage(_configuration)
                                          .AddLog4NetLogging()
                                          .AddRabbitMqMessageQueue(_configuration)
                                          .AddQuartzScheduler(_configuration)
                                          .AddPrintView(_configuration)
                                          .BuildProvider();

            return serviceProvider;
        }

        public void Configure(IApplicationBuilder app, IContainerResolver resolver)
        {
            app.UseStaticFilesMapping(_configuration);

            app.UseInfinniMiddlewares(resolver);
        }
    }
}