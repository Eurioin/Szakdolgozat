using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Szakdolgozat.Services.Interfaces;

namespace Szakdolgozat.Models.DatabaseModels
{
    public class CommentsDatabaseSettings : IDatabaseSettings
    {
        public string CollectionName { get; set; }

        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }
    }

    public class Comment : DatabaseEntityBase
    {
        [BsonElement("Author")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string AuthorId { get; set; }

        [BsonElement("DateOfCreation")]
        public DateTime DateOfCreation { get; set; }

        [BsonElement("Content")]
        public string Content { get; set; }

        [BsonElement("Task")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Task { get; set; }
    }
}
