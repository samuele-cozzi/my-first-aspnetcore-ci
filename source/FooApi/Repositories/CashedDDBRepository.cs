using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System;
using System.Configuration;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace FooApi.Repositories
{
    public class CachedDDBRepository<T> : IDocumentDBRepository<T> where T: class 
    {
        private string DatabaseId = string.Empty;
        private string CollectionId = string.Empty;
        private static DocumentClient client;
        private IDistributedCache cache;

        public CachedDDBRepository(IConfiguration configuration, IDistributedCache _cache, string configuration_section)
        {
            this.cache = _cache;
            DatabaseId = configuration.GetValue<string>(string.Format("{0}:database", configuration_section));
            CollectionId = configuration.GetValue<string>(string.Format("{0}:collection", configuration_section));

            client = new DocumentClient(
                new Uri(configuration.GetValue<string>(string.Format("{0}:endpoint", configuration_section))),
                configuration.GetValue<string>(string.Format("{0}:authKey", configuration_section)));

            CreateDatabaseIfNotExistsAsync().Wait();
            CreateCollectionIfNotExistsAsync().Wait();
        }

        private async Task CreateDatabaseIfNotExistsAsync()
        {
            try
            {
                await client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(DatabaseId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await client.CreateDatabaseAsync(new Database { Id = DatabaseId });
                }
                else
                {
                    throw;
                }
            }
        }

        private async Task CreateCollectionIfNotExistsAsync()
        {
            try
            {
                await client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await client.CreateDocumentCollectionAsync(
                        UriFactory.CreateDatabaseUri(DatabaseId),
                        new DocumentCollection { Id = CollectionId },
                        new RequestOptions { OfferThroughput = 1000 });
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<T> GetItemAsync(string id)
        {
            try
            {
                var value = cache.GetString(id);
                if (value == null){
                    Document document =
                        await client.ReadDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id));
                    return (T)(dynamic)document;
                }
                else {
                    return JsonConvert.DeserializeObject<T>(value);
                }
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return default(T);
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<IEnumerable<T>> GetItemsAsync(Expression<Func<T, bool>> predicate)
        {
            IDocumentQuery<T> query = client.CreateDocumentQuery<T>(
                UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId),
                new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true})
                .Where(predicate)
                .AsDocumentQuery();

            List<T> results = new List<T>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<T>());
            }

            return results;
        }

        public async Task<string> CreateItemAsync(T item)
        {
            Document doc = await client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId), item);
            string json = JsonConvert.SerializeObject(doc);
            cache.SetString(doc.Id, json);
            return doc.Id;
        }

        public async Task UpdateItemAsync(T item)
        {
            Document doc = await client.UpsertDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId), item);
            string json = JsonConvert.SerializeObject(doc);
            cache.SetString(doc.Id, json);
        }

        public async Task DeleteItemAsync(string id)
        {
            Document doc = await client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id));
            cache.Remove(id);
        }
    }
}