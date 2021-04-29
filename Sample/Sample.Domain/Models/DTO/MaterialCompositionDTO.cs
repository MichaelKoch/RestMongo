
using System.Text.Json.Serialization;
using RestMongo.Attributes;
using RestMongo.Interfaces;
using Sample.Domain.Models.Enities;


public class MaterialCompositionDTO
{


    [JsonPropertyName("MaterialCompositionID")]
    public string MaterialCompositionID { get; set; }

    [JsonPropertyName("Component")]
    public string Component { get; set; }


    [JsonPropertyName("ComponentText")]
    public string ComponentText { get; set; }

    [JsonPropertyName("Proportion")]
    public int Proportion { get; set; }

    [JsonPropertyName("Text")]
    public string Text { get; set; }

    [JsonPropertyName("Abbreviation")]
    public string Abbreviation { get; set; }


}

