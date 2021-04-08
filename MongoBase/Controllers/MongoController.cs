
using System.Collections.Generic;
using MongoBase.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Linq;
using Microsoft.AspNet.OData.Query;
using System;
using System.Web;
using Microsoft.AspNetCore.Http;
using MongoBase.Attributes;
using Swashbuckle.AspNetCore.Annotations;

namespace MongoBase.Controllers
{
    public class Controller<TDocument> : ControllerBase where TDocument : IDocument
    {
        protected readonly IRepository<TDocument> _repository;

        public Controller(IRepository<TDocument> repository)
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
        public virtual PagedResultModel<TDocument> MongoQuery([FromBody] dynamic query,
            [FromQuery(Name = "$top")] int top = 1000,
            [FromQuery(Name = "$skip")] int skip = 0,
            [FromQuery(Name = "$orderby")] string orderby = "")
        {
            return this._repository.Query(JsonSerializer.Serialize(query), orderby, top, skip);
        }

        [HttpGet("")]
        [SwaggerResponse(200)]
        public virtual PagedResultModel<TDocument> Get(
            [FromQuery(Name = "$top")] int top = 1000,
            [FromQuery(Name = "$skip")] int skip = 0,
            [FromQuery(Name = "$filter")] string filter = ""
            )
        {
            if (top - skip > 10000)
            {

                throw new NotSupportedException("max page size : 10000");
            }

            var oriQueryString = Request.QueryString;
            var t = HttpUtility.ParseQueryString(Request.QueryString.ToUriComponent());
            t.Remove("$top");
            t.Remove("$skip");
            Request.QueryString = new QueryString("?" + t.ToString());
            var queryOptions = new ODataQueryOptions(SchemaAttribute.GetODataQueryContext(typeof(TDocument)), Request);
            var countQuery = this._repository.AsQueryable();
            countQuery = queryOptions.ApplyTo(this._repository.AsQueryable()).OfType<TDocument>();

            Request.QueryString = oriQueryString;
            queryOptions = new ODataQueryOptions(SchemaAttribute.GetODataQueryContext(typeof(TDocument)), Request);
            var resultQuery = queryOptions.ApplyTo(this._repository.AsQueryable()).OfType<TDocument>();
            var retVal = new PagedResultModel<TDocument>()
            {
                Total = countQuery.Count(),
                Values = resultQuery.ToList(),
                Skip = skip,
                Top = top
            };
            return retVal;
        }


        [HttpGet("delta")]
        [SwaggerResponse(200)]
        public virtual IEnumerable<TDocument> delta([FromQuery] long since = 0, [FromQuery] int skip = 0, [FromQuery] int take = 200)
        {
            var query = this._repository.AsQueryable().
                        Where(i => i.ChangedAt >= since)
                        .Take(take)
                        .Skip(skip)
                        .OrderBy(c => c.ChangedAt);
            return query;

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

        // POST api/<TestController>
        [HttpPost]
        [SwaggerResponse(200)]
        [SwaggerResponse(409, "CONFLICT")]
        public virtual ActionResult<string> Post([FromBody] TDocument value)
        {

            var instance = this._repository.FindById(value.Id);
            if (instance != null)
            {
                return Conflict("DUPLICATE KEY");
            }
            return this._repository.InsertOne(value).Id.ToString(); ;
        }

        // PUT api/<TestController>/5
        [HttpPut("{id}")]
        [SwaggerResponse(204)]
        [SwaggerResponse(409)]
        public virtual ActionResult Put(string id, [FromBody] TDocument value)
        {
            var instance = this._repository.FindById(value.Id);
            if (instance == null)
            {
                return NotFound();
            }
            if (instance.ChangedAt / 1000 != value.ChangedAt / 1000)
            {
                return Conflict("CONCURRENT UPDATE");
            }
            value.Id = id;
            this._repository.ReplaceOne(value);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [SwaggerResponse(204)]
        [SwaggerResponse(404)]
        [SwaggerOperation("Delete instance by ID")]
        public virtual ActionResult Delete(string id)
        {

            var instance = this._repository.FindById(id);
            if (instance == null)
            {
                return NotFound();
            }
            this._repository.DeleteById(id);
            return NoContent();

        }


    }
}