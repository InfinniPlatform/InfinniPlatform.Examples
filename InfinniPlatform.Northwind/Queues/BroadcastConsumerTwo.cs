using System;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Queues;
using InfinniPlatform.Sdk.Queues.Consumers;

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
            await Task.Run(() => Console.WriteLine($"{nameof(BroadcastConsumerTwo)} recieved a message {message.Body["Example"]}."));
        }
    }
}