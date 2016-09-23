using System;
using System.Reflection;
using System.Threading.Tasks;

using InfinniPlatform.PrintView.Contract;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Services;

namespace InfinniPlatform.Northwind.PrintView
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

            builder.Get["pdf"] = r => GetPrintViewExample(PrintViewFileFormat.Pdf, HttpConstants.PdfContentType);
            builder.Get["html"] = r => GetPrintViewExample(PrintViewFileFormat.Html, HttpConstants.HtmlContentType);
        }


        private Task<object> GetPrintViewExample(PrintViewFileFormat printViewFormat, string contentType)
        {
            // Данные печатного представления
            object printViewSource = new DynamicWrapper { { "Date", DateTime.Now } };

            // Сборка, содержащая в ресурсах шаблон печатного представления
            var resourceAssembly = Assembly.GetExecutingAssembly();

            // Получение шаблона печатного представления по имени ресурса
            var printViewTemplate = resourceAssembly.GetManifestResourceStream("InfinniPlatform.Northwind.PrintView.PrintViewExample.json");

            // Создание печатного представления по шаблону и данным
            var printView = _printViewBuilder.Build(printViewTemplate, printViewSource, printViewFormat);

            var response = new StreamHttpResponse(printView, contentType);

            return Task.FromResult<object>(response);
        }
    }
}