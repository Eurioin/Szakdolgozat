using System;
using System.Linq;
using MongoDB.Driver;
using WebApplication1.Models.DatabaseModels;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Services
{
    public class ProjectService : AbstractServiceBase<Project> 
    {
        public ProjectService(ProjectsDatabaseSettings settings) : base(settings) {}

        public override Project GetByProperty(string propertyname, string value)
        {
            switch (propertyname.ToLower())
            {
                case "name":
                    return this._collection.Find(p => p.Name.Equals(value)).FirstOrDefault();
                case "dateofcreation":
                    var date = DateTime.Parse(value);
                    return this._collection.Find(p => p.DateOfCreation.Equals(date)).FirstOrDefault();
                default:
                    return this.GetById(value);
            }
        }

        public override Project GetById(string id)
        {
            return this._collection.Find(p => p.Id.Equals(id)).FirstOrDefault();
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
