using HV_NIX.Models;   // ★ Sửa đúng namespace Model của bạn
using System.Web.Mvc;

namespace HV_NIX.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext db = new AppDbContext();

        public ActionResult Index()
        {
            return View();
        }

        // 🧾 Chính sách đổi trả
        public ActionResult ReturnPolicy()
        {
            return View();
        }

        // 🚚 Chính sách vận chuyển
        public ActionResult ShippingPolicy()
        {
            return View();
        }

        // 📦 Chính sách kiểm hàng
        public ActionResult InspectionPolicy()
        {
            return View();
        }

        // 🔒 Chính sách bảo mật
        public ActionResult PrivacyPolicy()
        {
            return View();
        }

        // 💳 Chính sách thanh toán
        public ActionResult PaymentPolicy()
        {
            return View();
        }
    }
}
