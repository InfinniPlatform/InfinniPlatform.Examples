using System;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Queues;
using InfinniPlatform.Sdk.Queues.Consumers;

namespace InfinniPlatform.Northwind.Queues
{
    /// <summary>
    /// Пример потребителя сообщений из очереди задач.
    /// </summary>
    public class TaskConsumerTwo : TaskConsumerBase<ExampleMessage>
    {
        protected override async Task Consume(Message<ExampleMessage> message)
        {
            await Task.Run(() => Console.WriteLine($"{nameof(TaskConsumerTwo)} recieved a message {message.Body}."));
        }
    }
}