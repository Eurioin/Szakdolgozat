using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace WebApplication1.Models.DatabaseModels
{
    public class Account : DatabaseEntityBase
    {
        [BsonElement("Username")]
        public string Username { get; set; }

        [BsonElement("Name")]
        public string Name { get; set; }

        [BsonElement("FirstName")]
        public string FirstName{ get; set; }

        [BsonElement("LastName")]
        public string LastName { get; set; }

        [BsonElement("Email")]
        public string Email { get; set; }

        [BsonElement("PhoneNumber")]
        public string PhoneNumber { get; set; }

        [BsonElement("LastLogin")]
        [BsonRepresentation(BsonType.DateTime)]
        [BsonDateTimeOptions]
        public DateTime LastLogin { get; set; }

        [BsonElement("Url")]
        public string Url { get; set; }

        [BsonElement("AssignedProjects")]
        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> AssignedProjects { get; set; }

        [BsonElement("Roles")]
        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> Roles{ get; set; }
    }
}
