using HV_NIX.Models;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace HV_NIX.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext db = new AppDbContext();

        // ============================
        // 📌 LIST SẢN PHẨM + PHÂN TRANG
        // ============================
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

        // ============================
        // 📌 CHI TIẾT SẢN PHẨM
        // ============================
        public ActionResult Details(int id)
        {
            var product = db.Products
                            .Include(p => p.Category)
                            .FirstOrDefault(p => p.ProductID == id);

            if (product == null)
                return HttpNotFound();

            return View(product);
        }

        // ============================
        // 📌 LỌC THEO DANH MỤC CategoryID
        // ============================
        public ActionResult Category(int id)
        {
            var category = db.Categories.FirstOrDefault(c => c.CategoryID == id);

            if (category == null)
                return HttpNotFound("Danh mục không tồn tại!");

            var products = db.Products
                             .Where(p => p.CategoryID == id)
                             .OrderBy(p => p.ProductID)
                             .ToList();

            ViewBag.CategoryName = category.CategoryName;

            return View("Category", products);
        }

        // ============================
        // 📌 GET: CREATE
        // ============================
        public ActionResult Create()
        {
            ViewBag.Categories = db.Categories.ToList();
            return View();
        }

        // ============================
        // 📌 POST: CREATE
        // ============================
        [HttpPost]
        public ActionResult Create(Products product, HttpPostedFileBase ThumbnailFile)
        {
            // Validate Category
            if (product.CategoryID <= 0)
            {
                ModelState.AddModelError("", "Vui lòng chọn danh mục.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = db.Categories.ToList();
                return View(product);
            }

            // 📌 Upload ảnh Thumbnail
            if (ThumbnailFile != null && ThumbnailFile.ContentLength > 0)
            {
                string fileName = Guid.NewGuid() + Path.GetExtension(ThumbnailFile.FileName);
                string savePath = Server.MapPath("~/Content/Images/Products/" + fileName);

                ThumbnailFile.SaveAs(savePath);
                product.Thumbnail = fileName;
            }

            db.Products.Add(product);
            db.SaveChanges();

            TempData["Success"] = "Thêm sản phẩm thành công!";
            return RedirectToAction("Index");
        }
    }
}
