using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var categories = unitOfWork.Category.GetAll().ToList();

            return View(categories);
        }

        public IActionResult Create() 
        {
            return View();
        }


        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (category.Name == category.DisplayOrder.ToString())
            { 
                ModelState.AddModelError("Name", "Category Name and Display Order cannot be the same");
            }

            if (ModelState.IsValid)
            {
                unitOfWork.Category.Add(category);
                unitOfWork.Save();
                TempData["message"] = "Category has been created successfully";
                return RedirectToAction("Index");
            }

            return View(category);
        }


        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0) 
            {
                return NotFound();
            }

            Category? categoryFromDb = unitOfWork.Category.Get(_=>_.Id == id);

            if (categoryFromDb == null)
            {
                return NotFound();
            }

            return View(categoryFromDb);
        }


        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (category.Name == category.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "Category Name and Display Order cannot be the same");
            }

            if (ModelState.IsValid)
            {
                unitOfWork.Category.Update(category);
                unitOfWork.Save();
                TempData["message"] = "Category has been updated successfully";
                return RedirectToAction("Index");
            }

            return View(category);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Category? categoryFromDb = unitOfWork.Category.Get(_ => _.Id == id); 

            if (categoryFromDb == null)
            {
                return NotFound();
            }

            return View(categoryFromDb);
        }


        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Category? categoryFromDb = unitOfWork.Category.Get(_ => _.Id == id);

            if (categoryFromDb == null)
            {
                return NotFound();
            }

            unitOfWork.Category.Remove(categoryFromDb);
            unitOfWork.Save();
            TempData["message"] = "Category has been deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
