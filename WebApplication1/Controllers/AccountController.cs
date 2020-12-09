using Microsoft.AspNetCore.Mvc;
using Szakdolgozat.Services;
using Microsoft.AspNetCore.Authorization;
using Szakdolgozat.Models.DatabaseModels;
using System.Linq;
using System.Collections.Generic;
using Szakdolgozat.Models.DTOModels;

namespace Szakdolgozat.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly AccountService accountService;

        public AccountController(AccountService service)
        {
            this.accountService = service;
        }

        [HttpGet("user")]
        public ActionResult<Account> Details(string username)
        {
            return this.accountService.GetAll().Where(u => u.Username.Equals(username)).FirstOrDefault();
        }

        [HttpGet("user/getById")]
        public ActionResult<Account> GetById(string id)
        {
            return this.accountService.GetById(id);
        }

        [HttpGet("accounts")]
        public ActionResult<IEnumerable<Account>> GetProfiles()
        {
            return this.accountService.GetAll();
        }

        [HttpPost("update")]
        public ActionResult<Account> Update([FromBody]FromAngularAccount account)
        {
            var acc = this.accountService.GetById(account.id);
            acc.Email = account.email;
            acc.Name = account.name;
            acc.PhoneNumber = account.phoneNumber;

            string[] passedRoles;

            if (account.roles.Count(x => x == ';') == 1 && account.roles.LastIndexOf(';') == account.roles.Length - 1)
            {
                passedRoles = new string[1];
                passedRoles[0] = account.roles.Substring(0, account.roles.LastIndexOf(';'));
            }
            else
            {
                passedRoles = account.roles.Split(';');
            }

            acc.UniqueRoles = new List<string>();

            foreach (var role in passedRoles)
            {
                if (role.Length > 0)
                {
                    acc.UniqueRoles.Add(role);
                }
            }

            this.accountService.Update(acc.Id, acc);
            return accountService.GetById(acc.Id);
        }
    }
}
