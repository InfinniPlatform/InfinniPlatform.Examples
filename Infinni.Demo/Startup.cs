using System;

using InfinniPlatform.AspNetCore;
using InfinniPlatform.IoC;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infinni.Demo
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
            services.AddDataProtection(options => options.ApplicationDiscriminator = "Infinni.Demo")
                    .SetApplicationName("Infinni.Demo");

            var serviceProvider = services.AddAuthInternal(_configuration)
                                          .AddMongoDocumentStorage(_configuration)
                                          .AddDocumentStorageHttpService()
                                          .AddLog4NetLogging()
                                          .AddFileSystemBlobStorage()
                                          .BuildProvider(_configuration);

            return serviceProvider;
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, IContainerResolver resolver)
        {
            app.UseInfinniMiddlewares(resolver);
        }
    }
}