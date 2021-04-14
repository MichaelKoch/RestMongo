using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Sample.Domain.DataAdapter.Repositories
{
   public class ProductSourceRepository
   {

//where a.CreatedDate >='2021-01-12 10:10:50.283'
       private string selectTemplate =@"
        select Id,Locale into #tmpLocalInfo
             from [dbo].[ImportFile]
        SELECT JSON,Locale, a.CreatedDate
        FROM [dbo].[Product] as a
        inner join #tmpLocalInfo as b
            on a.ImportFileId = b.Id
        where a.CreatedDate >= '{TIMESTAMP}'
        ORDER BY a.CreatedDate desc 
        drop table  #tmpLocalInfo
       ";
    
        public IList<ProductSourceModel> getDeltaSince(long timestamp)
        {
                return getDeltaSince(new DateTime(timestamp));
        }
        public IList<ProductSourceModel> getDeltaSince(DateTime timestamp)
        {
            var retVal = new List<ProductSourceModel>();
           
            if(timestamp == DateTime.MinValue)
            {
                timestamp = DateTime.Parse("1970-01-01 00:00:00");

            } 
            var select = this.selectTemplate.Replace("{TIMESTAMP}",timestamp.ToString("yyyy-MM-dd HH:mm:ss.FFF"));
            using (var connection = new SqlConnection("Server=hbmes700;Database=ProductService_V2;Trusted_Connection=True;"))
            using (var command = new SqlCommand(select,connection))
            {
                connection.Open();
                var reader = command.ExecuteReaderAsync().Result;
                while(reader.Read())
                {
                    var entry = new ProductSourceModel();
                    entry.CreatedDate = (DateTime)reader["CreatedDate"];
                    entry.JSON   = (string) reader["JSON"];
                    entry.Locale = (string) reader["Locale"];
                    retVal.Add(entry);
                }
                connection.Close();
            }
            return retVal;
        }
        
        
        public ProductSourceRepository()
        {            
        }
        

   }
}
