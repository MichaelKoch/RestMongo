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
using System;
using MongoDB.Bson;
using System.Dynamic;
using MongoBase.Models;

namespace MongoBase.Controllers
{
    [Route("[controller]")]
    public abstract class LocalizedReadController<TEntity, TDataTransfer> : ControllerBase
            where TEntity : LocalizedDocument
    {


        public LocalizedReadController(IRepository<TEntity> repo, int maxPageSize = 1000)
        {
            this._repository = repo;
            this._maxPageSize = maxPageSize;
        }
        protected IRepository<TEntity> _repository;
        protected int _maxPageSize = 0;
        protected Type _entityType = typeof(TEntity);

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

        [HttpPost("query")]
        [SwaggerResponse(200)]
        [SwaggerResponse(412, "MAX PAGE SIZE EXCEEDED", typeof(string))]
        public async virtual Task<ActionResult<PagedResultModel<TDataTransfer>>> Query(
               [FromBody] ExpandoObject query,
               [FromQuery(Name = "locale")] string locale = "en-GB",
               [FromQuery(Name = "$orderby")] string orderby = "",
               [FromQuery(Name = "$expand")] string expand = "")
        {

            try
            {

                if (IsQueryableAttribute.IsAssignedTo(_entityType.GetProperty("Locale")))
                {
                    query.TryAdd("Locale", locale);
                }
                PagedResultModel<TEntity> result = this._repository.Query(JsonSerializer.Serialize(query), orderby);
                var retVal = new PagedResultModel<TDataTransfer>()
                {
                    Total = result.Total,
                    Values = await this.LoadRelations(Convert(result.Values), expand, locale),
                    Skip = 0,
                    Top = result.Total
                };
                return Ok(retVal);
            }
            catch (PageSizeExeededException ex)
            {
                return StatusCode((int)HttpStatusCode.PreconditionFailed, ex.Message);
            }
        }



        [HttpGet("")]
        [SwaggerResponse(200)]
        [SwaggerResponse(412, "MAX PAGE SIZE EXCEEDED", typeof(string))]
        public async virtual Task<ActionResult<PagedResultModel<TDataTransfer>>> Get(
            [FromQuery(Name = "locale")] string locale = "en-GB",
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
            var query = this._repository.AsQueryable();
            query = ODataQueryHelper.Apply<TEntity>(filter, query).OfType<TEntity>();


            if (IsQueryableAttribute.IsAssignedTo(_entityType.GetProperty("Locale")))
            {
                query = query.Where(c => c.Locale == locale);
            }
            var total = query.Count();
            query = query.Take(top).Skip(skip);

            var retVal = new PagedResultModel<TDataTransfer>()
            {
                Total = total,
                Values = await this.LoadRelations(Convert(query.ToList()), expand, locale),
                Skip = skip,
                Top = top
            };
            return retVal;
        }

        [HttpGet("{id}")]
        [SwaggerResponse(200)]
        [SwaggerResponse(404, "NOT FOUND", typeof(string))]

        public virtual async Task<ActionResult<TDataTransfer>> Get(
            string id,
            [FromQuery(Name = "locale")] string locale = "en-GB",
            [FromQuery(Name = "$expand")] string expand = ""
            )
        {

            var query = this._repository.AsQueryable();
            if (_entityType.IsAssignableTo(typeof(ILocalizedDocument)))
            {
                query = query.Where(c => c.Locale == locale);
            }
            var instance = query.Where(c => c.Id == id).FirstOrDefault();
            if (instance == null)
            {
                return NotFound();
            }
            var prepared = await this.LoadRelations(new List<TDataTransfer>() { ConvertToDTO(instance) }, expand, locale);
            return prepared[0];
        }



        protected async virtual Task<IList<TDataTransfer>> LoadRelations(IList<TDataTransfer> values, string relations, string locale)
        {
            var expands = new List<string>();
            if (!string.IsNullOrEmpty(relations))
            {
                expands = relations.Replace(";", ",").Split(",").ToArray().Select(e => e.Trim()).ToList();
            }
            return await LoadRelations(values, expands, locale);
        }
        protected async virtual Task<IList<TDataTransfer>> LoadRelations(IList<TDataTransfer> values, IList<string> relations, string locale)
        {
            return await Task.Run<IList<TDataTransfer>>(() => { return values; });
        }
    }
}
