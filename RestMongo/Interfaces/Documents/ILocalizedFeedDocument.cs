using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using RestMongo.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RestMongo.Interfaces
{
    public interface ILocalizedFeedDocument : ILocalizedDocument, IFeedDocument
    {



    }
}
