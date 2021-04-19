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

namespace MongoBase.Controllers
{
    public class ReadController<TDocument> : ControllerBase where TDocument : IDocument
    {
        protected readonly IRepository<TDocument> _repository;
        protected readonly int maxPageSize = 1000; //TODO => get it from configuration
        public ReadController(IRepository<TDocument> repository)
        {
            this._repository = repository;
        }
         


        

        /// <summary>
        ///     
        /// </summary>
        /// <param name="query">a mongo query for syntax see : https://docs.mongodb.com/manual/tutorial/query-documents/</param>
        /// <returns>the query result</returns>
        [HttpPost("mongoquery")]
        [SwaggerResponse(200)]
        [SwaggerResponse(412)]
        public virtual ActionResult<PagedResultModel<TDocument>> MongoQuery([FromBody] dynamic query,
            [FromQuery(Name = "$orderby")] string orderby = "",
            [FromQuery(Name = "$expand")] string expand = "")
        {
            //if (top - skip > this.maxPageSize)
            //{
            //    StatusCode(412,"MAX PAGE SIZE EXCEEDED");
            //}
            var retVal= this._repository.Query(JsonSerializer.Serialize(query), orderby);
            var expands = expand.Replace(";", ",").Split(",").ToArray().Select(e => e.Trim()).ToArray();
            retVal.Values = _repository.LoadRelations(retVal.Values, expands).Result;
            return retVal;
        }

        [HttpGet("")]
        [SwaggerResponse(200)]
        [SwaggerResponse(412)]
        public virtual ActionResult<PagedResultModel<TDocument>> Get(
            [FromQuery(Name = "$top")] int top = 200,
            [FromQuery(Name = "$skip")] int skip = 0,
            [FromQuery(Name = "$filter")] string filter = "",
            [FromQuery(Name = "$expand")] string expand = ""
        )
        {
            if (top > 1000)
            {
                return StatusCode(412,"MAX PAGE SIZE EXCEEDED");
            }
            //TODO => QUICK HACK REUSING ODATA QUERY PARSER 
            var oriQueryString = Request.QueryString;
            var t = HttpUtility.ParseQueryString(Request.QueryString.ToUriComponent());
            var total = 0;
            t.Remove("$top");
            t.Remove("$skip");
            t.Remove("$expand");
            Request.QueryString = new QueryString("?" + t.ToString());
            var queryOptions = new ODataQueryOptions(SchemaAttribute.GetODataQueryContext(typeof(TDocument)), Request);
            var query = this._repository.AsQueryable();
            query = queryOptions.ApplyTo(this._repository.AsQueryable()).OfType<TDocument>();
            total = query.Count();
            query = query.Skip(skip);
            query = query.Take(top);
            var retVal = new PagedResultModel<TDocument>()
            {
                Total = total,
                Values = query.ToList(),
                Skip = skip,
                Top = top
            };
            var expands = expand.Replace(";", ",").Split(",").ToArray().Select(e => e.Trim()).ToArray();
            retVal.Values = _repository.LoadRelations(retVal.Values, expands).Result;
            return retVal;
        }



        // GET api/<TestController>/5
        [HttpGet("{id}")]
        [SwaggerResponse(200)]
        [SwaggerResponse(404, "NOT FOUND")]
        public virtual ActionResult<TDocument> Get(string id, [FromQuery(Name = "$expand")] string expand = "")
        {
            var instance = this._repository.FindById(id);
            if (instance == null)
            {
                return NotFound();
            }
            var expands = expand.Replace(";", ",").Split(",").ToArray().Select(e => e.Trim()).ToArray();
            instance  = _repository.LoadRelations(new List<TDocument>() { instance }, expands).Result[0];
            return instance;
        }
    }
}