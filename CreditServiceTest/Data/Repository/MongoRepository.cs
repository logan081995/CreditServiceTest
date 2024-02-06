using CreditServiceTest.Data.Repository.Interface;
using CreditServiceTest.Models;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace CreditServiceTest.Data.Repository
{
    public class MongoRepository<TDocument> : IMongoRepository<TDocument> where TDocument : IDocument
    { 
        private readonly IMongoClient _client;
        private readonly IMongoCollection<TDocument> _collection;

        public MongoRepository(IMongoDbSettings settings)
        {
            _client = new MongoClient(settings.ConnectionString);

            var database = _client.GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<TDocument>(GetCollectionName(typeof(TDocument)));
        }

        private string GetCollectionName(Type documentType)
        {
            var attribute = documentType.GetCustomAttribute<BsonCollectionAttribute>();
            return attribute?.CollectionName ?? string.Empty;
        }

        public virtual IMongoQueryable<TDocument> GetQueryable()
        {
            return _collection.AsQueryable();
        }

        public virtual async Task<IEnumerable<TDocument>> FindAllAsync(IMongoQueryable<TDocument> collectionQuery)
        {
            return await collectionQuery.ToListAsync();
        }

        public virtual async Task<bool> InsertOneAsync(TDocument document)
        {
            try
            {
                await _collection.InsertOneAsync(document);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while inserting the document: {ex.Message}");
                return false;
            }
        }

        public virtual async Task<bool> ReplaceOneAsync(Expression<Func<TDocument, bool>> filterExpression, TDocument document)
        {
            var result = await _collection.ReplaceOneAsync(filterExpression, document);
            return true;
        }

        public async Task<bool> DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            var result = await _collection.DeleteOneAsync(filterExpression);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }
    }
}
