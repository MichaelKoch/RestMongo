using System.Collections.Generic;
using System.IO;
using MongoBase.Models;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Sample.Domain
{
    public static class Initializer
    {
        private const string _artifactDir = "db";
        internal static ConnectionSettings _settings;
        internal static List<string> _files = null;


        public static void Run(ConnectionSettings settings)
        {
            _settings = settings;
            _files = new List<string>(Directory.GetFiles(Path.GetDirectoryName(typeof(Initializer).Assembly.Location) + "/"+  _artifactDir));
            CreateViews();
            CreateIndizies();
            CreateFulltextIndizies();
        }
        private static List<string> ReadContent(   IEnumerable<string> files)
        {
            var retVal = new List<string>();
            foreach(var f in files)
            {
                var content =  File.ReadAllText(f);
                retVal.Add(content);
            }
            return retVal;
        }
        private static void CreateViews()
        {
            var views = ReadContent(_files.Where(c=> c.Contains(".view.json", System.StringComparison.OrdinalIgnoreCase)));
            var dbClient = new MongoClient(_settings.ConnectionString);
            var db = dbClient.GetDatabase(_settings.DatabaseName);
            foreach(string doc in views)
            {
                BsonDocument def = BsonDocument.Parse(doc);
                var name = def["create"].ToString();
                var existingView = db.GetCollection<BaseDocument>(name);
                if(existingView !=null)
                {
                   db.DropCollectionAsync(name).Wait();
                }
                db.RunCommand<BsonDocument>(doc);
            }
        }
        private static void CreateIndizies()
        {
            var views = ReadContent(_files.Where(c=> c.Contains(".index.json", System.StringComparison.OrdinalIgnoreCase)));
            var dbClient = new MongoClient(_settings.ConnectionString);
            var db = dbClient.GetDatabase(_settings.DatabaseName);
            foreach(string doc in views)
            {
                var builder = Builders<BsonDocument>.IndexKeys;
                IndexKeysDefinition<BsonDocument> keys = null;
                BsonDocument def = BsonDocument.Parse(doc);
                var fields = def["fields"].AsBsonDocument;
                foreach(BsonElement e in fields)
                {
                    if(keys == null)
                    {
                        keys = builder.Ascending(e.Name);
                    }
                    else
                    {
                        keys = keys.Ascending(e.Name);
                    }
                }
                string collectionName = def["indexOn"].ToString();
                var collection = db.GetCollection<BsonDocument>(collectionName);
                collection.Indexes.CreateOne(new CreateIndexModel<BsonDocument>(keys));
            }
        }
        private static void CreateFulltextIndizies()
        {
            var views = ReadContent(_files.Where(c=> c.Contains(".fulltext.json", System.StringComparison.OrdinalIgnoreCase)));
            var dbClient = new MongoClient(_settings.ConnectionString);
            var db = dbClient.GetDatabase(_settings.DatabaseName);
            foreach(string doc in views)
            {
                var builder = Builders<BsonDocument>.IndexKeys;
                IndexKeysDefinition<BsonDocument> keys = null;
                BsonDocument def = BsonDocument.Parse(doc);
                var fields = def["fields"].AsBsonDocument;
                foreach(BsonElement e in fields)
                {
                    if(keys == null)
                    {
                        keys = builder.Text(e.Name);
                    }
                    else
                    {
                        keys = keys.Text(e.Name);
                    }
                }
                string collectionName = def["indexOn"].ToString();
                var collection = db.GetCollection<BsonDocument>(collectionName);
                collection.Indexes.CreateOne(new CreateIndexModel<BsonDocument>(keys));
            }
        }
    }
}