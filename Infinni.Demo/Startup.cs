using System;

using InfinniPlatform.Extensions;
using InfinniPlatform.Sdk.IoC;

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
                    .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddDataProtection(options => options.ApplicationDiscriminator = "Infinni.Demo")
                    .SetApplicationName("Infinni.Demo");

            var serviceProvider = services.AddAuth()
                                          .AddDocumentStorage()
                                          .AddBlobStorage()
                                          .AddLog4NetAdapter()
                                          .BuildProvider();

            return serviceProvider;
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, IContainerResolver resolver)
        {
            loggerFactory.AddConsole();

            app.UseInfinniMiddlewares(resolver);
        }
    }
}