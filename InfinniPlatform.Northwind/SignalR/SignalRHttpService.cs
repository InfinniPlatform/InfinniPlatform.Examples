using System;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.PushNotification;
using InfinniPlatform.Sdk.Services;

namespace InfinniPlatform.Northwind.SignalR
{
    /// <summary>
    /// HTTP-сервис для работы с нотификациями на UI.
    /// </summary>
    public class SignalRHttpService : IHttpService
    {
        public SignalRHttpService(IPushNotificationService notifyService)
        {
            _notifyService = notifyService;
        }

        private readonly IPushNotificationService _notifyService;

        public void Load(IHttpServiceBuilder builder)
        {
            builder.ServicePath = "PushNotification";
            builder.Get["/NotifyAll"] = NotifyAll;
        }

        /// <summary>
        /// Оповещает подписчиков сервиса, подписанных на ключ 'HomePage'.
        /// </summary>
        /// <example> Пример запроса: http://localhost:9900/PushNotification/NotifyAll </example>
        private async Task<object> NotifyAll(IHttpRequest httpRequest)
        {
            var messageBody = $"Notification recieved at {DateTime.Now:U}.";
            const string messageType = "HomePage";

            await _notifyService.NotifyAll(messageType, messageBody);

            return messageBody;
        }
    }
}