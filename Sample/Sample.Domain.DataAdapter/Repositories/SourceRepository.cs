using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Sample.Domain.DataAdapter.Repositories
{
   public class SourceRepository
   {
        private IList<Product> _data = null;
        public SourceRepository()
        {
            if(!File.Exists("data/sample.json"))
            {
                throw new FileNotFoundException("data sample file not found");
            }
            _data = JsonSerializer.Deserialize<List<Product>>(File.ReadAllText("data/sample.json"));
            markRandomUpdates(100);
        }
        private void markRandomUpdates(int simulatedUpdateCount)
        {
            Random r = new Random();
            for(int i=0;i<=simulatedUpdateCount;i++)
            {
                int randomIndex = r.Next(0,_data.Count);
                _data[randomIndex].ChangedAt = DateTime.UtcNow.Ticks;
            }
        }
        public IQueryable<Product> AsQueryable()
        {
            return _data.AsQueryable<Product>();
        }

   }
}
