using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestMongo.Controllers
{
    public abstract class DocumentControllerBase<TEntity, TDataTransfer> : ControllerBase
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
            var expands = new List<string>();
            if (!string.IsNullOrEmpty(relations))
            {
                expands = relations.Replace(";", ",").Split(",").ToArray().Select(e => e.Trim()).ToList();
            }
            return await LoadRelations(value, expands);
        }

        protected internal async virtual Task<IList<TDataTransfer>> LoadRelations(IList<TDataTransfer> values, IList<string> relations)
        {
            return await Task.Run<IList<TDataTransfer>>(() => { return values; });
        }


        protected internal async virtual Task<TDataTransfer> LoadRelations(TDataTransfer value, IList<string> relations)
        {
            return await Task.Run<TDataTransfer>(() => { return value; });
        }

    }
}
