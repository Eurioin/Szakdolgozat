using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Szakdolgozat.Models.DatabaseModels;
using Szakdolgozat.Services.Interfaces;

namespace Szakdolgozat.Services
{
    public class CommentService : AbstractServiceBase<Comment>
    {
        public CommentService(CommentsDatabaseSettings settings) : base(settings) { }

        public override Comment GetById(string id)
        {
            return this._collection.Find(c => c.Id.Equals(id)).FirstOrDefault();
        }

        public override List<Comment> GetByProperty(string propertyname, string value)
        {
            switch (propertyname.ToLower())
            {
                case "author":
                    return this._collection.Find(c => c.AuthorId.Equals(value)).ToList();
                case "dateofcreation":
                    return this._collection.Find(c => c.DateOfCreation.Equals(value)).ToList();
                case "content":
                    return this._collection.Find(c => c.Content.Equals(value)).ToList();
                case "task":
                    return this._collection.Find(c => c.Task.Equals(value)).ToList();
                default:
                    return new List<Comment>() { this.GetById(value) };
            }
        }

        public override void Remove(Comment item)
        {
            this.Remove(item.Id);
        }

        public override void Remove(string id)
        {
            this._collection.DeleteOne(c => c.Id.Equals(id));
        }

        public override void Update(string id, Comment item)
        {
            this._collection.ReplaceOne(id, item);
        }
    }
}
