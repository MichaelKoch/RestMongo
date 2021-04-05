using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Data.Attributes;
using Data.Enums;
using Data.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Bindings;
using MongoDB.Driver.Core.Operations;

namespace Data.Repositories
{
    public class MongoEvents
    {

        public IMongoCollection<IMongoEvent> _collection;
        public IMongoCollection<BsonDocument> _eventSource;
        private readonly IMongoDatabase _database;

        public MongoEvents(IMongoDbSettings settings, string collectionName = "entityEvents")
        {
            _database = new MongoClient(settings.ConnectionString).GetDatabase(settings.DatabaseName);
            _collection = this.getOrCreateCollection();
            _eventSource = this._database.GetCollection<BsonDocument>(collectionName);
           // Task.Run(async () => TailCollectionAsync()).Wait();
        }

        public void Publish(List<IDocument> changes, ChangeTypeEnum changeType)
        {
            var events = changes.Select(c => new MongoEvent()
            {
                CollectionName = GetCollectionName(c.GetType()),
                ObjectId = c.Id.ToString(),
                ChangeAt = c.ChangedAt
            });
            this._collection.InsertManyAsync(events).Wait();
        }

        private protected string GetCollectionName(Type documentType)
        {
            return ((BsonCollectionAttribute)documentType.GetCustomAttributes(
                    typeof(BsonCollectionAttribute),
                    true)
                .FirstOrDefault())?.CollectionName;
        }
        private long getServerTicks()
        {
            var serverStatusCmd = new BsonDocumentCommand<BsonDocument>(new BsonDocument { { "serverStatus", 1 } }); 
                return _database.RunCommand(serverStatusCmd)["localTime"].ToUniversalTime().Ticks;
                
        }
        private async Task TailCollectionAsync()
        {
            long lastKnowChangeDate = 0;
            // if(lastEntry != null)
            // {
            //     lastKnowChangeDate = lastEntry.ChangeAt;
            // }

            var options = new FindOptions<BsonDocument>
            {
                // Our cursor is a tailable cursor and informs the server to await
                CursorType = MongoDB.Driver.CursorType.TailableAwait,
            };
            var filter = new BsonDocument();
            while (true)
            {
                try
                {
                    using (var cursor = await this._eventSource.FindAsync(filter, options))
                    {
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

                        });
                    }
                }

                catch (Exception ex)
                {

                }
            }
        }
        private IMongoCollection<IMongoEvent> getOrCreateCollection(string collectionName = "entityEvents")
        {
            IMongoCollection<IMongoEvent> retVal = null;
            var collections = this._database.ListCollectionNames().ToList();
            if (!collections.Contains(collectionName))
            {
                this._database.CreateCollection(collectionName, new CreateCollectionOptions()
                {
                    MaxDocuments = 50000,
                    MaxSize = (10 * 1024 * 1024),
                    Capped = true
                });
            }
            retVal = _database.GetCollection<IMongoEvent>(collectionName);
            return retVal;
        }
    }
}