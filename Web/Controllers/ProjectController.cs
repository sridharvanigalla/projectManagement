using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic;
using BusinessObject;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class ProjectController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;
        ProjectBL projectBL;
        CategoryBL categoryBL;

        /// <summary>
        /// Project Controller Constructor
        /// </summary>
        /// <param name="environment"></param>
        public ProjectController(IHostingEnvironment environment)
        {
            _hostingEnvironment = environment;
            projectBL = new ProjectBL(_hostingEnvironment);
            categoryBL = new CategoryBL(_hostingEnvironment);
        }

        /// <summary>
        /// Get All Project Details
        /// </summary>
        /// <returns></returns>
        public IActionResult ProjectDetails()
        {
            IEnumerable<ProjectObject> listProjectObject;
            listProjectObject = projectBL.GetAllProjects();
            return View(listProjectObject);
        }

        /// <summary>
        /// Create New Project View
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            ViewBag.listCategoryObject = categoryBL.GetAllCategory();
            return View();
        }

        /// <summary>
        /// Create Project Api
        /// </summary>
        /// <param name="projectObject"></param>
        /// <returns></returns>
        public IActionResult CreateProject([Bind] ProjectObject projectObject)
        {
            int result = 0;
            if (ModelState.IsValid)
            {
                result = projectBL.SaveUpdateProject(projectObject);
                if (result > 0)
                {
                    return RedirectToAction("ProjectDetails");
                }
                else
                {
                    ViewBag.listCategoryObject = categoryBL.GetAllCategory(); 
                    return RedirectToAction("Create");
                }
            }
            else
            {
                ViewBag.listCategoryObject = categoryBL.GetAllCategory();
                return View("Create");
            }
        }

        /// <summary>
        /// Edit Api
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public IActionResult EditProject(int projectId)
        {
            ProjectObject projectObject = new ProjectObject();
            projectObject = projectBL.ProjectEdit(projectId);
            if (projectObject != null)
            {
                ViewBag.listCategoryObject = categoryBL.GetAllCategory();
                return View(projectObject);
            }
            else
            {
                return RedirectToAction("ProjectDetails");
            }
        }

        /// <summary>
        /// Delete Project
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public IActionResult DeleteProject(int projectId)
        {
            projectBL.DeleteProject(projectId);
            return RedirectToAction("ProjectDetails");
        }
    }
}