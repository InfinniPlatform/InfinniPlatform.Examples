﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Queues.Consumers;
using InfinniPlatform.Sdk.Queues.Producers;
using InfinniPlatform.Sdk.Services;

namespace InfinniPlatform.Northwind.Queues
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
            builder.ServicePath = "queue";
            builder.Get["send"] = SendMessages;
            builder.Get["get"] = GetMessages;
        }

        /// <summary>
        /// Получает сообщение из очереди.
        /// </summary>
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

        /// <summary>
        /// Отправляет сообщения в очереди.
        /// </summary>
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

            // Преобразуем сообщения в DynamicWrapper.
            var dynamicWrapperMessages = exampleMessages.Select(exampleMessage => new DynamicWrapper
                                                                                  {
                                                                                      { "Example", exampleMessage }
                                                                                  })
                                                        .ToArray();

            // Отправляем сообщения в широковещательную очередь.
            foreach (var message in dynamicWrapperMessages)
            {
                // Отправлять сообщения с типа DynamicWrapper, следует используя методы PublishDynamic/PublishDynamicAsync, в противном случае метод выбросит ArgumentException.
                // Т.к. в качестве имени очереди по умолчанию берется имя типа, отправка DynamicWrapper может привести к ошибочной обработке сообщений. 
                // Чтобы избежать такой ситуации необходимо использовать именованную очередь.
                await _broadcastProducer.PublishDynamicAsync(message, "DynamicQueue");
            }

            return $"{exampleMessages.Count + dynamicWrapperMessages.Length} messages successfully sended.";
        }
    }
}