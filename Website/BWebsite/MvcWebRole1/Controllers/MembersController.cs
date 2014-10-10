using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bettery.WebRole.Services;
using Bettery.WebRole.Models;

namespace MvcWebRole1.Controllers
{
    public class MembersController : Controller
    {
        public MembersController()
            : this(new AccountService())
        {
        }

        public MembersController(AccountService accountService)
        {
            this.accountService = accountService;
        }


        private AccountService accountService;
        //
        // GET: /Members/Registration/5

        public ActionResult Registration(string id)
        {
            Member member = accountService.GetMember(id);
            return View(member);

        }

        //
        // POST: /Members/Registration/5

        [HttpPost]
        public ActionResult Registration(Member member)
        {
            if (ModelState.IsValid)
            {
                accountService.RegisterMember(member);
                return RedirectToAction("RegistrationSuccess", "Account");

            }
            return View(member);
        }


    }
}
