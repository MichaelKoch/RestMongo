

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
using MongoBase.Models;

namespace MongoBase.Controllers
{
    [Route("[controller]")]
    public abstract class ReadController<TEntity, TDataTransfer> : ControllerBase
            where TEntity : BaseDocument
    {
        protected IRepository<TEntity> _repository;
        protected int _maxPageSize = 0; //TODO => get it from configuration

        protected virtual TDataTransfer ConvertToDTO(TEntity value)
        {
            return CopyUtils<TDataTransfer>.Convert(value);
        }
        protected virtual TEntity ConvertFromDTO(TDataTransfer value)
        {
            return CopyUtils<TEntity>.Convert(value);
        }

        protected virtual IList<TDataTransfer> Convert(IList<TEntity> value)
        {
            return CopyUtils<IList<TDataTransfer>>.Convert(value.Cast<object>().ToList());
        }



        public ReadController(IRepository<TEntity> repository, int maxPageSize = 1000)
        {
            _maxPageSize = maxPageSize;
            _repository = repository;
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
                PagedResultModel<TEntity> result = this._repository.Query(JsonSerializer.Serialize(query), orderby, this._maxPageSize);
                var retVal = new PagedResultModel<TDataTransfer>()
                {
                    Total = result.Total,
                    Values = await this.LoadRelations(Convert(result.Values), expand),
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

            var query = this._repository.AsQueryable();
            query = ODataQueryHelper.Apply<TEntity>(filter, query);
            var total = query.Count();
            query = query.Skip(skip);
            query = query.Take(top);
            var retVal = new PagedResultModel<TDataTransfer>()
            {
                Total = total,
                Values = await this.LoadRelations(Convert(query.ToList()), expand),
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
            var result = await this.LoadRelations(new List<TDataTransfer>() { ConvertToDTO(instance) }, expand);
            return result[0];

        }



        protected async virtual Task<IList<TDataTransfer>> LoadRelations(IList<TDataTransfer> values, string relations)
        {
            var expands = new List<string>();
            if (!string.IsNullOrEmpty(relations))
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