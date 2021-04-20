using MongoBase.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Linq;
using Microsoft.AspNet.OData.Query;
using System.Web;
using Microsoft.AspNetCore.Http;
using MongoBase.Attributes;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System;

namespace MongoBase.Controllers
{
    public class ReadDeltaQueryFulltextController<TDocument> : ReadDeltaQueryController<TDocument>,IFulltextController<TDocument> where TDocument : IDocument
    {
        public ReadDeltaQueryFulltextController(IRepository<TDocument> repository) : base(repository)
        {

        }


        [HttpGet("search")]
        [SwaggerResponse(200)]
        [SwaggerResponse(412)]
        public virtual ActionResult<IList<TDocument>> Search(
                    [FromQuery(Name = "$term")] string searchTerm,
                    [FromQuery(Name = "$expand")] string expand = ""
                )
        {
            IList<TDocument> retVal = null;
            try
            {
                retVal = this._repository.Search(searchTerm, 30).ToList();
                retVal = this._repository.LoadRelations(retVal, expand).Result.ToList();
            }
            catch (System.NotSupportedException notSupported)
            {
                return StatusCode(412, notSupported.Message);
            }
            catch (Exception)
            {
                throw;
            }
            return Ok(retVal);
        }





    }
}