using System;
using MongoDB.Bson;
using System.Threading.Tasks;
using System.Collections.Generic;
using Szakdolgozat.Services.Interfaces;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace Szakdolgozat.Models.DatabaseModels
{
    public class ProjectsDatabaseSettings : IDatabaseSettings
    {
        public string CollectionName { get; set; }

        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }
    }

    public class Project : DatabaseEntityBase
    {
        [BsonElement("Name")]
        public string Name { get; set; }

        [BsonElement("DateOfCreation")]
        [BsonDateTimeOptions]
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime DateOfCreation { get; set; }

        [BsonElement("NumberOfAssignees")]
        public int NumberOfAssignees { get; set; }

        [BsonElement("Assignees")]
        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> Assignees { get; set; }

        [BsonElement("NumberOfTasks")]
        public int NumberOfTasks { get; set; }

        [BsonElement("Tasks")]
        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> Tasks { get; set; }

        [BsonElement("Company")]
        public string Company { get; set; }

        [NotMapped]
        public List<ApplicationUser> ServerSideUsersList { get; set; }

        [NotMapped]
        public List<Task> ServerSideTasksList { get; set; }
    }
}
