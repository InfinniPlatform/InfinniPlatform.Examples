using System;

using InfinniPlatform.Extensions;
using InfinniPlatform.Sdk.IoC;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Infinni.Demo
{
    public class Startup
    {
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var serviceProvider = services.AddDocumentStorage()
                                          .AddLog4NetAdapter()
                                          .BuildProvider();

            return serviceProvider;
        }

        public void Configure(IApplicationBuilder app, IContainerResolver resolver, IApplicationLifetime lifetime)
        {
            app.UseInfinniMiddlewares(resolver, lifetime);
        }
    }
}