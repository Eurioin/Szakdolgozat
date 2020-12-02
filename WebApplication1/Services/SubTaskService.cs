using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models.DatabaseModels;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Services
{
    public class SubTaskService : AbstractServiceBase<SubTask>
    {
        public SubTaskService(SubTasksDatabaseSettings settings) : base(settings) {}

        public override SubTask GetByProperty(string propertyname, string value)
        {
            switch (propertyname.ToLower())
            {
                case "description":
                    return this._collection.Find(p => p.Description.Equals(value)).FirstOrDefault();
                default:
                    return this.GetById(value);
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
