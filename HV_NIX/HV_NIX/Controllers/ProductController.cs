using HV_NIX.Models;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HV_NIX.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext db = new AppDbContext();

        // 👉 Danh sách sản phẩm có phân trang
        public ActionResult Index(int page = 1, int pageSize = 8)
        {
            int totalProducts = db.Products.Count();
            int totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);

            var products = db.Products
                             .OrderBy(p => p.ProductID)
                             .Skip((page - 1) * pageSize)
                             .Take(pageSize)
                             .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(products);
        }

        // 👉 Trang chi tiết sản phẩm
        public ActionResult Details(int id)
        {
            var product = db.Products
                            .Include("Category")   // load luôn category
                            .FirstOrDefault(p => p.ProductID == id);

            if (product == null)
                return HttpNotFound();

            return View(product);
        }

        // 👉 Sản phẩm theo danh mục (CategoryID)
        public ActionResult Category(int id)
        {
            // Lấy Category đúng theo Model mới
            var category = db.Categories.FirstOrDefault(c => c.CategoryID == id);

            if (category == null)
                return HttpNotFound("Danh mục không tồn tại!");

            // Lọc theo CategoryID (CHUẨN Model mới)
            var products = db.Products
                             .Where(p => p.CategoryID == id)
                             .OrderBy(p => p.ProductID)
                             .ToList();

            ViewBag.CategoryName = category.CategoryName;

            return View("Category", products);
        }
        // GET: Product/Create
        public ActionResult Create()
        {
            ViewBag.Categories = db.Categories.ToList();
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        public ActionResult Create(Products product, HttpPostedFileBase ThumbnailFile)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = db.Categories.ToList();
                return View(product);
            }

            if (ThumbnailFile != null && ThumbnailFile.ContentLength > 0)
            {
                string fileName = Guid.NewGuid() + Path.GetExtension(ThumbnailFile.FileName);
                string path = Server.MapPath("~/Content/Images/Products/" + fileName);
                ThumbnailFile.SaveAs(path);

                product.Thumbnail = fileName;
            }

            // 🚨 Nếu CategoryID = 0 → ép lỗi
            if (product.CategoryID <= 0)
            {
                ModelState.AddModelError("", "Vui lòng chọn danh mục.");
                ViewBag.Categories = db.Categories.ToList();
                return View(product);
            }

            db.Products.Add(product);
            db.SaveChanges();

            TempData["Success"] = "Thêm sản phẩm thành công!";
            return RedirectToAction("Index");
        }
    }
}