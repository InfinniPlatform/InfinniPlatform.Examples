using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.Sdk.Queues;
using InfinniPlatform.Sdk.Services;

namespace InfinniPlatform.Northwind.IoC
{
    /// <summary>
    /// Регистрация компонентов в IoC-контейнере.
    /// </summary>
    public sealed class NorthwindContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            //Получение информации о текущей сборке.
            var assembly = typeof(NorthwindContainerModule).Assembly;

            //Пример регистрация отдельных потребителей.
            /*builder.RegisterType<TaskConsumerOne>()
                   .As<ITaskConsumer>()
                   .SingleInstance();

            builder.RegisterType<TaskConsumerTwo>()
                   .As<ITaskConsumer>()
                   .SingleInstance();*/

            //Регистрация всех потребителей сообщений.
            builder.RegisterConsumers(assembly);

            //Регистрация всех HTTP-сервисов.
            builder.RegisterHttpServices(assembly);
        }
    }
}