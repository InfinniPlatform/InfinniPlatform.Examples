using System;
using System.Threading.Tasks;

using InfinniPlatform.MessageQueue.Contract;
using InfinniPlatform.MessageQueue.Contract.Consumers;

namespace InfinniPlatform.Northwind.Queues
{
    /// <summary>
    /// Пример потребителя сообщений из очереди задач.
    /// </summary>
    public class TaskConsumerTwo : TaskConsumerBase<ExampleMessage>
    {
        protected override async Task Consume(Message<ExampleMessage> message)
        {
            // Обработка сообщения.
            // Получаем сообщение и выводим его содержимое в консоль.
            await Task.Run(() => Console.WriteLine($"{nameof(TaskConsumerTwo)} recieved a message [{message.Body}]."));
        }
    }
}