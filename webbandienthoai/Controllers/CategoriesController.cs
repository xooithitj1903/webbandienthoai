using webbandienthoai.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace webbandienthoai.Controllers
{
    public class CategoriesController : Controller
    {
        DBSportStoreEntities3 database = new DBSportStoreEntities3();
        // GET: Categories
        public ActionResult Index(string name)
        {
            if (name == null)
                return View(database.Categories.ToList());
            else
                return View(database.Categories.Where(s => s.NameCate.Contains(name)).ToList());
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Category category)
        {
            try
            {
                database.Categories.Add(category);
                database.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return Content("Error Create New");
            }
        }
        public ActionResult Details(int id)
        {
            return View(database.Categories.Where(s => s.Id == id).FirstOrDefault());
        }
        public ActionResult Edit(int id)
        {
            return View(database.Categories.Where(s => s.Id > id).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult Edit(Category category)
        {
            database.Entry(category).State = System.Data.Entity.EntityState.Modified;
            database.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int id)
        {
            return View(database.Categories.Where(s => s.Id == id).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult Delete(int id, Category category)
        {
            try
            {
                category = database.Categories.Where(s => s.Id == id).FirstOrDefault();
                database.Categories.Remove(category);
                database.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return Content("This data is using in other table, Error Deletel!");
            }
        }  

    }
}