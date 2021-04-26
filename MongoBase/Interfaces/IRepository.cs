using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MongoBase.Interfaces
{
    public interface IRepository<TEntity> where TEntity : IDocument
    {
        IQueryable<TEntity> AsQueryable();
        PagedResultModel<TEntity> Query(string query, Dictionary<string, string> orderby = null, int maxPageSize = 100);
        PagedResultModel<TEntity> Query(string query, string orderby,string expand ="", int maxPageSize = 100);
        IEnumerable<TEntity> Search(string searchTerm,int maxCount);
        IEnumerable<TEntity> FilterBy(
            Expression<Func<TEntity, bool>> filterExpression);

        
        TEntity FindById(string id);

        Task<TEntity> FindByIdAsync(string id);

        TEntity InsertOne(TEntity document);

        Task InsertOneAsync(TEntity document);

        void InsertMany(ICollection<TEntity> documents);

        Task InsertManyAsync(ICollection<TEntity> documents);

        void ReplaceOne(TEntity document);
        
        Task ReplaceOneAsync(TEntity document);

        void DeleteById(string id);
        void DeleteById(IList<string> ids);

        Task DeleteByIdAsync(string id);
      

    }
}