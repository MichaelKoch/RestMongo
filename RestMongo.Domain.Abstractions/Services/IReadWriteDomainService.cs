using System.Threading.Tasks;

namespace RestMongo.Domain.Abstractions.Services
{
    public interface
        IReadWriteDomainService<TReadModel, in TCreateModel, in TUpdateModel> : IReadDomainService<TReadModel>
        where TReadModel : class
        where TCreateModel : class
        where TUpdateModel : class
    {
        Task<TReadModel> Create(TCreateModel value);
        Task UpdateById(string id, TUpdateModel value, bool enableConcurrency = false);
        Task DeleteById(string id);
    }
}