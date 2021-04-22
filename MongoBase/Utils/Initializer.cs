using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using MongoBase.Interfaces;
using MongoBase.Models;
using MongoBase.Repositories;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoBase.Utils
{
    public static class SchemaInitializer
    {
        private static string _sourceDir = "";
        internal static IConnectionSettings _settings;
        internal static Dictionary<string, string> _files = null;
        private static bool isInitialized = false;

        public static void Run(IConnectionSettings settings, Assembly assembly, string sourceDir = null)
        {
            if (isInitialized) return;
            var assemblyName = assembly.GetName().Name;
            var assemblyVersion = assembly.GetName().Version.ToString();
            var hash = CalculateHash(_files);
            _settings = settings;
            _sourceDir = sourceDir;
            if (string.IsNullOrEmpty(_sourceDir))
            {
                _sourceDir = $"{Path.GetDirectoryName(assembly.Location)}/db";
            }
            _files = ReadContent(_sourceDir);
            MongoRepository<DomainSchemaInfo> schemaRepo = new Repositories.MongoRepository<DomainSchemaInfo>(_settings);
            var schemainfo = schemaRepo.AsQueryable().SingleOrDefault(c => c.AssemblyName == assemblyName);
            
            if(schemainfo ==null || schemainfo.Hash != CalculateHash(_files))
            {
                CreateViews();
                CreateIndizies();
                var newSchemaInfo = new DomainSchemaInfo()
                {
                    AssemblyName = assemblyName,
                    AssemblyVersion = assemblyVersion,
                    AppliedSchema = JsonSerializer.Serialize(_files),
                    Hash = hash,
                };
                if(schemainfo ==null)
                {
                    schemaRepo.InsertOne(newSchemaInfo);
                }
                else
                {
                    newSchemaInfo.Id = schemainfo.Id;
                    schemaRepo.ReplaceOne(newSchemaInfo);
                }
            }
            isInitialized = true;
        }
        private static Dictionary<string, string> ReadContent(string sourceDir)
        {
            var files = Directory.GetFiles(sourceDir);
            var retVal = new Dictionary<string, string>();
            foreach (var f in files)
            {
                var content = File.ReadAllText(f);
                retVal.Add(f.ToLower(), content);
            }
            return retVal;
        }
        private static string CalculateHash(object instance)
        {
            string json = JsonSerializer.Serialize(instance);
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(json);
            byte[] hashBytes = md5.ComputeHash(inputBytes);
            var sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }
            return sb.ToString();
        }
        private static void CreateViews()
        {
            var views = _files.Keys.Where(k => k.Contains(".view.json")).Select(k => _files[k]);
            if (!views.Any()) { return; }
            var dbClient = new MongoClient(_settings.ConnectionString);
            var db = dbClient.GetDatabase(_settings.DatabaseName);
            foreach (string doc in views)
            {
                BsonDocument def = BsonDocument.Parse(doc);
                var name = def["create"].ToString();
                var existingView = db.GetCollection<BaseDocument>(name);
                if (existingView != null)
                {
                    db.DropCollectionAsync(name).Wait();
                }
                db.RunCommand<BsonDocument>(doc);
            }
        }
        private static void CreateIndizies()
        {
            var views = _files.Keys.Where(k => k.Contains(".index.json")).Select(k => _files[k]);
            if (!views.Any()) { return; }
            var dbClient = new MongoClient(_settings.ConnectionString);
            var db = dbClient.GetDatabase(_settings.DatabaseName);
            foreach (string doc in views)
            {
                var builder = Builders<BsonDocument>.IndexKeys;
                IndexKeysDefinition<BsonDocument> keys = null;
                BsonDocument def = BsonDocument.Parse(doc);
                var fields = def["fields"].AsBsonDocument;


                //TODO -> Simplify / harmonize if keys not initialized 
                foreach (BsonElement e in fields)
                {
                    if (keys == null)
                    {
                        switch (e.Value.ToString().ToLower())
                        {
                            case "text":
                                keys = builder.Text(e.Name);
                                break;
                            case "asc":
                                keys = builder.Ascending(e.Name);
                                break;
                            case "desc":
                                keys = builder.Descending(e.Name);
                                break;
                        }
                    }
                    else
                    {
                        switch (e.Value.ToString().ToLower())
                        {
                            case "text":
                                keys = keys.Text(e.Name);
                                break;
                            case "asc":
                                keys = keys.Ascending(e.Name);
                                break;
                            case "desc":
                                keys = keys.Descending(e.Name);
                                break;
                        }
                    }
                }
                if(keys != null)
                {
                    string collectionName = def["indexOn"].ToString();
                    var collection = db.GetCollection<BsonDocument>(collectionName);
                    collection.Indexes.CreateOne(new CreateIndexModel<BsonDocument>(keys));
                }
                else
                {
                    throw new InvalidDataException("Invalid index definition :" + def["indexOn"]);
                }
            }
        }
    }
}