using RestMongo.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text.Json;

public class QueryModelSampleProvider: IExamplesProvider<JsonDocument> 
{
    public JsonDocument GetExamples()
    {
        return JsonDocument.Parse(
            @"{'sdfsd','sdfsd'}");
    }
}