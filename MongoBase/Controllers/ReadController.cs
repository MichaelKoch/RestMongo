

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
using System.Threading.Tasks;
using System.Net;
using MongoBase.Exceptions;
using MongoBase.Utils;

namespace MongoBase.Controllers
{
    [Route("[controller]")]
    public abstract class ReadController<TEntity, TDataTransfer> : ControllerBase where TEntity : IDocument
    {
        protected IRepository<TEntity> _repository;
        protected int _maxPageSize = 0; //TODO => get it from configuration

        protected virtual TDataTransfer ConvertToDTO(TEntity value)
        {
            return CopyUtils<TDataTransfer>.Convert(value);
        }

        protected virtual IList<TDataTransfer> Convert(IList<TEntity> value)
        {
            return CopyUtils<IList<TDataTransfer>>.Convert(value.Cast<object>().ToList());
        }



        public ReadController(IRepository<TEntity> repository, int maxPageSize = 1000)
        {
            _maxPageSize = maxPageSize;
        }

        [HttpPost("query")]
        [SwaggerResponse(200)]
        [SwaggerResponse(412, "MAX PAGE SIZE EXCEEDED", typeof(string))]
        public async virtual Task<ActionResult<PagedResultModel<TDataTransfer>>> Query(
               [FromBody] dynamic query,
               [FromQuery(Name = "$orderby")] string orderby = "",
               [FromQuery(Name = "$expand")] string expand = "")
        {
            //TODO : Clean response code 
            try
            {
                PagedResultModel<TEntity> result = this._repository.Query(JsonSerializer.Serialize(query), orderby);
                var retVal = new PagedResultModel<TDataTransfer>()
                {
                    Total = result.Total,
                    Values = await this.LoadRelations(Convert(query.ToList()), expand),
                    Skip = 0,
                    Top = result.Total
                };
                return Ok(retVal);
            }
            catch (PageSizeExeededException ex)
            {
                return StatusCode((int)HttpStatusCode.PreconditionFailed, ex);
            }
        }



        [HttpGet("")]
        [SwaggerResponse(200)]
        [SwaggerResponse(412, "MAX PAGE SIZE EXCEEDED", typeof(string))]
        public async virtual Task<ActionResult<PagedResultModel<TDataTransfer>>> Get(
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
            var queryOptions = new ODataQueryOptions(IsQueryableAttribute.GetODataQueryContext(typeof(TEntity)), Request);
            var query = this._repository.AsQueryable();
            query = queryOptions.ApplyTo(this._repository.AsQueryable()).OfType<TEntity>();
            total = query.Count();
            query = query.Skip(skip);
            query = query.Take(top);
            var retVal = new PagedResultModel<TDataTransfer>()
            {
                Total = total,
                Values =await  this.LoadRelations(Convert(query.ToList()), expand),
                Skip = skip,
                Top = top
            };
            return retVal;
        }

        [HttpGet("{id}")]
        [SwaggerResponse(200)]
        [SwaggerResponse(404, "NOT FOUND", typeof(string))]
        public async virtual Task<ActionResult<TDataTransfer>> Get(string id, [FromQuery(Name = "$expand")] string expand = "")
        {
            var instance = this._repository.FindById(id);
            if (instance == null)
            {
                return NotFound();
            }
            return this.LoadRelations(new List<TDataTransfer>() { ConvertToDTO(instance) }, expand).Result[0];

        }



        protected async virtual Task<IList<TDataTransfer>> LoadRelations(IList<TDataTransfer> values, string relations)
        {
            var expands = new List<string>();
            if(!string.IsNullOrEmpty(relations))
            {
                expands = relations.Replace(";", ",").Split(",").ToArray().Select(e => e.Trim()).ToList();
            }
            return await LoadRelations(values, expands);
        }
        protected async virtual Task<IList<TDataTransfer>> LoadRelations(IList<TDataTransfer> values, IList<string> relations)
        {
            return await Task.Run<IList<TDataTransfer>>(() => { return values; });
        }
    }
}