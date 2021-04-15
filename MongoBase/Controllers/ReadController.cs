using MongoBase.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Linq;
using Microsoft.AspNet.OData.Query;
using System.Web;
using Microsoft.AspNetCore.Http;
using MongoBase.Attributes;
using Swashbuckle.AspNetCore.Annotations;

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
        [SwaggerResponse(409)]
        public virtual ActionResult<PagedResultModel<TDocument>> MongoQuery([FromBody] dynamic query,
            [FromQuery(Name = "$top")] int top = 200,
            [FromQuery(Name = "$skip")] int skip = 0,
            [FromQuery(Name = "$orderby")] string orderby = "")
        {
            if (top - skip > this.maxPageSize)
            {
                Conflict("MAX PAGE SIZE EXCEEDED");
            }
            return this._repository.Query(JsonSerializer.Serialize(query), orderby, top, skip);
        }

        [HttpGet("")]
        [SwaggerResponse(200)]
        [SwaggerResponse(409)]
        public virtual ActionResult<PagedResultModel<TDocument>> Get(
            [FromQuery(Name = "$top")] int top = 200,
            [FromQuery(Name = "$skip")] int skip = 0,
            [FromQuery(Name = "$filter")] string filter = ""
            )
        {
            if (top - skip > 1000)
            {
                return Conflict("MAX PAGE SIZE EXCEEDED");
            }

            var oriQueryString = Request.QueryString;
            var t = HttpUtility.ParseQueryString(Request.QueryString.ToUriComponent());
            var count = 0;
            t.Remove("$top");
            t.Remove("$skip");
            Request.QueryString = new QueryString("?" + t.ToString());
            var queryOptions = new ODataQueryOptions(SchemaAttribute.GetODataQueryContext(typeof(TDocument)), Request);
            var countQuery = this._repository.AsQueryable();
            countQuery = queryOptions.ApplyTo(this._repository.AsQueryable()).OfType<TDocument>();
            Request.QueryString = oriQueryString;
            queryOptions = new ODataQueryOptions(SchemaAttribute.GetODataQueryContext(typeof(TDocument)), Request);
            var resultQuery = queryOptions.ApplyTo(this._repository.AsQueryable()).OfType<TDocument>();
            resultQuery = resultQuery.Skip(skip);
            resultQuery = resultQuery.Take(top);
            count = resultQuery.Count();
            if (count > 1000)
            {
                return Conflict("MAX PAGE SIZE EXCEEDED");
            }
            var retVal = new PagedResultModel<TDocument>()
            {
                Total = countQuery.Count(),
                Values = resultQuery.ToList(),
                Skip = skip,
                Top = top
            };
            return retVal;
        }



        // GET api/<TestController>/5
        [HttpGet("{id}")]
        [SwaggerResponse(200)]
        [SwaggerResponse(404, "NOT FOUND")]
        public virtual ActionResult<TDocument> Get(string id)
        {
            var instance = this._repository.FindById(id);
            if (instance == null)
            {
                return NotFound();
            }
            return instance;
        }
    }
}