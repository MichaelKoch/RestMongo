using System.Threading.Tasks;
using RestMongo.Data.Abstractions.Extensions;
using RestMongo.Data.Abstractions.Repository;
using RestMongo.Data.Abstractions.Repository.Mongo.Documents;
using RestMongo.Domain.Abstractions.Services;
using RestMongo.Domain.Exceptions;
using RestMongo.Domain.Models;

namespace RestMongo.Domain.Services
{
    public class ReadWriteDomainService<TEntity, TReadModel, TCreateModel, TUpdateModel> :
        ReadDomainService<TEntity, TReadModel>,
        IReadWriteDomainService<TReadModel, TCreateModel, TUpdateModel>
        where TEntity : class, IFeedDocument
        where TReadModel : class
        where TCreateModel : class
        where TUpdateModel : class
    {
        private readonly IRepository<TEntity> _repository;

        public ReadWriteDomainService(IRepository<TEntity> repository) : base(repository)
        {
            _repository = repository;
        }

        public async Task<TReadModel> Create(TCreateModel value)
        {
            var feedInfo = value.Transform<KeyedDto>();
            var instance = await this._repository.FindByIdAsync(feedInfo.Id);
            if (instance != null)
            {
                throw new ConflictException($"DUPLICATE KEY {feedInfo.Id}");
            }

            var insert = value.Transform<TEntity>();
            await _repository.InsertOneAsync(insert);
            return insert.Transform<TReadModel>();
        }

        public async Task UpdateById(string id, TUpdateModel value, bool enableConcurrency = false)
        {
            var feedInfo = value.Transform<ConcurrentKeyedDto>();
            var instance = await this._repository.FindByIdAsync(id);
            if (instance == null)
            {
                throw new NotFoundException($"Could not find entity with id {id}.");
            }

            var updateInstance = value.Transform<TEntity>();
            updateInstance.Id = id;
            if (enableConcurrency)
            {
                if ((feedInfo.Timestamp == 0) || (instance.Timestamp != feedInfo.Timestamp))
                {
                    throw new ConflictException("CONCURRENCY CONFLICT");
                }
            }

            await _repository.ReplaceOneAsync(updateInstance);
        }

        public async Task DeleteById(string id)
        {
            var instance = await _repository.FindByIdAsync(id);
            if (instance == null)
            {
                throw new NotFoundException($"Could not find entity with id {id}.");
            }

            await _repository.DeleteByIdAsync(id);
        }
    }
}