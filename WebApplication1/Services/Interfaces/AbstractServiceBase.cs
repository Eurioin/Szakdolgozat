using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Szakdolgozat.Models.DatabaseModels;

namespace Szakdolgozat.Services.Interfaces
{
    public abstract class AbstractServiceBase<T> : IMongoCrudRepository<T>
    {
        protected readonly IMongoCollection<T> _collection;

        public AbstractServiceBase(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var db = client.GetDatabase(settings.DatabaseName);

            _collection = db.GetCollection<T>(settings.CollectionName);
        }

        public virtual void Create(T item)
        {
            this._collection.InsertOne(item);
        }

        public virtual List<T> GetAll()
        {
            return this._collection.Find(p => true).ToList();
        }

        public virtual T GetById(string id)
        {
            throw new NotImplementedException();
        }

        public virtual List<T> GetByProperty(string propertyname, string value)
        {
            throw new NotImplementedException();
        }

        public virtual void Remove(T item)
        {
            throw new NotImplementedException();
        }

        public virtual void Remove(string id)
        {
            throw new NotImplementedException();
        }

        public virtual void Update(string id, T item)
        {
            throw new NotImplementedException();
        }
    }
}
