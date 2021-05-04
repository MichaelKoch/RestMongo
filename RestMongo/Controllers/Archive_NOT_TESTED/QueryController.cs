//using Microsoft.AspNetCore.Mvc;
//using RestMongo.Exceptions;
//using RestMongo.Interfaces;
//using RestMongo.Models;
//using RestMongo.Utils;
//using Swashbuckle.AspNetCore.Annotations;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Text;
//using System.Text.Json;
//using System.Threading.Tasks;

//namespace RestMongo.Controllers
//{

//    [Route("[controller]")]
//    public abstract class  QueryController<TEntity, TDataTransfer> : DocumentControllerBase<TEntity,TDataTransfer>
//            where TEntity : BaseDocument
//            where TDataTransfer : class
//    {
//        protected IRepository<TEntity> _repository;
//        protected int _maxPageSize = 0; //TODO => get it from configuration

//        public QueryController(IRepository<TEntity> repository, int maxPageSize = 1000)
//        {
//            _maxPageSize = maxPageSize;
//            _repository = repository;
//        }

//        [HttpGet("")]
//        [SwaggerResponse(200)]
//        [SwaggerResponse(412, "MAX PAGE SIZE EXCEEDED", typeof(string))]
//        public async virtual Task<ActionResult<PagedResultModel<TDataTransfer>>> Get(
//            [FromQuery(Name = "$top")]   int top = 200,
//            [FromQuery(Name = "$skip")] int skip = 0,
//            [FromQuery(Name = "$filter")] string filter = "",
//            [FromQuery(Name = "$expand")] string expand = ""
//        )
//        {
//            if (top > _maxPageSize)
//            {
//                return StatusCode(412, "MAX PAGE SIZE EXCEEDED");
//            }
//            var query = this._repository.AsQueryable();
//            query = ODataQueryHelper.Apply<TEntity>(filter, query);
//            var total = query.Count();
//            query = query.Skip(skip);
//            query = query.Take(top);
//            var entityList = query.ToList();
//            var dtoList = entityList.Transform<List<TDataTransfer>>();
//            var retVal = new PagedResultModel<TDataTransfer>()
//            {
//                Total = total,
//                Values = await this.LoadRelations(dtoList, expand),
//                Skip = skip,
//                Top = top
//            };
//            return retVal;
//        }


//        [HttpPost("queries")]
//        [SwaggerResponse(200)]
//        [SwaggerResponse(412, "MAX PAGE SIZE EXCEEDED", typeof(string))]
//        public async virtual Task<ActionResult<PagedResultModel<TDataTransfer>>> Query(
//              [FromBody] dynamic query,
//              [FromQuery(Name = "$orderby")] string orderby = "",
//              [FromQuery(Name = "$expand")]  string expand  = "")
//        {
//            try
//            {
//                PagedResultModel<TEntity> result = this._repository.Query(JsonSerializer.Serialize(query), orderby, this._maxPageSize);
//                var retVal = new PagedResultModel<TDataTransfer>()
//                {
//                    Total = result.Total,
//                    Values = await this.LoadRelations(result.Values.Transform<List<TDataTransfer>>(), expand),
//                    Skip = 0,
//                    Top = result.Total
//                };
//                return Ok(retVal);
//            }
//            catch (PageSizeExeededException ex)
//            {
//                return StatusCode((int)HttpStatusCode.PreconditionFailed, ex);
//            }
//        }



//    }
//}
