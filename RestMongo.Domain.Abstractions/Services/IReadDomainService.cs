using System.Threading.Tasks;
using RestMongo.Domain.Abstractions.Models;

namespace RestMongo.Domain.Abstractions.Services
{
    public interface IReadDomainService<TReadModel>
        where TReadModel : class
    {
        Task<IPagedResultModel<TReadModel>> Query(string query, string orderBy = "", string expand = "", int maxPageSize = 200);
        Task<TReadModel> GetById(string id, string expand = "");
    }
}