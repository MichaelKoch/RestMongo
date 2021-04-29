

using RestMongo.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Linq;
using Microsoft.AspNet.OData.Query;
using System.Web;
using Microsoft.AspNetCore.Http;
using RestMongo.Attributes;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using RestMongo.Exceptions;
using RestMongo.Utils;
using RestMongo.Models;
using System;

namespace RestMongo.Controllers
{
    [Route("[controller]")]
    public abstract class ReadController<TEntity, TDataTransfer> : ControllerBase
            where TEntity : BaseDocument
            where TDataTransfer : class 
      {
        protected IRepository<TEntity> _repository;
        protected int _maxPageSize = 0; //TODO => get it from configuration

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
                    Values = await this.LoadRelations(result.Values.Transform<List<TDataTransfer>>(), expand),
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
            var entityList = query.ToList();
            var dtoList = entityList.Transform<List<TDataTransfer>>();
            var retVal = new PagedResultModel<TDataTransfer>()
            {
                Total = total,
                Values = await this.LoadRelations(dtoList, expand),
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
            TEntity instance = this._repository.FindById(id);
            if (instance == null)
            {
                return NotFound();
            }

            TDataTransfer dto = instance.Transform<TDataTransfer>();
            var result = await this.LoadRelations(dto as TDataTransfer, expand);
            return result;
        }

        protected async virtual Task<TDataTransfer> LoadRelations(TDataTransfer value, string relations)
        {
            var expands = new List<string>();
            if (!string.IsNullOrEmpty(relations))
            {
                expands = relations.Replace(";", ",").Split(",").ToArray().Select(e => e.Trim()).ToList();
            }
            return await LoadRelations(value, expands);
        }
        protected async virtual Task<TDataTransfer> LoadRelations(TDataTransfer value, IList<string> relations)
        {
            return await Task.Run<TDataTransfer>(() => { return value; });
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