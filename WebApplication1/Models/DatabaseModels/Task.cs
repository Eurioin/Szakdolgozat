using System;
using MongoDB.Bson;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations.Schema;
using Szakdolgozat.Services.Interfaces;

namespace Szakdolgozat.Models.DatabaseModels
{
    public class TasksDatabaseSettings : IDatabaseSettings
    {
        public string CollectionName { get; set; }

        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }
    }

    public class Task : DatabaseEntityBase
    {
        [BsonElement("Name")]
        public string Name { get; set; }

        [BsonElement("Project")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Project { get; set; }

        [BsonElement("Priority")]
        public string Priority { get; set; }

        [BsonElement("Type")]
        public string Type { get; set; }

        [BsonElement("Status")]
        public string Status { get; set; }

        [BsonElement("HandledBy")]
        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> HandledBy { get; set; }

        [BsonElement("Description")]
        public string Description { get; set; }

        [BsonElement("NumberOfSubTasks")]
        public int NumberOfSubTasks { get; set; }

        [BsonElement("DateOfCreation")]
        [BsonRepresentation(BsonType.DateTime)]
        [BsonDateTimeOptions]
        public DateTime DateOfCreation { get; set; }

        [BsonElement("EndDate")]
        [BsonRepresentation(BsonType.DateTime)]
        [BsonDateTimeOptions]
        public DateTime EndDate { get; set; }

        [BsonElement("SubTasks")]
        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> SubTasksIds { get; set; }

        [NotMapped]
        public List<SubTask> ServerSideTaskList { get; set; }
    }
}
