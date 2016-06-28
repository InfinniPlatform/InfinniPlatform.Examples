using System;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Queues;
using InfinniPlatform.Sdk.Queues.Consumers;

namespace InfinniPlatform.Northwind.Queues
{
    /// <summary>
    /// Пример потребителя сообщений из очереди задач.
    /// </summary>
    public class TaskConsumerOne : TaskConsumerBase<ExampleMessage>
    {
        protected override async Task Consume(Message<ExampleMessage> message)
        {
            await Task.Run(() => Console.WriteLine($"{nameof(TaskConsumerOne)} recieved a message {message.Body}."));
        }
    }


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


    /// <summary>
    /// Пример потребителя сообщений из широковещательной очереди.
    /// </summary>
    [QueueName("DynamicQueue")]
    public class BroadcastConsumerOne : BroadcastConsumerBase<DynamicWrapper>
    {
        protected override async Task Consume(Message<DynamicWrapper> message)
        {
            await Task.Run(() => Console.WriteLine($"{nameof(BroadcastConsumerOne)} recieved a message {message.Body["Example"]}."));
        }
    }


    /// <summary>
    /// Пример потребителя сообщений из широковещательной очереди.
    /// </summary>
    [QueueName("DynamicQueue")]
    public class BroadcastConsumerTwo : BroadcastConsumerBase<DynamicWrapper>
    {
        protected override async Task Consume(Message<DynamicWrapper> message)
        {
            await Task.Run(() => Console.WriteLine($"{nameof(BroadcastConsumerTwo)} recieved a message {message.Body["Example"]}."));
        }
    }
}