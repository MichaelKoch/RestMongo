using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MongoBase.Interfaces
{
    public interface IRepository<TDocument> where TDocument : IDocument
    {
        IQueryable<TDocument> AsQueryable();
        PagedResultModel<TDocument> Query(string query, Dictionary<string, string> orderby = null, int maxPageSize = 100);
        PagedResultModel<TDocument> Query(string query, string orderby,string expand ="", int maxPageSize = 100);
        IEnumerable<TDocument> Search(string searchTerm,int maxCount);
        IEnumerable<TDocument> FilterBy(
            Expression<Func<TDocument, bool>> filterExpression);

        
        TDocument FindById(string id);

        Task<TDocument> FindByIdAsync(string id);

        TDocument InsertOne(TDocument document);

        Task InsertOneAsync(TDocument document);

        void InsertMany(ICollection<TDocument> documents);

        Task InsertManyAsync(ICollection<TDocument> documents);

        void ReplaceOne(TDocument document);
        
        Task ReplaceOneAsync(TDocument document);

        void DeleteById(string id);
        void DeleteById(IList<string> ids);

        Task DeleteByIdAsync(string id);
      

    }
}