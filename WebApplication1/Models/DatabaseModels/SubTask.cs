using MongoDB.Bson;
using WebApplication1.Services.Interfaces;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApplication1.Models.DatabaseModels
{
    public class SubTasksDatabaseSettings : IDatabaseSettings
    {
        public string CollectionName { get; set; }

        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }
    }

    public class SubTask : DatabaseEntityBase
    {

        [BsonElement("Description")]
        public string Description { get; set; }

        [BsonElement("ParentTaksId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ParentTaksId { get; set; }
    }
}
