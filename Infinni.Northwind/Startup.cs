using System;

using InfinniPlatform.Extensions;
using InfinniPlatform.Sdk.Hosting;
using InfinniPlatform.Sdk.IoC;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Infinni.Northwind
{
    public class Startup
    {
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var serviceProvider = services.AddCaching()
                                          .AddDocumentStorage()
                                          .AddLog4NetAdapter()
                                          .AddMessageQueue()
                                          .AddScheduler()
                                          .AddPrintView()
                                          .BuildProvider();

            return serviceProvider;
        }

        public void Configure(IApplicationBuilder app, IContainerResolver resolver, IApplicationLifetime lifetime)
        {
            app.UseInfinniMiddlewares(resolver, lifetime);
        }
    }
}