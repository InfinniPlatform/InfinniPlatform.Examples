using System.Threading.Tasks;

using InfinniPlatform.Caching.Contract;
using InfinniPlatform.Sdk.Http.Services;

namespace InfinniPlatform.Northwind.Cache
{
    /// <summary>
    /// HTTP-сервис для работы с кэшем.
    /// </summary>
    /// <remarks>
    /// Кэш может работать в двух режимах: 'Memory' и 'Shared' (устанавливается в секции 'cache', в значении 'Type' конфигурационого файла).
    /// В режиме 'Memory' кэш хранится в памяти и очищается при остановке приложения.
    /// В режиме 'Shared' кэш хранится в памяти, но также дублируется в БД Redis, что позволяет разделить его между несколькими экземплярами одного приложения.
    /// Подробнее http://infinniplatform.readthedocs.io/ru/latest/11-cache/index.html
    /// </remarks>
    public class CacheHttpService : IHttpService
    {
        public CacheHttpService(ICache cache)
        {
            _cache = cache;
        }

        private readonly ICache _cache;

        public void Load(IHttpServiceBuilder builder)
        {
            builder.ServicePath = "Cache";
            builder.Get["Set"] = Set;
            builder.Get["Get"] = Get;
        }

        /// <summary>
        /// Задает значение в кэше по ключу.
        /// </summary>
        /// <param name="httpRequest">HTTP-запрос</param>
        /// <example> Пример запроса: http://localhost:9900/Cache/Set?key=key&value=value </example>
        private Task<object> Set(IHttpRequest httpRequest)
        {
            //Получаем ключ и значение из параметров (query) HTTP-запроса.
            var key = httpRequest.Query["key"];
            var value = httpRequest.Query["value"];

            //Сохраняем ключ и значение в кэш.
            _cache.Set(key, value);

            return Task.FromResult<object>($"Set: {key}={value}");
        }

        /// <summary>
        /// по
        /// </summary>
        /// <param name="httpRequest">HTTP-запрос</param>
        /// <example> Пример запроса: http://localhost:9900/Cache/Get?key=key </example>
        private Task<object> Get(IHttpRequest httpRequest)
        {
            //Получаем ключ из параметров (query) HTTP-запроса.
            var key = httpRequest.Query["key"];

            //Получаем значение ключа в кэше.
            var value = _cache.Get(key);

            return Task.FromResult<object>($"Get: {key}={value}");
        }
    }
}