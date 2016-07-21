using System;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Services;

namespace InfinniPlatform.Northwind.RazorViews
{
    /// <summary>
    /// Пример сервиса, возвращающего Razor-пресдтавления.
    /// </summary>
    /// <remarks>
    /// Подробнее http://infinniplatform.readthedocs.io/ru/latest/14-view-engine/index.html.
    /// </remarks>
    public class RazorViewsHttpService : IHttpService
    {
        public void Load(IHttpServiceBuilder builder)
        {
            builder.ServicePath = "/razor";
            // Возвращаем Razor-представление Index.cshtml, передавая динамическую модель данных.
            builder.Get["/Index"] = httpRequest => Task.FromResult<object>(new ViewHttpResponce("Index", new DynamicWrapper
                                                                                                         {
                                                                                                             { "Title", "Title" },
                                                                                                             { "Data1", "Somedata" },
                                                                                                             { "Data2", DateTime.Now }
                                                                                                         }));
            // Возвращаем Razor-представление About.cshtml, не принимающее модель данных.
            builder.Get["/About"] = httpRequest1 => Task.FromResult<object>(new ViewHttpResponce("About"));
        }
    }
}