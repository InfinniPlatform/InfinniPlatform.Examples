using System;
using System.Threading.Tasks;

using InfinniPlatform.MessageQueue;

namespace Infinni.Northwind.Queues
{
    /// <summary>
    /// Пример потребителя сообщений из очереди задач.
    /// </summary>
    public class TaskConsumerOne : TaskConsumerBase<ExampleMessage>
    {
        protected override async Task Consume(Message<ExampleMessage> message)
        {
            // Обработка сообщения.
            // Получаем сообщение и выводим его содержимое в консоль.
            await Task.Run(() => Console.WriteLine($"{nameof(TaskConsumerOne)} recieved a message [{message.Body}]."));
        }
    }
}