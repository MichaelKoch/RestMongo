
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Data;
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
        private long getServerTimeStamp()
        {
            return this._collection.Database.getServerTimeStap();
        }
        private void setChangedDate(TDocument document)
        {
            long timestamp = getServerTimeStamp();
            document.ChangedAt = timestamp;
        }
        private void setChangedDate(ICollection<TDocument> documents)
        {
            long timestamp = getServerTimeStamp();
            foreach (var d in documents)
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

        public void DeleteById(ObjectId id)
        {
             DeleteById( new List<ObjectId>(){id});
        }
         public void DeleteById(List<ObjectId> ids)
        {
            var filter = Builders<TDocument>.Filter.In(doc => doc.Id, ids);
            var r = _collection.DeleteManyAsync(filter).Result;
        }

        public void DeleteById(string id)
        {
            DeleteById(new ObjectId(id));
        }
        public void DeleteById(IList<string> ids)
        {
           DeleteById(ids.Select(i=> new ObjectId(i)).ToList());
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


        // private void DeleteMany(Expression<Func<TDocument, bool>> filterExpression)
        // {
        //                _collection.DeleteMany(filterExpression.);

        // }

        // private Task DeleteManyAsync(Expression<Func<TDocument, bool>> filterExpression)
        // {
        //     return Task.Run(() => _collection.DeleteManyAsync(filterExpression));
        // }
    }
}