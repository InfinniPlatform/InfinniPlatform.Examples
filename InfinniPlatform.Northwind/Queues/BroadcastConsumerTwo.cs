using System;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.MessageQueue.Contract;
using InfinniPlatform.MessageQueue.Contract.Consumers;
using InfinniPlatform.Sdk.Serialization;

namespace InfinniPlatform.Northwind.Queues
{
    /// <summary>
    /// Пример потребителя сообщений из широковещательной очереди.
    /// </summary>
    /// <remarks>
    /// Для отправки сообщения в определенную очередь используется атрибут <see cref="QueueNameAttribute" />
    /// </remarks>
    [QueueName("DynamicQueue")]
    public class BroadcastConsumerTwo : BroadcastConsumerBase<DynamicWrapper>
    {
        protected override async Task Consume(Message<DynamicWrapper> message)
        {
            // Обработка сообщения.
            // Получаем сообщение и выводим его содержимое в консоль.
            await Task.Run(() =>
                           {
                               var exampleMessage = JsonObjectSerializer.Default.ConvertFromDynamic<ExampleMessage>(message.Body["Example"]);

                               Console.WriteLine($"{nameof(BroadcastConsumerTwo)} recieved a message [{exampleMessage}].");
                           });
        }
    }
}