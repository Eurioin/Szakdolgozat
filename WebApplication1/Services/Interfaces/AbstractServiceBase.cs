using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Services.Interfaces
{
    public abstract class AbstractServiceBase<T> : IMongoCrudRepository<T>
    {
        public void Create(T item)
        {
            throw new NotImplementedException();
        }

        public List<T> GetAll()
        {
            throw new NotImplementedException();
        }

        public T GetById(string id)
        {
            throw new NotImplementedException();
        }

        public T GetByProperty(string propertyname, string value)
        {
            throw new NotImplementedException();
        }

        public void Remove(T item)
        {
            throw new NotImplementedException();
        }

        public void Remove(string id)
        {
            throw new NotImplementedException();
        }

        public void Update(string id, T item)
        {
            throw new NotImplementedException();
        }
    }
}
