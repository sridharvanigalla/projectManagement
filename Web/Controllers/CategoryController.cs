using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using BusinessLogic;
using BusinessObject;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class CategoryController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;
        CategoryBL categoryBL;

        /// <summary>
        /// Category Controller
        /// </summary>
        /// <param name="environment"></param>
        public CategoryController(IHostingEnvironment environment)
        {
            _hostingEnvironment = environment;
            categoryBL = new CategoryBL(_hostingEnvironment);
        }

        /// <summary>
        /// Get Category Details
        /// </summary>
        /// <returns></returns>
        public IActionResult CategoryDetails()
        {
            IEnumerable<CategoryObject> listCategoryObject;
            listCategoryObject=categoryBL.GetAllCategory();
            return View(listCategoryObject);
        }

        /// <summary>
        /// Return Create Category Page
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Edit Category
        /// </summary>
        /// <param name="CategoryId"></param>
        /// <returns></returns>
        public IActionResult EditCategory(int CategoryId)
        {
            CategoryObject categoryObject = new CategoryObject();
            categoryObject = categoryBL.CategoryEdit(CategoryId);
            if (categoryObject != null)
            {
                return View(categoryObject);
            }
            else
            {
                return RedirectToAction("CategoryDetails");
            }
        }

        /// <summary>
        /// Delete Category
        /// </summary>
        /// <param name="CategoryId"></param>
        /// <returns></returns>
        public IActionResult DeleteCategory(int CategoryId)
        {
            categoryBL.DeleteCategory(CategoryId);
            return RedirectToAction("CategoryDetails");
        }

        /// <summary>
        /// Create Category Post
        /// </summary>
        /// <param name="categoryObject"></param>
        /// <returns></returns>
        public IActionResult CreateCategory([Bind] CategoryObject categoryObject)
        {
            int result = 0;
            if (ModelState.IsValid)
            {
                result =categoryBL.SaveUpdateCategory(categoryObject);
                if(result>0)
                {
                    return RedirectToAction("CategoryDetails");
                }
                else
                {
                    ModelState.AddModelError("CategoryName", "Already Exist");
                    return RedirectToAction("Create");
                }
            }
            else
            {
                ViewBag.Issue = false;
                return View("Create");
            }
        }
    }
}