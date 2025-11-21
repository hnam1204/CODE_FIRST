using HV_NIX.Models;
using System.Linq;
using System.Web.Mvc;

namespace HV_NIX.Controllers
{
    public class PaymentController : Controller
    {
        private readonly AppDbContext db = new AppDbContext();

        public ActionResult Index()
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Login", "Account");

            int uid = (int)Session["UserID"];

            var list = db.PaymentMethods
                         .Where(x => x.UserID == uid)
                         .OrderByDescending(x => x.IsDefault)
                         .ToList();

            return View(list);
        }

        [HttpPost]
        public ActionResult Add(string methodType, string provider, string accountNumber, bool isDefault)
        {
            int uid = (int)Session["UserID"];

            if (isDefault)
            {
                var old = db.PaymentMethods.Where(x => x.UserID == uid && x.IsDefault);
                foreach (var o in old) o.IsDefault = false;
            }

            db.PaymentMethods.Add(new PaymentMethod
            {
                UserID = uid,
                MethodType = methodType,
                Provider = provider,
                AccountNumber = accountNumber,
                IsDefault = isDefault
            });

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult SetDefault(int id)
        {
            int uid = (int)Session["UserID"];

            var pm = db.PaymentMethods.FirstOrDefault(x => x.MethodID == id && x.UserID == uid);
            if (pm == null) return RedirectToAction("Index");

            var all = db.PaymentMethods.Where(x => x.UserID == uid);
            foreach (var m in all) m.IsDefault = false;

            pm.IsDefault = true;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            int uid = (int)Session["UserID"];
            var pm = db.PaymentMethods.FirstOrDefault(x => x.MethodID == id && x.UserID == uid);

            if (pm != null)
            {
                db.PaymentMethods.Remove(pm);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}
