using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Services.Interfaces
{
    public interface IMongoCrudRepository<T>
    {
        T GetById(string id);

        T GetByProperty(string propertyname, string value);

        List<T> GetAll();

        void Create(T item);

        void Update(string id, T item);

        void Remove(T item);

        void Remove(string id);
    }
}
