using System.Threading.Tasks;

using InfinniPlatform.DocumentStorage.Contract;
using InfinniPlatform.Sdk.Http.Services;

namespace Infinni.Demo.HttpServices
{
    public class DatabaseService : IHttpService
    {
        public DatabaseService(IDocumentStorageFactory storageFactory)
        {
            _products = storageFactory.GetStorage("Products");
        }

        private readonly IDocumentStorage _products;

        public void Load(IHttpServiceBuilder builder)
        {
            builder.Get["Products"] = Products;
        }

        private async Task<object> Products(IHttpRequest httpRequest)
        {
            var productList = await _products.Find()
                                             .Limit(20)
                                             .ToListAsync();

            return new JsonHttpResponse(productList);
        }
    }
}