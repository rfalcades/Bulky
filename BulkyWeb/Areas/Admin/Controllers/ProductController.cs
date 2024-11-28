using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
            var products = unitOfWork.Product.GetAll().ToList();

            return View(products);
        }

        public IActionResult Upsert(int? id)
        {
            ProductViewModel vm = new ProductViewModel
            {
                CategoryList = unitOfWork.Category.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                Product = new Product()
            };

            if (id == null || id == 0)
            {
                return View(vm);
            }
            else 
            {
                // update
                vm.Product = unitOfWork.Product.Get(_ => _.Id == id);
                return View(vm);
            }

            //return View(vm);
        }


        [HttpPost]
        public IActionResult Upsert(ProductViewModel vm, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.Product.Add(vm.Product);
                unitOfWork.Save();
                TempData["message"] = "Product has been created successfully";
                return RedirectToAction("Index");
            }
            else
            {
                vm.CategoryList = unitOfWork.Category.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
            }

            return View(vm);
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
