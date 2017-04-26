using System;
using System.Threading.Tasks;

using InfinniPlatform.Dynamic;
using InfinniPlatform.MessageQueue;
using InfinniPlatform.Serialization;

namespace Infinni.Northwind.Queues
{
    /// <summary>
    /// Пример потребителя сообщений из широковещательной очереди.
    /// </summary>
    /// <remarks>
    /// Для отправки сообщения в определенную очередь используется атрибут <see cref="QueueNameAttribute" />
    /// </remarks>
    [QueueName("DynamicQueue")]
    public class BroadcastConsumerOne : BroadcastConsumerBase<DynamicDocument>
    {
        protected override async Task Consume(Message<DynamicDocument> message)
        {
            // Обработка сообщения.
            // Получаем сообщение и выводим его содержимое в консоль.
            await Task.Run(() =>
                           {
                               var exampleMessage = JsonObjectSerializer.Default.ConvertFromDynamic<ExampleMessage>(message.Body["Example"]);

                               Console.WriteLine($"{nameof(BroadcastConsumerOne)} recieved a message [{exampleMessage}].");
                           });
        }
    }
}