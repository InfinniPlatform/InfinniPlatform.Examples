using System.Reflection;

using InfinniPlatform.Sdk.Http.Services;
using InfinniPlatform.Sdk.IoC;

namespace Infinni.Demo.IoC
{
    public class DemoModule :IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterHttpServices(typeof(DemoModule).GetTypeInfo().Assembly);
        }
    }
}