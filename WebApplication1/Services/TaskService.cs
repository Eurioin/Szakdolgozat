using System;
using System.Linq;
using MongoDB.Driver;
using System.Collections.Generic;
using Szakdolgozat.Services.Interfaces;
using Szakdolgozat.Models.DatabaseModels;

namespace Szakdolgozat.Services
{
    public class TaskService : AbstractServiceBase<Task>
    {
        public TaskService(TasksDatabaseSettings settings) : base(settings) { }

        public override List<Task> GetByProperty(string propertyname, string value)
        {
            switch (propertyname.ToLower())
            {
                case "name":
                    return this._collection.Find(p => p.Name.Equals(value)).ToList();
                case "dateofcreation":
                    var date = DateTime.Parse(value);
                    return this._collection.Find(p => p.DateOfCreation.Equals(date)).ToList();
                case "description":
                    return this._collection.Find(p => p.Description.Equals(value)).ToList();
                default:
                    return new List<Task>() { this.GetById(value) };
            }
        }

        public override Task GetById(string id)
        {
            return this._collection.Find(p => p.Id.Equals(id)).FirstOrDefault();
        }

        public override void Update(string id, Task item)
        {
            this._collection.ReplaceOne(p => p.Id.Equals(id), item);
        }

        public override void Remove(Task item)
        {
            this.Remove(item.Id);
        }

        public override void Remove(string id)
        {
            this._collection.DeleteOne(p => p.Id.Equals(id));
        }
    }
}
