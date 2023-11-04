using webbandienthoai.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace webbandienthoai.Controllers
{
    public class LoginUserController : Controller
    {
        DBSportStoreEntities3 database = new DBSportStoreEntities3();

        // GET: LoginUser
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LoginAcount(AdminUser user)
        {
            var check = database.AdminUsers.Where(s => s.NameUser == user.NameUser && s.PasswordUser == user.PasswordUser).FirstOrDefault();
            if("admin" == user.NameUser && "123456789" == user.PasswordUser)
            {
                database.Configuration.ValidateOnSaveEnabled = false;
                Session["ID"] = "admin";
                return RedirectToAction("Index", "Product");
            }
            if (check == null)
            {
                ViewBag.ErrorInfo = "sai info";
                return View("Index");
            }else
            {
                database.Configuration.ValidateOnSaveEnabled = false;
                Session["ID"]= check.ID;
                Session["PasswordUser"]= check.PasswordUser;
                return RedirectToAction("Index", "Product");
                //return View("Index");

            }
        }
        public ActionResult Dangky()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Dangky(AdminUser user)
        {
            if(ModelState.IsValid)
            {
                var lastUserId = database.AdminUsers.OrderByDescending(u => u.ID).Select(u => u.ID) .FirstOrDefault();
                user.ID =lastUserId + 1;
                var check = database.AdminUsers.Where(s => s.NameUser == user.NameUser).FirstOrDefault();
                if(check == null)
                {
                    database.Configuration.ValidateOnSaveEnabled= false;
                    database.AdminUsers.Add(user);
                    database.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.ErrorRegister = "this ID is Exixst";
                    return View();
                }
            }
            return View();
        }
        public ActionResult Dangxuat()
        {
            Session.Clear();
            return RedirectToAction("Index", "Product");
        }
    }
}