
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

namespace MongoBase.Repositories
{
    public class Repository<TDocument> : IRepository<TDocument>
                 where TDocument : IDocument
    {

        private readonly IMongoCollection<TDocument> _collection;

        public IMongoCollection<TDocument> Collection => _collection;

        public void StoreSyncDelta(IList<TDocument> delta)
        {
            IQueryable<TDocument> query = this.AsQueryable();
            List<TDocument> inserts = new();
            List<TDocument> updates = new();
            if (_collection.AsQueryable().Where(c => c.ChangedAt > 0).Any())
            {
                foreach (var p in delta)
                {
                    var exists = query.Where(c => c.Id == p.Id).Any();
                    if (exists)
                    {
                        updates.Add(p);
                    }
                    else
                    {
                        inserts.Add(p);
                    }
                }
            }
            else
            {
                inserts.AddRange(delta);
            }
            var waitfor = new List<Task>();
            if (inserts.Count > 0)
            {
                waitfor.Add(this._collection.InsertManyAsync(inserts));
            }
            foreach (var u in updates)
            {
                waitfor.Add(this._collection.ReplaceOneAsync(i => i.Id == u.Id, u));
            }
            Task.WaitAll(waitfor.ToArray());
        }
        public long GetMaxLastChanged()
        {
            long retVal = 0;
            int total = this.AsQueryable().Count();
            if (total > 0)
            {
                retVal = this.AsQueryable().Max(c => c.ChangedAt);
            }
            return retVal;
        }
   
        public Repository(IConnectionSettings settings)
        {
            var database = new MongoClient(settings.ConnectionString).GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<TDocument>(GetCollectionName(typeof(TDocument)));
        }

        public PagedResultModel<TDocument> Query(string query, string orderby = null, int top = 1000, int skip = 0)
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

            return this.Query(query, orderbyDict, top, skip);
        }

        public IList<TDocument> Query(FilterDefinition<TDocument> filter)
        {
            return this._collection.Find(filter).ToList();
        }


        public PagedResultModel<TDocument> Query(string query, Dictionary<string, string> orderby = null, int top = 1000, int skip = 0)
        {
            var retVal = new PagedResultModel<TDocument>();
            if ((orderby == null) || (orderby.Count == 0))
            {
                orderby = new Dictionary<string, string>
                {
                    { "Id", "asc" }
                };
            }
            retVal.Total = (int)this._collection.CountDocuments(query);
            var matchInfo = new BsonDocument { { "$match", BsonDocument.Parse(query) } };
            var skipInfo = new BsonDocument { { "$skip", skip } };
            var topInfo = new BsonDocument { { "$limit", top } };
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
            PipelineDefinition<TDocument, TDocument> pipeline = new BsonDocument[]
            {
                matchInfo,
                topInfo,
                skipInfo,
                sortInfoDoc
            };
            var results = this._collection.Aggregate<TDocument>(pipeline);
            retVal.Values = results.ToList();
            retVal.Skip = skip;
            retVal.Top = top;
            return retVal;
        }
        private protected static string GetCollectionName(Type documentType)
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
            return _collection.AsQueryable().SingleOrDefault(c => c.Id == id);
        }
        public virtual Task<TDocument> FindByIdAsync(string id)
        {
            return Task.Run(() => _collection.AsQueryable().SingleOrDefault(c => c.Id == id));
        }
        private long GetServerTimeStamp()
        {
            return new DateTime(this._collection.Database.GetServerTimeStap()).Ticks;
        }
        private void SetChangedDate(TDocument document)
        {
            long timestamp = GetServerTimeStamp();
            document.ChangedAt = timestamp;
        }
        private void SetChangedDate(ICollection<TDocument> documents)
        {
            long timestamp = GetServerTimeStamp();
            foreach (var d in documents)
            {
                d.ChangedAt = timestamp;
            }
        }

        public virtual TDocument InsertOne(TDocument document)
        {
            SetChangedDate(document);
            _collection.InsertOne(document);
            return document;
        }

        public virtual Task InsertOneAsync(TDocument document)
        {
            SetChangedDate(document);
            return Task.Run(() => _collection.InsertOneAsync(document));
        }

        public void InsertMany(ICollection<TDocument> documents)
        {
            SetChangedDate(documents);
            _collection.InsertMany(documents);
        }


        public virtual async Task InsertManyAsync(ICollection<TDocument> documents)
        {
            SetChangedDate(documents);
            await _collection.InsertManyAsync(documents).ConfigureAwait(false);
        }

        public void ReplaceOne(TDocument document)
        {
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, document.Id);
            SetChangedDate(document);
            _collection.ReplaceOne(filter, document);
        }

        public virtual async Task ReplaceOneAsync(TDocument document)
        {
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, document.Id);
            SetChangedDate(document);
            await _collection.FindOneAndReplaceAsync(filter, document).ConfigureAwait(false);
        }

        public void DeleteById(string id)
        {
            DeleteById(new List<string>() { id });
        }

        public void DeleteById(List<string> ids)
        {
            var filter = Builders<TDocument>.Filter.In(doc => doc.Id, ids);
            var r = _collection.DeleteManyAsync(filter).Result;
        }

        public void DeleteById(IList<string> ids)
        {
            DeleteById(ids);
        }

        public Task DeleteByIdAsync(string id)
        {
            return Task.Run(() =>
            {
                var objectId = new ObjectId(id);
                var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, id);
                _collection.FindOneAndDeleteAsync(filter);
            });
        }
    }
}