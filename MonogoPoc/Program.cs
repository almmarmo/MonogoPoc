using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using System;
using System.Linq;

namespace MonogoPoc
{
    class Program
    {
        static void Main(string[] args)
        {
            const string SERVER = "localhost";
            const string DATABASE = "poc";
            const string COLLECTION = "Grid";
            const bool USE_OLDFIELD = true;

            var serializer = new MongoDB.Bson.Serialization.Serializers.DateTimeSerializer(DateTimeKind.Local);
            BsonSerializer.RegisterSerializer<DateTime>(serializer);

            var dbSettings = new MongoClientSettings()
            {
                Server = new MongoServerAddress(SERVER),
                UseTls = false
            };

            MongoClient dbClient = new MongoClient(dbSettings);
            var db = dbClient.GetDatabase(DATABASE);
            var collection = db.GetCollection<BsonDocument>(COLLECTION);

            var docs = collection.Find(new BsonDocument());
            foreach(var doc in docs.ToList())
            {
                bool converted = false;
                Console.WriteLine($"Starting grid id {doc["_id"]}.");
                var slots = doc["Slots"];
                foreach(var slot in slots.AsBsonArray)
                {
                    Console.Write($"Checking Slot id {slot["_id"]} for Clauses...");
                    var clauses = slot["Clauses"];
                    if (!clauses.IsBsonNull)
                    {
                        Console.WriteLine($"{clauses.AsBsonArray.Count} Clauses founded.");
                        foreach (var clause in clauses.AsBsonArray)
                        {
                            if (clause.ConvertField<DateTime, BsonDateTime>("StartDate", USE_OLDFIELD ? "OldStartDate" : null, x => DateTime.Parse(x)))
                                converted = true;
                            if (clause.ConvertField<DateTime, BsonDateTime>("EndDate", USE_OLDFIELD ? "OldEndDate" : null, x => DateTime.Parse(x)))
                                converted = true;
                        }
                    }
                    else
                    {
                        Console.WriteLine(" No Clauses founded.");
                    }
                }

                if (converted)
                {
                    var filter = Builders<BsonDocument>.Filter.Eq(x => x["_id"], doc["_id"]);
                    collection.ReplaceOne(filter, doc);

                    Console.WriteLine($"Grid id {doc["_id"]} replaced.");
                    Console.WriteLine("----------------------");
                }
                else
                {
                    Console.WriteLine($"No Clauses changed for Grid id {doc["_id"]} .");
                    Console.WriteLine("----------------------");
                }
            }

        }
    }
}
