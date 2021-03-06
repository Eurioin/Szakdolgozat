﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Szakdolgozat.Models.DatabaseModels;

namespace Szakdolgozat.Services.Interfaces
{
    public interface IMongoCrudRepository<T> 
    {
        T GetById(string id);

        List<T> GetByProperty(string propertyname, string value);

        List<T> GetAll();

        void Create(T item);

        void Update(string id, T item);

        void Remove(T item);

        void Remove(string id);
    }
}
