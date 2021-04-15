using MongoBase.Repositories;
using Sample.Domain.DataAdapter.Repositories.Abstractions;
using Sample.Domain.Models;
using System.Collections.Generic;
using System.Linq;
namespace Sample.Domain.DataAdapter.Repositories
{
    public class ProductColorSizeSourceRepository:JsonSourceRepository<ProductColorSize>
    {
        public void Sync(Repository<ProductColorSize> repo)
        {
            if (repo is null)
            {
                throw new System.ArgumentNullException(nameof(repo));
            }
            long lastKnowTimestamp = repo.GetMaxLastChanged();
            List<ProductColorSize> delta = this._data.Where(d=>
                d.ChangedAt > lastKnowTimestamp &&
                d.EAN != null
            ).ToList();
            repo.StoreSyncDelta(delta);
        }

        protected override void Transform()
        {
            foreach(var i in this._data)
            {
                i.MaterialNumber = long.Parse(i.MaterialNumber).ToString();
                i.FormMaterialNumber =  long.Parse(i.FormMaterialNumber).ToString();
                i.QualityMaterialNumber = long.Parse(i.QualityMaterialNumber).ToString();
                i.EAN =  long.Parse(i.EAN).ToString();
                if(i.EAN.Length == 12)
                {
                    i.UPC = i.EAN;
                    i.EAN = null;
                }
            }
        }
    }
}