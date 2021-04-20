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
    public class ReadDeltaQueryController<TDocument> : ReadDeltaController<TDocument>,IQueryController<TDocument> where TDocument : IDocument
    {
        public ReadDeltaQueryController(IRepository<TDocument> repository):base(repository)
        {
            
        }





        /// <summary>
        ///     
        /// </summary>
        /// <param name="query">a mongo query for syntax see : https://docs.mongodb.com/manual/tutorial/query-documents/</param>
        /// <returns>the query result</returns>
        [HttpPost("query")]
        [SwaggerResponse(200)]
        [SwaggerResponse(412)]
        public virtual ActionResult<PagedResultModel<TDocument>> Query(
            [FromBody] dynamic query,
            [FromQuery(Name = "$orderby")] string orderby = "",
            [FromQuery(Name = "$expand")] string expand = "")
        {
           //TODO : Clean response code 
            var retVal = this._repository.Query(JsonSerializer.Serialize(query), orderby);
            retVal.Values = _repository.LoadRelations(retVal.Values, expand).Result;
            return retVal;
        }

    }
}