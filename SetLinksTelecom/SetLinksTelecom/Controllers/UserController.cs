using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SetLinksTelecom.Data;
using SetLinksTelecom.DTO;
using SetLinksTelecom.Models;

namespace SetLinksTelecom.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepo _repo;

        public UserController(IUserRepo repo)
        {
            _repo = repo;
        }

        // GET: User
        public ActionResult Login()
        {
            DtoUserLogin user = new DtoUserLogin();
            //user.ErrorMessage = "Error";
            return View(user);
        }

        [HttpPost]
        public ActionResult Login(DtoUserLogin userModel)
        {
            userModel.ErrorMessage = String.Empty;
            DtoUserLogin user = _repo.Login(userModel);
            if (!string.IsNullOrWhiteSpace(user.ErrorMessage))
            {
                return View(userModel);
            }

            Session["userName"] = user.Username;
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Login", "User");
        }
    }
}