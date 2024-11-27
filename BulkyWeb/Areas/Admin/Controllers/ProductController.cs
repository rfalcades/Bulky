using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        public ProductController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var categories = unitOfWork.Product.GetAll().ToList();

            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Create(Product Product)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.Product.Add(Product);
                unitOfWork.Save();
                TempData["message"] = "Product has been created successfully";
                return RedirectToAction("Index");
            }

            return View(Product);
        }


        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Product? ProductFromDb = unitOfWork.Product.Get(_ => _.Id == id);

            if (ProductFromDb == null)
            {
                return NotFound();
            }

            return View(ProductFromDb);
        }


        [HttpPost]
        public IActionResult Edit(Product Product)
        {

            if (ModelState.IsValid)
            {
                unitOfWork.Product.Update(Product);
                unitOfWork.Save();
                TempData["message"] = "Product has been updated successfully";
                return RedirectToAction("Index");
            }

            return View(Product);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Product? ProductFromDb = unitOfWork.Product.Get(_ => _.Id == id);

            if (ProductFromDb == null)
            {
                return NotFound();
            }

            return View(ProductFromDb);
        }


        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Product? ProductFromDb = unitOfWork.Product.Get(_ => _.Id == id);

            if (ProductFromDb == null)
            {
                return NotFound();
            }

            unitOfWork.Product.Remove(ProductFromDb);
            unitOfWork.Save();
            TempData["message"] = "Product has been deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
