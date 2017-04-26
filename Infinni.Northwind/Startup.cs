using System;

using InfinniPlatform.AspNetCore;
using InfinniPlatform.IoC;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Infinni.Northwind
{
    public class Startup
    {
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var serviceProvider = services.AddInMemoryCache()
                                          .AddMongoDocumentStorage()
                                          .AddLog4NetLogging()
                                          .AddRabbitMqMessageQueue()
                                          .AddQuartzScheduler()
                                          .AddPrintView()
                                          .BuildProvider();

            return serviceProvider;
        }

        public void Configure(IApplicationBuilder app, IContainerResolver resolver, IApplicationLifetime lifetime)
        {
            app.UseInfinniMiddlewares(resolver);
        }
    }
}