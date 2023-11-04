using webbandienthoai.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Razor;

namespace webbandienthoai.Controllers
{
    public class ShoppingCartController : Controller
    {
        DBSportStoreEntities3 database = new DBSportStoreEntities3();
        public ActionResult ShowCart()
        {   
            if (Session["Cart"] == null)
                return View("EmptyCart");
            Cart cart = Session["Cart"] as Cart;
            return View(cart);
        }
        // GET: ShoppingCart
        public ActionResult Index()
        {
            return View();
        }
        public Cart GetCart()
        {
            Cart cart = Session["Cart"] as Cart;
            if (cart == null || Session["Cart"] == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }
            return cart;
        }
        public ActionResult AddToCart(int id)
        {
            var pro = database.Products.SingleOrDefault(s => s.ProductID == id);
            if (pro != null)
            {
                GetCart().Add_Product_Cart(pro);
            }
            return RedirectToAction("ShowCart", "ShoppingCart");
        }
        public ActionResult update_cart_quantity(FormCollection form)
        {
            Cart cart = Session["Cart"] as Cart;
            int idpro = int.Parse(form["ID"]);
            int quantity = int.Parse(form["carquantity"]);
            cart.Update_quantity(idpro, quantity);
            return RedirectToAction("ShowCart", "ShoppingCart");
        }
        public ActionResult RemoveCart(int id)
        {
            Cart cart = Session["Cart"] as Cart;
            cart.Remove_cartitem(id);
            return RedirectToAction("ShowCart", "ShoppingCart");
        }
        public PartialViewResult BagCart()
        {
            int total_quantity = 0;
            Cart cart = Session["Cart"] as Cart;
            if (cart != null)
                total_quantity = cart.Total_quantity();
            ViewBag.QuantityCart = total_quantity;
            return PartialView("BagCart");
        }
        public ActionResult DatHang()
        {
            int id = int.Parse(Session["ID"].ToString()); // Gán giá trị mặc định, hoặc giá trị mong muốn nếu Session không tồn tại
            var pro = database.AdminUsers.SingleOrDefault(s => s.ID == id);
            Cart cart = Session["Cart"] as Cart;
            OrderPro order = new OrderPro();
            order.DateOrder = DateTime.Now.Date;
            order.AddressDeliverry = pro.RoleUser;
            order.IDCus = pro.ID;
            database.OrderProes.Add(order);
            database.SaveChanges();
            foreach (var item in cart.Items)
            {
                OrderDetail detail = new OrderDetail();
                detail.IDOrder = order.ID;
                detail.IDProduct = item.product.ProductID;
                detail.UnitPrice = (double)item.product.Price;
                detail.Quantity = item.quantity;
                detail.ImagePro = item.product.ImagePro;
                detail.Price = item.product.Price;
                detail.NamePro = item.product.NamePro;
                database.OrderDetails.Add(detail);
                database.SaveChanges();
            }
            cart.ClearCart();
            return RedirectToAction("Index", "Product");
        }
        public ActionResult DanhsachDH()
        {
            var danhsach = database.OrderProes.ToList();
            return View(danhsach);
        }
        public ActionResult chitiet(int id)
        {
            var danhsach = database.OrderDetails.Where(s => s.IDOrder == id).ToList();
            return View(danhsach);
        }
    }
}