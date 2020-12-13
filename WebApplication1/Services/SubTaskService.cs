using System.Linq;
using MongoDB.Driver;
using System.Collections.Generic;
using Szakdolgozat.Services.Interfaces;
using Szakdolgozat.Models.DatabaseModels;

namespace Szakdolgozat.Services
{
    public class SubTaskService : AbstractServiceBase<SubTask>
    {
        public SubTaskService(SubTasksDatabaseSettings settings) : base(settings) { }

        public override List<SubTask> GetByProperty(string propertyname, string value)
        {
            switch (propertyname.ToLower())
            {
                case "description":
                    return this._collection.Find(p => p.Description.Equals(value)).ToList();
                case "parenttaskid":
                    return this._collection.Find(p => p.ParentTaksId.Equals(value)).ToList();
                default:
                    return new List<SubTask>() { this.GetById(value) };
            }
        }

        public override SubTask GetById(string id)
        {
            return this._collection.Find(p => p.Id.Equals(id)).FirstOrDefault();
        }

        public override void Update(string id, SubTask item)
        {
            this._collection.ReplaceOne(p => p.Id.Equals(id), item);
        }

        public override void Remove(SubTask item)
        {
            this.Remove(item.Id);
        }

        public override void Remove(string id)
        {
            this._collection.DeleteOne(p => p.Id.Equals(id));
        }
    }
}
