using MongoBase.Repositories;
using Sample.Domain.DataAdapter.Abstractions;
using Sample.Domain.Models;
using System.Collections.Generic;
using System.Linq;
namespace Sample.Domain.DataAdapter
{
    public class ArticleVariantDataAdapter:JsonSourceRepository<ArticleVariant,ArticleVariant>
    {
        
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