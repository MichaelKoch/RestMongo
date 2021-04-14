using MongoBase.Models;
using MongoDB.Driver;
using Sample.Domain.ProductV1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Domain.ProductV1
{
    public class ProductRepository : MongoBase.Repositories.Repository<Product>
    {
        public ProductRepository(ConnectionSettings connecttionSettings) : base(connecttionSettings)
        {}
        public void StoreSyncDelta(IList<Product> delta)
        {
            IQueryable<Product> query = this.AsQueryable();
            List<Product> inserts = new();
            List<Product> updates = new();
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
                waitfor.Add(this._collection.ReplaceOneAsync(i=>i.Id == u.Id,u));
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
    }
}
