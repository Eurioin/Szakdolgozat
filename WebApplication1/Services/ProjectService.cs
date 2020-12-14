using System;
using System.Linq;
using MongoDB.Driver;
using System.Collections.Generic;
using Szakdolgozat.Services.Interfaces;
using Szakdolgozat.Models.DatabaseModels;

namespace Szakdolgozat.Services
{
    public class ProjectService : AbstractServiceBase<Project> 
    {
        public ProjectService(ProjectsDatabaseSettings settings) : base(settings) {}

        public override List<Project> GetByProperty(string propertyname, string value)
        {
            switch (propertyname.ToLower())
            {
                case "name":
                    return this._collection.Find(p => p.Name.Equals(value)).ToList();
                case "dateofcreation":
                    var date = DateTime.Parse(value);
                    return this._collection.Find(p => p.DateOfCreation.Equals(date)).ToList();
                default:
                    return new List<Project>() { this.GetById(value) };
            }
        }

        public override Project GetById(string id)
        {
            if (id != null ||id != "null")
            {
                return this._collection.Find(p => p.Id.Equals(id)).FirstOrDefault();
            }
            return null;
        }

        public override void Update(string id, Project item)
        {
            this._collection.ReplaceOne(p => p.Id.Equals(id), item);
        }

        public override void Remove(Project item)
        {
            this.Remove(item.Id);
        }

        public override void Remove(string id)
        {
            this._collection.DeleteOne(p => p.Id.Equals(id));
        }
    }
}
