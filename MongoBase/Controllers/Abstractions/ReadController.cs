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
using MongoBase.Interfaces.Controllers;
using System.Threading.Tasks;

namespace MongoBase.Controllers
{   [Route("[controller]")]
    public abstract class ReadController<TDocument> : ControllerBase, IReadController<TDocument> where TDocument : IDocument
    {
        protected IRepository<TDocument> _repository;
        protected int _maxPageSize = 1000; //TODO => get it from configuration


        public ReadController(IRepository<TDocument> repository, int maxPageSize = 1000)
        {
            _maxPageSize = maxPageSize;
        }

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
            this.LoadRelations(retVal.Values, expand).Wait();
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
            if (top > _maxPageSize)
            {
                return StatusCode(412, "MAX PAGE SIZE EXCEEDED");
            }
            //TODO => QUICK HACK REUSING ODATA QUERY PARSER 
            var oriQueryString = Request.QueryString;
            var t = HttpUtility.ParseQueryString(Request.QueryString.ToUriComponent());
            var total = 0;
            t.Remove("$top");
            t.Remove("$skip");
            t.Remove("$expand");
            Request.QueryString = new QueryString("?" + t.ToString());
            var queryOptions = new ODataQueryOptions(IsQueryableAttribute.GetODataQueryContext(typeof(TDocument)), Request);
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
            this.LoadRelations(retVal.Values, expand).Wait();
           
            return retVal;
        }

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
            this.LoadRelations(new List<TDocument>() { instance }, expand);
            return instance;
        }



        protected async virtual Task<bool> LoadRelations(IList<TDocument> values, string relations)
        {
            var expands = relations.Replace(";", ",").Split(",").ToArray().Select(e => e.Trim()).ToList();
            return await LoadRelations(values, expands);
        }
        protected async virtual Task<bool> LoadRelations(IList<TDocument> values, IList<string> relations)
        { 
            return await Task.Run<bool>(()=> { return true; });
        }
    }
}