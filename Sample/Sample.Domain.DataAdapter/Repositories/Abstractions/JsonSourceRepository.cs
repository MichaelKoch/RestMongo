using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Sample.Domain.Models;

namespace Sample.Domain.DataAdapter.Repositories.Abstractions
{

   public class JsonSourceRepository<TDocument>
   {
        protected readonly string _dataFileName = $"data/{typeof(TDocument).Name}.sample.json";
        protected readonly IList<TDocument> _data = null;
        public JsonSourceRepository()
        {
            if(!File.Exists(_dataFileName))
            {
                throw new FileNotFoundException("data sample file not found");
            }
            _data = JsonSerializer.Deserialize<List<TDocument>>(File.ReadAllText(_dataFileName));
            this.Transform();
            MarkRandomUpdates(100);
        }
        protected virtual void MarkRandomUpdates(int simulatedUpdateCount)
        {
            
        }
        protected virtual void Transform()
        {
            
        }
        public IQueryable<TDocument> AsQueryable()
        {
            return _data.AsQueryable<TDocument>();
        }

   }
}
