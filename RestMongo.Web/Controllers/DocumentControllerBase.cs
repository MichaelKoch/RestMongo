using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace RestMongo.Web.Controllers
{
    public abstract class DocumentControllerBase<TDataTransfer> : ControllerBase
    {

        protected internal async Task<IList<TDataTransfer>> LoadRelations(IList<TDataTransfer> values, string relations)
        {
            var expands = new List<string>();
            if (!string.IsNullOrEmpty(relations))
            {
                expands = relations.Replace(";", ",").Split(",").ToArray().Select(e => e.Trim()).ToList();
            }
            return await LoadRelations(values, expands);
        }
       
        protected internal async Task<TDataTransfer> LoadRelations(TDataTransfer value, string relations)
        {
            var items = new List<TDataTransfer> { value };
            var result = await LoadRelations(items,relations);
            return result[0];

        }
        protected internal virtual async Task<IList<TDataTransfer>> LoadRelations(IList<TDataTransfer> values, IList<string> relations)
        {
            return await Task.Run(() => values);
        }
       
    }
}
