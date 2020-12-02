using System;
using MongoDB.Bson;
using System.Collections.Generic;
using Szakdolgozat.Services.Interfaces;
using MongoDB.Bson.Serialization.Attributes;

namespace Szakdolgozat.Models.DatabaseModels
{
    public class AccountsDatabaseSettings : IDatabaseSettings
    {
        public string CollectionName { get; set; }

        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }
    }

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
