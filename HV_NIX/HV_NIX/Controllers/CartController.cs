using HV_NIX.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace HV_NIX.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDbContext db = new AppDbContext();

        // 🛒 1️⃣ Hiển thị giỏ hàng
        public ActionResult Index()
        {
            var cart = Session["Cart"] as List<CartItem> ?? new List<CartItem>();
            ViewBag.TotalAmount = cart.Sum(x => x.Price * x.Quantity);
            return View(cart);
        }

        // 🟩 2️⃣ Thêm sản phẩm vào giỏ
        [HttpPost]
        public ActionResult AddToCart(int id, string size = "M")
        {
            var product = db.Products.FirstOrDefault(p => p.ProductID == id);
            if (product == null)
                return Json(new { success = false, message = "❌ Không tìm thấy sản phẩm." });

            var cart = Session["Cart"] as List<CartItem> ?? new List<CartItem>();

            var existingItem = cart.FirstOrDefault(c => c.ProductID == id && c.Size == size);
            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                cart.Add(new CartItem
                {
                    ProductID = product.ProductID,
                    ProductName = product.ProductName,
                    Price = product.Price,
                    Thumbnail = product.Thumbnail,   // ✔ FIX mới
                    Quantity = 1,
                    Size = size
                });
            }

            Session["Cart"] = cart;

            return Json(new
            {
                success = true,
                totalItems = cart.Sum(x => x.Quantity),
                message = $"🛒 {product.ProductName} (Size {size}) đã được thêm vào giỏ!"
            });
        }

        // 🧮 3️⃣ Lấy tổng số lượng sản phẩm trong giỏ
        [HttpGet]
        public ActionResult GetCartCount()
        {
            var cart = Session["Cart"] as List<CartItem> ?? new List<CartItem>();
            int totalItems = cart.Sum(x => x.Quantity);
            return Json(new { totalItems }, JsonRequestBehavior.AllowGet);
        }

        // 🔁 4️⃣ Cập nhật số lượng sản phẩm
        [HttpPost]
        public ActionResult UpdateQuantity(int id, string size, int quantity)
        {
            if (quantity <= 0)
                return Json(new { success = false, message = "Số lượng không hợp lệ." });

            var cart = Session["Cart"] as List<CartItem>;
            if (cart != null)
            {
                var item = cart.FirstOrDefault(c => c.ProductID == id && c.Size == size);
                if (item != null)
                {
                    item.Quantity = quantity;
                }

                Session["Cart"] = cart;
            }

            return Json(new
            {
                success = true,
                totalItems = cart.Sum(x => x.Quantity),
                totalAmount = cart.Sum(x => x.Price * x.Quantity).ToString("N0") + "₫"
            });
        }

        // 🗑️ 5️⃣ Xóa 1 sản phẩm
        [HttpPost]
        public ActionResult Remove(int id, string size)
        {
            var cart = Session["Cart"] as List<CartItem>;
            if (cart != null)
            {
                var item = cart.FirstOrDefault(c => c.ProductID == id && c.Size == size);
                if (item != null)
                    cart.Remove(item);

                Session["Cart"] = cart;
            }
            return RedirectToAction("Index");
        }

        // ❌ 6️⃣ Xóa toàn bộ giỏ hàng
        public ActionResult Clear()
        {
            Session.Remove("Cart");
            return RedirectToAction("Index");
        }

        // 💳 7️⃣ Trang thanh toán
        [HttpGet]
        public ActionResult Checkout()
        {
            var userId = Session["UserID"];
            if (userId == null)
            {
                TempData["CheckoutRedirect"] = true;
                TempData["Message"] = "⚠️ Vui lòng đăng nhập để thanh toán đơn hàng.";
                return RedirectToAction("Login", "Account");
            }

            var cart = Session["Cart"] as List<CartItem>;
            if (cart == null || !cart.Any())
            {
                TempData["Error"] = "Giỏ hàng của bạn đang trống.";
                return RedirectToAction("Index");
            }

            ViewBag.TotalAmount = cart.Sum(x => x.Price * x.Quantity);
            return View(cart);
        }

        // ✅ 8️⃣ Xác nhận thanh toán
        [HttpPost]
        public ActionResult CheckoutConfirm()
        {
            var userId = Session["UserID"];
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var cart = Session["Cart"] as List<CartItem>;
            if (cart == null || !cart.Any())
            {
                TempData["Error"] = "Không có sản phẩm nào trong giỏ hàng.";
                return RedirectToAction("Index");
            }

            // 🧾 Tạo đơn hàng mới
            var order = new Orders
            {
                UserID = Convert.ToInt32(userId),
                OrderDate = DateTime.Now,
                Total = cart.Sum(x => x.Price * x.Quantity),   // ✔ FIX theo model mới
                Status = "Pending"
            };

            db.Orders.Add(order);
            db.SaveChanges();

            // ➕ Thêm chi tiết đơn hàng
            foreach (var item in cart)
            {
                db.OrderDetails.Add(new OrderDetails
                {
                    OrderID = order.OrderID,
                    ProductID = item.ProductID,
                    Quantity = item.Quantity,
                    Price = item.Price
                });
            }

            db.SaveChanges();

            // 🧹 Xóa giỏ hàng
            Session.Remove("Cart");
            TempData["OrderID"] = order.OrderID;
            TempData["Success"] = "🎉 Đặt hàng thành công!";

            return RedirectToAction("ThankYou");
        }

        public ActionResult ThankYou()
        {
            if (TempData["OrderID"] == null)
                return RedirectToAction("Index", "Home");

            int orderId = Convert.ToInt32(TempData["OrderID"]);
            TempData.Keep("OrderID");

            var order = db.Orders.FirstOrDefault(o => o.OrderID == orderId);

            var details = (from d in db.OrderDetails
                           join p in db.Products on d.ProductID equals p.ProductID
                           where d.OrderID == orderId
                           select new ThankYouItem
                           {
                               ProductName = p.ProductName,
                               Thumbnail = p.Thumbnail,
                               Quantity = d.Quantity,
                               Price = d.Price
                           }).ToList();

            ViewBag.Order = order;

            return View(details);
        }
    }

    // 👉 Model hiển thị ở trang cảm ơn
    public class ThankYouItem
    {
        public string ProductName { get; set; }
        public string Thumbnail { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }

    // 🧩 Model phụ cho giỏ hàng
    public class CartItem
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string Thumbnail { get; set; }   // ✔ FIX
        public int Quantity { get; set; }
        public string Size { get; set; }
    }
}
