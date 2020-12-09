using MongoDB.Bson;
using Szakdolgozat.Services.Interfaces;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Szakdolgozat.Models.DatabaseModels
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

        [BsonElement("DateOfCreation")]
        [BsonRepresentation(BsonType.DateTime)]
        [BsonDateTimeOptions]
        public DateTime DateOfCreation { get; set; }
    }
}
