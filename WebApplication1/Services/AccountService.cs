using MongoDB.Driver;
using System.Collections.Generic;
using Szakdolgozat.Models.DatabaseModels;
using Szakdolgozat.Services.Interfaces;

namespace Szakdolgozat.Services
{
    public class AccountService : AbstractServiceBase<Account>
    {
        public AccountService(AccountsDatabaseSettings settings) : base(settings) { }

        public override List<Account> GetByProperty(string propertyname, string value)
        {
            switch (propertyname.ToLower())
            {
                case "name":
                    return this._collection.Find(p => p.Name.Equals(value)).ToList();
                case "username":
                    return this._collection.Find(p => p.Username.Equals(value)).ToList();
                case "email":
                    return this._collection.Find(p => p.Email.Equals(value)).ToList();
                case "phonenumber":
                    return this._collection.Find(p => p.PhoneNumber.Equals(value)).ToList();
                case "url":
                    return this._collection.Find(p => p.Url.Equals(value)).ToList();
                default:
                    return new List<Account>() { this.GetById(value) };
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
