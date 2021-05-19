using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using RestMongo.Data.Abstractions.Extensions;
using RestMongo.Data.Abstractions.Repository;
using RestMongo.Data.Abstractions.Repository.Mongo.Documents;
using RestMongo.Domain.Abstractions.Models;
using RestMongo.Domain.Abstractions.Services;
using RestMongo.Domain.Exceptions;
using RestMongo.Domain.Models;

namespace RestMongo.Domain.Services
{
    public class ReadDomainService<TEntity, TReadModel> : IReadDomainService<TReadModel>
        where TEntity : class, IDocument
        where TReadModel : class
    {
        private readonly IRepository<TEntity> _repository;

        public ReadDomainService(IRepository<TEntity> repository)
        {
            _repository = repository;
        }

        public virtual async Task<IList<TReadModel>> LoadRelations(IList<TReadModel> values, string relations)
        {
            var expands = new List<string>();
            if (!string.IsNullOrEmpty(relations))
            {
                expands = relations.Replace(";", ",").Split(",").ToArray().Select(e => e.Trim()).ToList();
            }

            return await LoadRelations(values, expands);
        }

        public virtual async Task<TReadModel> LoadRelations(TReadModel value, string relations)
        {
            var items = new List<TReadModel> {value};
            var result = await LoadRelations(items, relations);
            return result[0];
        }

        public virtual async Task<IList<TReadModel>> LoadRelations(IList<TReadModel> values, IList<string> relations)
        {
            return await Task.Run(() => values);
        }


        public async Task<IPagedResultModel<TReadModel>> Query(string query, string orderBy = "", string expand = "",
            int maxPageSize = 200)
        {
            List<TEntity> result;
            int total;
            try
            {
                result = _repository.Query(query, orderBy, out total, maxPageSize)
                    .ToList();
            }
            catch (AggregateException ae)
            {
                if (ae.InnerException?.GetType() == typeof(ArgumentOutOfRangeException))
                {
                    throw new PageSizeExeededException("", ae);
                }

                throw;
            }
            catch (ArgumentOutOfRangeException argumentOutOfRangeException)
            {
                throw new PageSizeExeededException("", argumentOutOfRangeException);
            }

            var retVal = new PagedResultModel<TReadModel>
            {
                Total = total,
                Values = await LoadRelations(result.Transform<List<TReadModel>>(), expand),
                Skip = 0,
                Top = total
            };
            return retVal;
        }

        public async Task<TReadModel> GetById(string id, string expand = "")
        {
            TEntity instance = _repository.FindById(id);
            if (instance == null)
            {
                throw new NotFoundException($"Could not find {id}.");
            }

            TReadModel dto = instance.Transform<TReadModel>();
            var result = await LoadRelations(dto, expand);
            return result;
        }
    }
}