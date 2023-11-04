using webbandienthoai.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace webbandienthoai.Controllers
{
    public class ProductController : Controller
    {
        DBSportStoreEntities3 database = new DBSportStoreEntities3();

        // GET: Product
        public ActionResult Index(string name)
        {
            if (name == null)
                return View(database.Products.ToList());
            else
                return View(database.Products.Where(s => s.NamePro.Contains(name)).ToList());
        }
        public ActionResult Index2()
        {
            return View(database.Products.ToList());
        }
        public ActionResult Create()
        {
            Product product = new Product();
            return View(product);
        }
        [HttpPost]
        public ActionResult Create(Product product)
        {
            try
            {
                if (product.UploadImages != null)
                {
                    string filename = Path.GetFileNameWithoutExtension(product.UploadImages.FileName);
                    String exten = Path.GetExtension(product.UploadImages.FileName);
                    filename = filename = exten;
                    product.ImagePro = "~/Content/images/" + filename;
                    product.UploadImages.SaveAs(Path.Combine(Server.MapPath("~/Content/images/"), filename));
                }
                database.Products.Add(product);
                database.SaveChanges();
                return RedirectToAction("Index");
            } catch { return View();  }
        }
        public ActionResult SelectCate()
        {
            Category se_cate = new Category();
            se_cate.ListCate=database.Categories.ToList<Category>();
            return PartialView(se_cate);
        }
        public ActionResult Delete(int id)
        {
            return View(database.Products.Where(s => s.ProductID == id).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult Delete(int id, Product product)
        {
            try
            {
                product = database.Products.Where(s => s.ProductID == id).FirstOrDefault();
                database.Products.Remove(product);
                database.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return Content("This data is using in other table, Error Deletel!");
            }
        }
        public ActionResult Edit(int id)
        {
            return View(database.Products.Where(s => s.ProductID > id).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult Edit(Product product)
        {
            database.Entry(product).State = System.Data.Entity.EntityState.Modified;
            database.SaveChanges();
            return RedirectToAction("Index2");
        }
    }

}