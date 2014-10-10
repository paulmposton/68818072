using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bettery.WebRole.Models;
using Bettery.WebRole.Services;

namespace Bettery.WebRole.Controllers
{
    public class AdminController : Controller
    {
        private AdminService adminService;


        public AdminController()
            : this(new AdminService())
        {
        }

        public AdminController(AdminService adminService)
        {
            this.adminService = adminService;
        }

        //
        // GET: /Index
        [Authorize]
        public ActionResult Index()
        {
            if (Session["isAdmin"] == null)
                return RedirectToAction("LogIn", "Account");

            return View();
        }

        //
        // GET: /Admin/AccountList
        [Authorize]
        public ActionResult MemberList()
        {

            if (Session["isAdmin"] == null)
                return RedirectToAction("LogIn", "Account");

            IEnumerable<Member> model = adminService.GetMembers();

            return View(model);
        }

        //
        // GET: /Admin/EditMember
        [Authorize]
        public ActionResult EditMember(int id)
        {

            if (Session["isAdmin"] == null)
                return RedirectToAction("LogIn", "Account");

            EditMember model = adminService.GetMember(id);

            return View(model);
        }

        //
        // POST: /Admin/EditMember
        [Authorize]
        [HttpPost]
        public ActionResult EditMember(EditMember model)
        {
            if (Session["isAdmin"] == null)
                return RedirectToAction("LogIn", "Account");

            adminService.UpdateMember(model);

            return RedirectToAction("Index");
        }


        //
        // GET: /Admin/DeleteMember
        [Authorize]
        public ActionResult DeleteMember(int id)
        {

            return View(id);
        }

        //
        // POST: /Admin/DeleteMember
        // The int? foo is just for forcing a different signautre to DeleteMember
        [Authorize]
        [HttpPost]
        public ActionResult DeleteMember(int id, int? foo)
        {
            if (Session["isAdmin"] == null)
                return RedirectToAction("LogIn", "Account");

            adminService.DeleteMember(id);

            return RedirectToAction("MemberList");
        }


        //
        // GET: /Admin/AddPromoCode
        [Authorize]
        public ActionResult AddPromoCode()
        {

            if (Session["isAdmin"] == null)
                return RedirectToAction("LogIn", "Account");

            AddPromoModel model = new AddPromoModel();

            
            model.PromoCode = GenPromoCode();
            model.ExpireDate = DateTime.Today.AddDays(1);
            return View(model);
        }

        //
        // POST: /Admin/AddPromoCode
        [Authorize]
        [HttpPost]
        public ActionResult AddPromoCode(AddPromoModel model)
        {
            if (ModelState.IsValid)
            {
                if (Session["isAdmin"] == null)
                    return RedirectToAction("LogIn", "Account");

                if (model.Command == "ADD")
                {
                    string PostedPromoCode = model.PromoCode;
                    ModelState.Clear();

                    try
                    {
                        model.CreatedBy = (int)Session["MemberID"];
                        adminService.CreatePromo(model);
                        
                        model.Message = "Promo Code " + PostedPromoCode + " has been saved.  Enter a new Promo or cancel.";

                        Random random = new Random();
                        model.PromoCode = GenPromoCode();
                        model.ExpireDate = DateTime.Today.AddDays(1);
                        return View(model);
                    }
                    catch
                    {
                        model.Message = "Error Adding Promo";
                        return View(model);
                    }

                }
                else if (model.Command == "CANCEL")
                {
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }
        private string GenPromoCode()
        {
            string tmpPromoCode = string.Empty;
            bool isExistingPromoCode = false;
            Random random = new Random();
            while (!isExistingPromoCode)
            {
                tmpPromoCode = random.Next(1000000, 9999999).ToString();
                isExistingPromoCode = adminService.GetIsExistingPromoCode(tmpPromoCode);
            }
            return tmpPromoCode;
        }
    }
}
