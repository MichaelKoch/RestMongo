
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoBase.Interfaces;
using MongoBase.Attributes;
using System.Text.RegularExpressions;
using MongoBase.Exceptions;

namespace MongoBase.Repositories
{
    public class MongoRepository<TEntity> : IRepository<TEntity>
                 where TEntity : IDocument
    {
        public readonly IConnectionSettings ConnectionSettings;
        private readonly IMongoCollection<TEntity> _collection;
        private readonly IMongoDatabase _database;
        private readonly MongoClient _client;
        private readonly Type documentType = typeof(TEntity);
        public MongoRepository(IConnectionSettings settings)
        {
            ConnectionSettings = settings;
            _client = new MongoClient(settings.ConnectionString);
            _database = _client.GetDatabase(settings.DatabaseName);
            _collection = _database.GetCollection<TEntity>(GetCollectionName(typeof(TEntity)));
        }

        public IMongoCollection<TEntity> Collection => _collection;
        public void StoreSyncDelta(IList<TEntity> delta)
        {
            if (delta == null || delta.Count == 0) return;
            var bulkOps = new List<WriteModel<TEntity>>();
            if ( _collection.AsQueryable().Count() == 0)
            {
                _collection.InsertMany(delta);
            }
            else
            {
                foreach (var record in delta)
                {
                    var upsertOne = new ReplaceOneModel<TEntity>(
                        Builders<TEntity>.Filter.Where(x => x.Id == record.Id),
                        record)
                    { IsUpsert = true };
                    bulkOps.Add(upsertOne);
                }
                _collection.BulkWrite(bulkOps);
            }


           
            }
        public long GetMaxLastChanged()
        {
            if(!documentType.IsAssignableFrom(typeof(IFeedDocument)))
            {
                return 0;
            }
            long retVal = 0;
            int total = this.AsQueryable().Count();
            if (total > 0)
            {
                retVal = this.AsQueryable().Cast<IFeedDocument>().Max(c => c.Timestamp);
            }
            return retVal;
        }
   
        public PagedResultModel<TEntity> Query(string query, string orderby = null, int maxPageSize = 100)
        {
            var orderbyDict = new Dictionary<string, string>();
            orderby = orderby.Replace(";", ",");
            var orderbySplit = orderby.Split(",");
            foreach (string orderbyField in orderbySplit)
            {
                if (!string.IsNullOrWhiteSpace(orderbyField))
                {
                    var regex = new Regex("[ ]{2,}", RegexOptions.None);
                    var orderInfo = regex.Replace(orderbyField, " ");
                    var fieldAndDirection = orderInfo.Split(" ");
                    if (fieldAndDirection.Length == 1)
                    {
                        orderbyDict.Add(fieldAndDirection[0].Trim(), "asc");
                    }
                    if (fieldAndDirection.Length == 2)
                    {
                        orderbyDict.Add(fieldAndDirection[0].Trim(), fieldAndDirection[1].Trim());
                    }
                }
            }
            var retVal    = this.Query(query, orderbyDict,maxPageSize);
        
            return retVal;
        }

  

        public PagedResultModel<TEntity> Query(string query, Dictionary<string, string> orderby = null,int maxPageSize=1000)
        {
            var retVal = new PagedResultModel<TEntity>();
            if ((orderby == null) || (orderby.Count == 0))
            {
                orderby = new Dictionary<string, string>
                {
                    { "Id", "asc" }
                };
            }
            retVal.Total = (int)this._collection.CountDocuments(query);
            if(retVal.Total > maxPageSize)
            {
                throw new PageSizeExeededException($"MAX PAGE SIZE EXEEDED [{maxPageSize}]");
            }
       
            var matchInfo = new BsonDocument { { "$match", BsonDocument.Parse(query) } };
            var sortInfoField = new BsonDocument();
            var sortInfoDoc = new BsonDocument { { "$sort", sortInfoField } };
          
            foreach (string field in orderby.Keys)
            {
                var direction = orderby[field];
                if (string.Equals(direction, "desc", StringComparison.OrdinalIgnoreCase))
                {
                    sortInfoField.Add(field, -1);
                }
                else if ((string.Equals(direction, "asc", StringComparison.OrdinalIgnoreCase)) || (string.Equals(direction, "", StringComparison.OrdinalIgnoreCase)))
                {
                    sortInfoField.Add(field, 1);
                }
                else
                {
                    throw new NotSupportedException("sort direction not supported : " + direction);
                }
            }
            PipelineDefinition<TEntity, TEntity> pipeline = new BsonDocument[]
            {
                matchInfo,
                sortInfoDoc
            };
            var results = this._collection.Aggregate<TEntity>(pipeline);
            retVal.Values = results.ToList();
            retVal.Skip = 0;
            retVal.Top = retVal.Total;
            return retVal;
        }
        private static string GetCollectionName(Type documentType)
        {
            return ((BsonCollectionAttribute)documentType.GetCustomAttributes(
                    typeof(BsonCollectionAttribute),
                    true)
                .FirstOrDefault())?.CollectionName;
        }

        public virtual IQueryable<TEntity> AsQueryable()
        {
            return _collection.AsQueryable();
        }


        public virtual IEnumerable<TEntity> Search(string searchTerm,int maxPageSize = 50)
        {
            var query = Builders<TEntity>.Filter.Text(searchTerm);
            var totalCount = (int)this._collection.CountDocuments(query);
            if (totalCount > maxPageSize)
            {
                throw new PageSizeExeededException($"MAX PAGE SIZE EXEEDED [{maxPageSize}]");
            }
            return _collection.Find(Builders<TEntity>.Filter.Text(searchTerm)).ToList();
            
        }

        public virtual IEnumerable<TEntity> FilterBy(
            Expression<Func<TEntity, bool>> filterExpression)
        {
            return _collection.Find(filterExpression).ToEnumerable();
        }

        public virtual IEnumerable<TProjected> FilterBy<TProjected>(
            Expression<Func<TEntity, bool>> filterExpression,
            Expression<Func<TEntity, TProjected>> projectionExpression)
        {
            return _collection.Find(filterExpression).Project(projectionExpression).ToEnumerable();
        }

        public virtual TEntity FindOne(Expression<Func<TEntity, bool>> filterExpression)
        {
            return _collection.Find(filterExpression).FirstOrDefault();
        }

        public virtual Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> filterExpression)
        {
            return Task.Run(() => _collection.Find(filterExpression).FirstOrDefaultAsync());
        }

