using HV_NIX.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace HV_NIX.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext db = new AppDbContext();

        // ============================
        // ✔ REGISTER (GET)
        // ============================
        [HttpGet]
        public ActionResult Register()
        {
            if (Session["UserID"] != null)
                return RedirectToAction("Index", "Home");

            return View();
        }

        // ============================
        // ✔ REGISTER (POST)
        // ============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(string fullname, string phone, string address,
                                    string email, string password, string confirmPassword)
        {
            if (string.IsNullOrEmpty(fullname) || string.IsNullOrEmpty(phone) ||
                string.IsNullOrEmpty(address) || string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "⚠️ Vui lòng nhập đầy đủ thông tin.";
                return View();
            }

            if (password != confirmPassword)
            {
                ViewBag.Error = "⚠️ Mật khẩu xác nhận không khớp.";
                return View();
            }

            if (db.Users.Any(u => u.Email == email))
            {
                ViewBag.Error = "⚠️ Email đã tồn tại.";
                return View();
            }

            // Tạo user
            var user = new Users
            {
                Email = email,
                PasswordHash = password,
                CreatedAt = DateTime.Now
            };
            db.Users.Add(user);
            db.SaveChanges();

            // Tạo UserInfo (không có City, PaymentMethod)
            var info = new UserInfo
            {
                UserID = user.UserID,
                FullName = fullname,
                Phone = phone,
                Address = address
            };
            db.UserInfos.Add(info);
            db.SaveChanges();

            // Tạo session
            Session["UserID"] = user.UserID;
            Session["UserName"] = fullname;

            TempData["Success"] = "🎉 Đăng ký thành công!";
            return RedirectToAction("Index", "Home");
        }

        // ============================
        // ✔ LOGIN (GET)
        // ============================
        [HttpGet]
        public ActionResult Login()
        {
            if (Session["UserID"] != null)
                return RedirectToAction("Index", "Home");

            return View();
        }

        // ============================
        // ✔ LOGIN (POST)
        // ============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string password)
        {
            var user = db.Users.FirstOrDefault(u => u.Email == email && u.PasswordHash == password);
            if (user == null)
            {
                ViewBag.Error = "❌ Sai email hoặc mật khẩu.";
                return View();
            }

            Session["UserID"] = user.UserID;
            Session["UserName"] = db.UserInfos.FirstOrDefault(i => i.UserID == user.UserID)?.FullName;

            return RedirectToAction("Index", "Home");
        }

        // ============================
        // ✔ LOGOUT
        // ============================
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        // ============================
        // ✔ CHECK LOGIN (AJAX)
        // ============================
        [HttpGet]
        public ActionResult CheckLogin()
        {
            return Json(new { isLoggedIn = Session["UserID"] != null }, JsonRequestBehavior.AllowGet);
        }

        // ============================
        // ✔ MY PROFILE (GET)
        // ============================
        [HttpGet]
        public ActionResult MyProfile()
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Login");

            int uid = Convert.ToInt32(Session["UserID"]);
            var info = db.UserInfos.FirstOrDefault(i => i.UserID == uid);

            if (info == null)
            {
                TempData["Error"] = "Không tìm thấy thông tin người dùng.";
                return RedirectToAction("Index", "Home");
            }

            return View(info);
        }

        // ============================
        // ✔ MY PROFILE (POST)
        // ============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MyProfile(UserInfo model)
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Login");

            int uid = Convert.ToInt32(Session["UserID"]);
            var info = db.UserInfos.FirstOrDefault(i => i.UserID == uid);

            if (info == null)
            {
                TempData["Error"] = "Không tìm thấy thông tin người dùng.";
                return RedirectToAction("Index", "Home");
            }

            info.FullName = model.FullName;
            info.Phone = model.Phone;
            info.Address = model.Address;

            db.SaveChanges();

            TempData["Success"] = "Cập nhật hồ sơ thành công!";
            return RedirectToAction("MyProfile");
        }

        // ============================
        // ✔ LẤY LỊCH SỬ ĐƠN HÀNG
        // ============================
        [HttpGet]
        public ActionResult OrderHistory()
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Login");

            int uid = Convert.ToInt32(Session["UserID"]);

            var orders = db.Orders
                .Where(o => o.UserID == uid)
                .OrderByDescending(o => o.OrderDate)
                .ToList();

            return View(orders);
        }

        // ============================
        // ✔ LẤY CHI TIẾT ĐƠN HÀNG + REVIEW
        // ============================
        [HttpGet]
        public JsonResult GetOrderDetail(int orderId)
        {
            if (Session["UserID"] == null)
                return Json(new { error = "NotLogin" }, JsonRequestBehavior.AllowGet);

            int uid = Convert.ToInt32(Session["UserID"]);

            var order = db.Orders.FirstOrDefault(o => o.OrderID == orderId && o.UserID == uid);
            if (order == null)
                return Json(new { error = "Không tìm thấy đơn hàng" }, JsonRequestBehavior.AllowGet);

            var details = db.OrderDetails
                .Where(d => d.OrderID == orderId)
                .Select(d => new
                {
                    d.ProductID,
                    ProductName = d.Product.ProductName,
                    ImageURL = d.Product.Thumbnail,
                    d.Quantity,
                    d.Price,
                    HasReview = db.Reviews.Any(r =>
                        r.ProductID == d.ProductID &&
                        r.OrderID == orderId &&
                        r.UserID == uid)
                })
                .ToList();

            return Json(new
            {
                order.OrderID,
                Date = order.OrderDate.ToString("dd/MM/yyyy HH:mm"),
                Total = order.Total,
                order.Status,
                Details = details
            }, JsonRequestBehavior.AllowGet);
        }

        // ============================
        // ✔ GỬI REVIEW
        // ============================
        [HttpPost]
        public JsonResult SubmitReview(int ProductID, int OrderID, int Rating, string Comment)
        {
            if (Session["UserID"] == null)
                return Json(new { success = false, message = "Vui lòng đăng nhập" });

            int uid = Convert.ToInt32(Session["UserID"]);

            var check = db.OrderDetails.Any(od =>
                od.OrderID == OrderID && od.ProductID == ProductID);

            if (!check)
                return Json(new { success = false, message = "Sản phẩm không thuộc đơn hàng" });

            db.Reviews.Add(new Reviews
            {
                ProductID = ProductID,
                UserID = uid,
                OrderID = OrderID,
                Rating = Rating,
                Comment = Comment ?? "",
                ReviewDate = DateTime.Now
            });

            db.SaveChanges();

            return Json(new { success = true, message = "Đánh giá thành công!" });
        }
        // ============================
        // ✔ TRANG 'Đánh giá của tôi'
        // ============================
        public ActionResult MyReviews()
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Login");

            int uid = (int)Session["UserID"];

            var list = db.Reviews
                .Where(r => r.UserID == uid)
                .OrderByDescending(r => r.ReviewDate)
                .ToList();

            return View(list);
        }

        // ============================
        // ✔ XÓA REVIEW
        // ============================
        public ActionResult DeleteReview(int id)
        {
            int uid = (int)Session["UserID"];

            var review = db.Reviews.FirstOrDefault(r => r.ReviewID == id && r.UserID == uid);

            if (review != null)
            {
                db.Reviews.Remove(review);
                db.SaveChanges();
            }

            return RedirectToAction("MyReviews");
        }
        public ActionResult Notifications()
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Login");

            int uid = (int)Session["UserID"];

            var list = db.Notifications
                .Where(n => n.UserID == uid)
                .OrderByDescending(n => n.CreatedAt)
                .ToList();

            return View(list);
        }

        // Đánh dấu đã đọc
        public ActionResult MarkRead(int id)
        {
            var noti = db.Notifications.FirstOrDefault(n => n.NotificationID == id);
            if (noti != null)
            {
                noti.IsRead = true;
                db.SaveChanges();
            }
            return RedirectToAction("Notifications");
        }
        public ActionResult ActivityLog()
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Login");

            int uid = (int)Session["UserID"];

            var logs = db.ActivityLogs
                         .Where(l => l.UserID == uid)
                         .OrderByDescending(l => l.Date)
                         .ToList();

            return View(logs);
        }

    }
}
