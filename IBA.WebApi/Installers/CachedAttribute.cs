using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Text;

namespace IBA.WebApi.Installers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _timeToLiveSeconds;
        /// <summary>
        /// Saniye cinsinden önbellek süresi
        /// </summary>
        /// <param name="timeToLiveSeconds"></param>
        public CachedAttribute(int timeToLiveSeconds)
        {
            _timeToLiveSeconds = timeToLiveSeconds;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.HttpContext.Request.Method != "GET")//Sadece Get olan metodlarda önbellek kontrol çalışacak. Get Haricinde çalışmayacak.
            {
                await next();
                return;
            }
            var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);//Genel request ve parametrelerine göre bir anahtar oluşturur.
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<IMemoryCache>();
            if (cacheService.TryGetValue(cacheKey, out string cachedValue))//İlgili anahtara ait önbellekte veri olup olmadığını sorgular varsa döner
            {
                var contentResult = new ContentResult
                {
                    Content = cachedValue,
                    ContentType = "application/json",
                    StatusCode = 200
                };
                context.Result = contentResult;
                return;
            }

            var executedContext = await next();//Önbellekte veri yoksa, ilgili controller hata vermedi ve 200 döndü ise aynı anahtar ile önbelleğe yazar.
            if (executedContext.Exception == null && executedContext.HttpContext.Response.StatusCode == 200)
            {
                DefaultContractResolver contractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                };
                var result = JsonConvert.SerializeObject(((ObjectResult)executedContext.Result).Value, new JsonSerializerSettings
                {
                    ContractResolver = contractResolver
                });
                cacheService.Set(cacheKey, result, new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddSeconds(_timeToLiveSeconds),
                    Priority = CacheItemPriority.Normal
                });


            }
        }

        private static string GenerateCacheKeyFromRequest(HttpRequest request)
        {
            var keyBuilder = new StringBuilder();

            keyBuilder.Append($"{request.Path}");

            foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
            {
                keyBuilder.Append($"|{key}-{value}");
            }

            return keyBuilder.ToString();
        }
        //medium.com/berkut-teknoloji/in-memory-cache-use-of-memory-caching-in-net-core-41d99153ebd0
        //gencayyildiz.com/blog/asp-net-corede-in-memory-cache/
    }
}