        public virtual TEntity FindById(string id)
        {
            return _collection.AsQueryable().SingleOrDefault(c => c.Id == id);
        }
        public virtual Task<TEntity> FindByIdAsync(string id)
        {
            return Task.Run(() => _collection.AsQueryable().SingleOrDefault(c => c.Id == id));
        }
        private long GetServerTimeStamp()
        {
            return new DateTime(this._collection.Database.GetServerTimeStap()).Ticks;
        }
        private void SetChangedDate(IFeedDocument document)
        {
            long timestamp = GetServerTimeStamp();
            document.Timestamp = timestamp;
        }
        private void SetChangedDate(ICollection<IFeedDocument> documents)
        {
            long timestamp = GetServerTimeStamp();
            foreach (var d in documents)
            {
                d.Timestamp = timestamp;
            }
        }

        public virtual TEntity InsertOne(TEntity document)
        {
            if(documentType.IsAssignableTo(typeof(IFeedDocument)))
            {
                SetChangedDate(document as IFeedDocument);
            }
       
            _collection.InsertOne(document);
            return document;
        }

        public virtual Task InsertOneAsync(TEntity document)
        {
            if (documentType.IsAssignableTo(typeof(IFeedDocument)))
            {
                SetChangedDate(document as IFeedDocument);
            }
            return Task.Run(() => _collection.InsertOneAsync(document));
        }

        public void InsertMany(ICollection<TEntity> documents)
        {
            if (documentType.IsAssignableTo(typeof(IFeedDocument)))
            {
                SetChangedDate(documents.Cast<IFeedDocument>().ToList());
            }
            
            _collection.InsertMany(documents);
        }


        public virtual async Task InsertManyAsync(ICollection<TEntity> documents)
        {
            if (documentType.IsAssignableTo(typeof(IFeedDocument)))
            {
             
                SetChangedDate(documents.Cast<IFeedDocument>().ToList());
            }

            await _collection.InsertManyAsync(documents).ConfigureAwait(false);
        }

        public void ReplaceOne(TEntity document)
        {
            var filter = Builders<TEntity>.Filter.Eq(doc => doc.Id, document.Id);
            if (documentType.IsAssignableTo(typeof(IFeedDocument)))
            {
                SetChangedDate(document as IFeedDocument);
            }
            _collection.ReplaceOne(filter, document);
        }

        public virtual async Task ReplaceOneAsync(TEntity document)
        {
            var filter = Builders<TEntity>.Filter.Eq(doc => doc.Id, document.Id);
            if (documentType.IsAssignableTo(typeof(IFeedDocument)))
            {
                SetChangedDate(document as IFeedDocument);
            }
            await _collection.FindOneAndReplaceAsync(filter, document).ConfigureAwait(false);
        }

        public void DeleteById(string id)
        {
            DeleteById(new List<string>() { id });
        }

        public void DeleteById(IList<string> ids)
        {
            var filter = Builders<TEntity>.Filter.In(doc => doc.Id, ids);
            var r = _collection.DeleteManyAsync(filter).Result;
        }
        public Task DeleteByIdAsync(string id)
        {
            return Task.Run(() =>
            {
               return DeleteByIdAsync(new List<string> { id });
            });
        }
        public Task DeleteByIdAsync(IList<string> ids)
        {
            return Task.Run(() =>
            {
                var filter = Builders<TEntity>.Filter.In(doc => doc.Id, ids);
                return  _collection.DeleteManyAsync(filter).Result;
            });
        }
      

        
    }
}