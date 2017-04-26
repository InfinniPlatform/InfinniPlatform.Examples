using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using InfinniPlatform.Dynamic;
using InfinniPlatform.Http;
using InfinniPlatform.MessageQueue;

namespace Infinni.Northwind.Queues
{
    /// <summary>
    /// HTTP-сервис для работы с очередью сообщений.
    /// </summary>
    public class MessageProducerHttpService : IHttpService
    {
        public MessageProducerHttpService(ITaskProducer taskProducer,
                                          IBroadcastProducer broadcastProducer,
                                          IOnDemandConsumer onDemandConsumer)
        {
            _taskProducer = taskProducer;
            _broadcastProducer = broadcastProducer;
            _onDemandConsumer = onDemandConsumer;
        }

        private readonly ITaskProducer _taskProducer;
        private readonly IBroadcastProducer _broadcastProducer;
        private readonly IOnDemandConsumer _onDemandConsumer;

        public void Load(IHttpServiceBuilder builder)
        {
            builder.ServicePath = "Queue";
            builder.Get["Send"] = SendMessages;
            builder.Get["Get"] = GetMessages;
        }

        /// <summary>
        /// Отправляет сообщения в очереди.
        /// </summary>
        /// <example> Пример запроса: http://localhost:9900/Queue/Send </example>
        private async Task<object> SendMessages(IHttpRequest httpRequest)
        {
            // Составляем список сообщений.
            var exampleMessages = new List<ExampleMessage>();
            for (var i = 0; i < 5; i++)
            {
                exampleMessages.Add(new ExampleMessage(i, $"{i}", DateTime.Now));
            }

            // Публикуем сообщения в очередь задач.
            foreach (var exampleMessage in exampleMessages)
            {
                await _taskProducer.PublishAsync(exampleMessage);
            }

            // Публикуем сообщения в очередь задач для получения OnDemandConsumer'ом.
            foreach (var exampleMessage in exampleMessages)
            {
                await _taskProducer.PublishAsync(exampleMessage, "OnDemandQueue");
            }

            // Преобразуем сообщения в DynamicDocument.
            var DynamicDocumentMessages = exampleMessages.Select(exampleMessage => new DynamicDocument
                                                                                  {
                                                                                      { "Example", exampleMessage }
                                                                                  })
                                                        .ToArray();

            // Отправляем сообщения в широковещательную очередь.
            foreach (var message in DynamicDocumentMessages)
            {
                // Отправлять сообщения с типа DynamicDocument, следует используя методы PublishDynamic/PublishDynamicAsync, в противном случае метод выбросит ArgumentException.
                // Т.к. в качестве имени очереди по умолчанию берется имя типа, отправка DynamicDocument может привести к ошибочной обработке сообщений. 
                // Чтобы избежать такой ситуации необходимо использовать именованную очередь.
                await _broadcastProducer.PublishDynamicAsync(message, "DynamicQueue");
            }

            return $"{exampleMessages.Count + DynamicDocumentMessages.Length} messages successfully sended.";
        }

        /// <summary>
        /// Получает сообщение из очереди по запросу.
        /// </summary>
        /// <example> Пример запроса: http://localhost:9900/Queue/Get </example>
        private async Task<object> GetMessages(IHttpRequest httpRequest)
        {
            //Получаем сообщение из очереди с именем "OnDemandQueue".
            var message = await _onDemandConsumer.Consume<ExampleMessage>("OnDemandQueue");

            if (message == null)
            {
                return null;
            }

            //Получаем тело сообщения.
            var body = (ExampleMessage)message.GetBody();

            return $"{body}";
        }
    }
}