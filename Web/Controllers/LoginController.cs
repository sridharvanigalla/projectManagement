using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic;
using BusinessObject;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class LoginController : Controller
    {
        UserBL userBL = new UserBL();

        /// <summary>
        /// Login View Return
        /// </summary>
        /// <returns></returns>
        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// User Login Post
        /// </summary>
        /// <param name="cust"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult UserLogin([Bind] LoginObject cust)
        {
            if (ModelState.IsValid)
            {
                if (userBL.LoginUser(cust))
                {
                    return Redirect("/Category/CategoryDetails");
                }
                else
                {
                    ViewData["InvalidLogin"] = "Invalid User Name or Password";
                    return View("Login");
                }
            }
            else
            {
                return View("Login");
            }
        }
    }
}