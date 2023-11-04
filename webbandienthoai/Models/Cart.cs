using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webbandienthoai.Models
{
    public class CartItem
    {
        public Product product { get; set; }
        public int quantity { get; set; }
    }
    public class Cart
    {
        List<CartItem> items = new List<CartItem>();
        public IEnumerable<CartItem> Items { get { return items; } }
        public void Add_Product_Cart(Product pro, int quan = 1)
        {
            var item = Items.FirstOrDefault(s => s.product.ProductID == pro.ProductID);
            if (item == null) items.Add(new CartItem { product = pro, quantity = quan });
            else item.quantity += quan;
        }
        public int Total_quantity()
        {
            return items.Sum(s => s.quantity);
        }
        public decimal Total_money()    
        {
            var total = items.Sum(s => s.quantity * s.product.Price);
            return (decimal)total;
        }
        public void Update_quantity(int id, int new_quan)
        {
            var item = items.Find(s => s.product.ProductID == id);
            if (item != null)
                item.quantity = new_quan;
        }
        public void Remove_cartitem(int id)
        {
            items.RemoveAll(s => s.product.ProductID == id);
        }
        public void ClearCart()
        {
            items.Clear();
        }
    }
}