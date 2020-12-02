using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Szakdolgozat.Models.DatabaseModels
{
    public abstract class DatabaseEntityBase
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}
