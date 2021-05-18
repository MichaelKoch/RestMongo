using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using RestMongo.Data.Abstractions.Transform;

namespace RestMongo.Data.Abstractions.Repository
{
    public interface IRepository<TEntity>
        where TEntity : class, IEntity, ITransformable
    {
        IQueryable<TEntity> AsQueryable();
        // TODO: Use of PagedResultModel violates DDD this should return a list instead. PagedResultModel should be Domain's concern
        IEnumerable<TEntity> Query(string query, out int total, Dictionary<string, string> orderby = null, int maxPageSize = 1000);
        IEnumerable<TEntity> Query(string query, string orderby, out int total, int maxPageSize = 1000);

        IEnumerable<TEntity> FilterBy(
            Expression<Func<TEntity, bool>> filterExpression);

        Task<List<TEntity>> FilterByAsync(Expression<Func<TEntity, bool>> filterExpression);

        TEntity FindById(string id);

        Task<TEntity> FindByIdAsync(string id);

        TEntity FindOne(Expression<Func<TEntity, bool>> filterExpression);

        Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> filterExpression);

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