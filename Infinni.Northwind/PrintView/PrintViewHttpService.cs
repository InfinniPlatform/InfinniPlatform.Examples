using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

using InfinniPlatform.PrintView.Contract;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Http.Services;

namespace Infinni.Northwind.PrintView
{
    internal class PrintViewHttpService : IHttpService
    {
        public PrintViewHttpService(IPrintViewBuilder printViewBuilder)
        {
            _printViewBuilder = printViewBuilder;
        }


        private readonly IPrintViewBuilder _printViewBuilder;


        public void Load(IHttpServiceBuilder builder)
        {
            builder.ServicePath = "printView";

            builder.Get["pdf"] = r => GetPrintViewExample(PrintViewFileFormat.Pdf);
            builder.Get["html"] = r => GetPrintViewExample(PrintViewFileFormat.Html);
        }


        private Task<object> GetPrintViewExample(PrintViewFileFormat fileFormat)
        {
            // Данные печатного представления
            object dataSource = new DynamicWrapper { { "Date", DateTime.Now } };

            // Сборка, содержащая в ресурсах шаблон печатного представления
            var resourceAssembly = Assembly.GetEntryAssembly();

            // Получение шаблона печатного представления по имени ресурса
            Func<Stream> template = () => resourceAssembly.GetManifestResourceStream("InfinniPlatform.Northwind.PrintView.PrintViewExample.json");

            var response = new PrintViewHttpResponse(_printViewBuilder, template, dataSource, fileFormat);

            return Task.FromResult<object>(response);
        }
    }
}