using System.Reflection;

using InfinniPlatform.DocumentStorage;
using InfinniPlatform.Http;
using InfinniPlatform.IoC;

namespace Infinni.Demo.IoC
{
    public class DemoModule :IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterHttpServices(typeof(DemoModule).GetTypeInfo().Assembly);
            builder.RegisterDocumentHttpService("Shippers");
        }
    }
}