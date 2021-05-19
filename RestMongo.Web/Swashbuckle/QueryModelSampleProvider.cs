using System.Text.Json;
using Swashbuckle.AspNetCore.Filters;

namespace RestMongo.Web.Swashbuckle
{
    public class QueryModelSampleProvider: IExamplesProvider<JsonDocument> 
    {
        public JsonDocument GetExamples()
        {
            return JsonDocument.Parse(
                @"{'sdfsd','sdfsd'}");
        }
    }
}