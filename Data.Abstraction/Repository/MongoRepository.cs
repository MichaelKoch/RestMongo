
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Data.Attributes;
using Data.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Data.Repositories
{
    public class MongoRepository<TDocument> : IMongoRepository<TDocument>
        where TDocument : IDocument
    {
        private readonly IMongoCollection<TDocument> _collection;

        public MongoRepository(IMongoDbSettings settings)
        {
            var database = new MongoClient(settings.ConnectionString).GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<TDocument>(GetCollectionName(typeof(TDocument)));

            // var _bsoncollection = database.GetCollection<BsonDocument>(GetCollectionName(typeof(TDocument)));
            // Task.Run(async () => TailCollectionAsync(_bsoncollection)).Wait();
        }

        private static async Task TailCollectionAsync(IMongoCollection<BsonDocument> collection)
        {
            // Set lastInsertDate to the smallest value possible
            BsonValue lastInsertDate = BsonMinKey.Value;

            var options = new FindOptions<BsonDocument>
            {
                // Our cursor is a tailable cursor and informs the server to await
                CursorType = CursorType.TailableAwait
            };

            // Initially, we don't have a filter. An empty BsonDocument matches everything.
            BsonDocument filter = new();

            // NOTE: This loops forever. It would be prudent to provide some form of 
            // an escape condition based on your needs; e.g. the user presses a key.
            while (true)
            {
                try
                {
                    using (var cursor = await collection.FindAsync(filter, options))
                    {

                        // This callback will get invoked with each new document found
                        await cursor.ForEachAsync(document =>
                        {
                            try
                            {
                                
                                // Write the document to the console.
                                Console.WriteLine(document.ToJson());
                            }
                            catch (Exception ex)
                            {

                            }

                        }).ConfigureAwait(false);
                    }
                }
                catch (Exception ex)
                {

                }
                // Start the cursor and wait for the initial response


                // The tailable cursor died so loop through and restart it
                // Now, we want documents that are strictly greater than the last value we saw
              
            }
        }

        private protected string GetCollectionName(Type documentType)
        {
            return ((BsonCollectionAttribute)documentType.GetCustomAttributes(
                    typeof(BsonCollectionAttribute),
                    true)
                .FirstOrDefault())?.CollectionName;
        }

        public virtual IQueryable<TDocument> AsQueryable()
        {
            return _collection.AsQueryable();
        }

        public virtual IEnumerable<TDocument> FilterBy(
            Expression<Func<TDocument, bool>> filterExpression)
        {
            return _collection.Find(filterExpression).ToEnumerable();
        }

        public virtual IEnumerable<TProjected> FilterBy<TProjected>(
            Expression<Func<TDocument, bool>> filterExpression,
            Expression<Func<TDocument, TProjected>> projectionExpression)
        {
            return _collection.Find(filterExpression).Project(projectionExpression).ToEnumerable();
        }

        public virtual TDocument FindOne(Expression<Func<TDocument, bool>> filterExpression)
        {
            return _collection.Find(filterExpression).FirstOrDefault();
        }

        public virtual Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            return Task.Run(() => _collection.Find(filterExpression).FirstOrDefaultAsync());
        }

        public virtual TDocument FindById(string id)
        {
            var objectId = new ObjectId(id);
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, objectId);
            return _collection.Find(filter).SingleOrDefault();
        }

        public virtual Task<TDocument> FindByIdAsync(string id)
        {
            return Task.Run(() =>
            {
                var objectId = new ObjectId(id);
                var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, objectId);
                return _collection.Find(filter).SingleOrDefaultAsync();
            });
        }

        private void setChangedDate(TDocument document)
        {
           long timestamp =  DateTime.UtcNow.Ticks;
           document.ChangedAt = timestamp;
        }
         private void setChangedDate(ICollection<TDocument> documents)
        {
           long timestamp =  DateTime.UtcNow.Ticks;
           foreach(var d in documents)
           {
               d.ChangedAt = timestamp;
           }
          }


        public virtual void InsertOne(TDocument document)
        {
            setChangedDate(document);
            _collection.InsertOne(document);
        }

        public virtual Task InsertOneAsync(TDocument document)
        {
             setChangedDate(document);
            return Task.Run(() => _collection.InsertOneAsync(document));
        }

        public void InsertMany(ICollection<TDocument> documents)
        {
            setChangedDate(documents);
            _collection.InsertMany(documents);
        }


        public virtual async Task InsertManyAsync(ICollection<TDocument> documents)
        {
            setChangedDate(documents);
            await _collection.InsertManyAsync(documents);
        }

        public void ReplaceOne(TDocument document)
        {
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, document.Id);
              setChangedDate(document);
            _collection.FindOneAndReplace(filter, document);
        }

        public virtual async Task ReplaceOneAsync(TDocument document)
        {
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, document.Id);
             setChangedDate(document);
            await _collection.FindOneAndReplaceAsync(filter, document);
        }

        public void DeleteOne(Expression<Func<TDocument, bool>> filterExpression)
        {
            _collection.FindOneAndDelete(filterExpression);
        }

        public Task DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            return Task.Run(() => _collection.FindOneAndDeleteAsync(filterExpression));
        }

        public void DeleteById(string id)
        {
            var objectId = new ObjectId(id);
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, objectId);
            _collection.FindOneAndDelete(filter);
        }

        public Task DeleteByIdAsync(string id)
        {
            return Task.Run(() =>
            {
                var objectId = new ObjectId(id);
                var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, objectId);
                _collection.FindOneAndDeleteAsync(filter);
            });
        }

        public void DeleteMany(Expression<Func<TDocument, bool>> filterExpression)
        {
            _collection.DeleteMany(filterExpression);
        }

        public Task DeleteManyAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            return Task.Run(() => _collection.DeleteManyAsync(filterExpression));
        }
    }
}