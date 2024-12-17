using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Diagnostics;
using System.Net.Http;

namespace MongoApi
{
    public class MongoRepository<T>
    {
        private readonly IMongoCollection<BsonDocument> _collection;

        public MongoRepository(IMongoDatabase database, string collectionName)
        {
            _collection = database.GetCollection<BsonDocument>(collectionName);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var bsonDocuments = await _collection.Find(new BsonDocument()).ToListAsync();
            return bsonDocuments.Select(doc => BsonSerializer.Deserialize<T>(doc));
        }

        public async Task<T> GetByIdAsync(string id)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(id));
            var doc = await _collection.Find(filter).FirstOrDefaultAsync();
            return BsonSerializer.Deserialize<T>(doc);
        }

        public async Task CreateAsync(T entity)
        {
            var document = entity.ToBsonDocument();
            await _collection.InsertOneAsync(document);
        }

        public async Task UpdateAsync(string id, T entity)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(id));
            var document = entity.ToBsonDocument();
            await _collection.ReplaceOneAsync(filter, document);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            if (!ObjectId.TryParse(id, out ObjectId objectId))
            {
                throw new ArgumentException("Invalid ID format. ID must be a 24-character hexadecimal string.");
            }

            var filter = Builders<BsonDocument>.Filter.Eq("_id", objectId);
            var result = await _collection.DeleteOneAsync(filter);
            return result.DeletedCount > 0;
        }

        public async Task<bool> ExistsAsync(string email)
        {
            return true;
        }
    }
}

