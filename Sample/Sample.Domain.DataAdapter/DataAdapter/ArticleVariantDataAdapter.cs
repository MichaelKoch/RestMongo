using MongoBase.Repositories;
using Sample.Domain.DataAdapter.Abstractions;
using Sample.Domain.Models.Enities;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Sample.Domain.DataAdapter
{
    public class ArticleVariantDataAdapter: JsonSourceDataAdapter<ArticleVariant,ArticleVariant>
    {
        
        

        public override IList<ArticleVariant> Transform(IList<ArticleVariant> source)
        {
            long timestamp = DateTime.UtcNow.Ticks;
            
            foreach (var i in source)
            {
                var scancode = i.EAN;
                switch(scancode.ToString().Length)
                {
                    case 11:

                        i.Id = scancode.ToString();
                        i.EAN = scancode;
                        i.UPC = 0;
                        break;
                    case 13:
                        
                        i.Id = scancode.ToString();
                        i.EAN = scancode;
                        i.UPC = 0;
                        break;

                    case 12:
                        
                        i.Id = scancode.ToString();
                        i.UPC = scancode;
                        i.EAN = 0;
                        break;

                    default:
                        
                        i.Id = Guid.NewGuid().ToString();
                        i.EAN = 0;
                        i.UPC = 0;
                        break;
                }
                i.Timestamp = timestamp;
            }
            return this.Distinct(source);
        }
    }
}