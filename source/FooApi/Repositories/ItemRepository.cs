using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Caching.Distributed;

namespace FooApi.Repositories
{
    public class ItemRepository<T> : CachedDDBRepository<T> where T: class 
    {
        public ItemRepository(IConfiguration configuration, IDistributedCache _cache) : base (configuration, _cache, "CosmosDB") {}
    }
}