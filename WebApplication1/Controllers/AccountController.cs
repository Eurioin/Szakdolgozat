using Microsoft.AspNetCore.Mvc;
using Szakdolgozat.Services;
using Microsoft.AspNetCore.Authorization;
using Szakdolgozat.Models.DatabaseModels;
using System.Linq;
using System.Collections.Generic;

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
        public ActionResult<Account> Update([FromBody]Account account)
        {
            this.accountService.Update(account.Id, account);
            return accountService.GetById(account.Id);
        }
    }
}
