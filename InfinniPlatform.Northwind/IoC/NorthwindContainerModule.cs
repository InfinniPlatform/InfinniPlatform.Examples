using InfinniPlatform.Scheduler.Contract;
using InfinniPlatform.Sdk.Http.Services;
using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.MessageQueue.Contract;

namespace InfinniPlatform.Northwind.IoC
{
    /// <summary>
    /// Регистрация компонентов в IoC-контейнере.
    /// </summary>
    public sealed class NorthwindContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            // Получение информации о текущей сборке
            var assembly = typeof(NorthwindContainerModule).Assembly;

            /*

            // Регистрация потребителя сообщений TaskConsumerOne
            builder.RegisterType<TaskConsumerOne>()
                   .As<ITaskConsumer>()
                   .SingleInstance();

            // Регистрация потребителя сообщений TaskConsumerTwo
            builder.RegisterType<TaskConsumerTwo>()
                   .As<ITaskConsumer>()
                   .SingleInstance();

            */

            // Регистрация всех потребителей сообщений
            builder.RegisterConsumers(assembly);

            // Регистрация всех HTTP-сервисов.
            builder.RegisterHttpServices(assembly);

            // Регистрация всех обработчиков планировщика заданий
            builder.RegisterJobHandlers(assembly);

            // Регистрация всех источников планировщика заданий
            builder.RegisterJobInfoSources(assembly);
        }
    }
}