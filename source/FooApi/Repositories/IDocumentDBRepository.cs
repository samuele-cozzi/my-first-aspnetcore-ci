using System.Threading.Tasks;

namespace FooApi.Repositories {
    
    public interface IDocumentDBRepository<T>
    {
        Task<T> GetItemAsync(string id);
        Task<string> CreateItemAsync(T item);
        Task UpdateItemAsync(T item);
        Task DeleteItemAsync(string id);
    }
}