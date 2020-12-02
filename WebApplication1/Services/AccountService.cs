using MongoDB.Driver;
using System;
using Szakdolgozat.Models.DatabaseModels;
using Szakdolgozat.Services.Interfaces;

namespace Szakdolgozat.Services
{
    public class AccountService : AbstractServiceBase<Account>
    {
        public AccountService(AccountsDatabaseSettings settings) : base(settings) {}

        public override Account GetByProperty(string propertyname, string value)
        {
            switch (propertyname.ToLower())
            {
                case "name":
                    return this._collection.Find(p => p.Name.Equals(value)).FirstOrDefault();
                case "username":
                    return this._collection.Find(p => p.Username.Equals(value)).FirstOrDefault();
                case "email":
                    return this._collection.Find(p => p.Email.Equals(value)).FirstOrDefault();
                case "phonenumber":
                    return this._collection.Find(p => p.PhoneNumber.Equals(value)).FirstOrDefault();
                case "url":
                    return this._collection.Find(p => p.Url.Equals(value)).FirstOrDefault();
                default:
                    return this.GetById(value);
            }
        }

        public override Account GetById(string id)
        {
            return this._collection.Find(p => p.Id.Equals(id)).FirstOrDefault();
        }

        public override void Update(string id, Account item)
        {
            this._collection.ReplaceOne(p => p.Id.Equals(id), item);
        }

        public override void Remove(Account item)
        {
            this.Remove(item.Id);
        }

        public override void Remove(string id)
        {
            this._collection.DeleteOne(p => p.Id.Equals(id));
        }
    }
}
