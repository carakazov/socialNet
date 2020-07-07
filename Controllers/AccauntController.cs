using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using SocialNetwork.DAO;
using SocialNetwork.Models;
using WebGrease.Css.Ast.Selectors;
using System.Text.RegularExpressions;
using System.Web.WebSockets;

namespace SocialNetwork.Controllers
{
    public class AccauntController : Controller
    {
        private readonly UsersDAO usersDAO = new UsersDAO();
        private readonly AlbumsDAO albumsDAO = new AlbumsDAO();
        [HttpPost]
        public ActionResult LogIn(string login, string password)
        {
            Users user = new Users();
            user.Login = user.Hash(login);
            user.Password = user.Hash(password);
            if (usersDAO.DoExists(user.Login, user.Password))
            {
                return RedirectToAction("FriendList", "FriendManagement", new { login = user.Login });
            }
            else
            {
                return View();
            }
        }

        public ActionResult LogIn()
        {
            return View("Login");
        }

        public ActionResult LogOut()
        {
            Users user = (Users)Session["session"];
            usersDAO.OnlineChange(user.ID);
            Session["session"] = null;
            return RedirectToAction("LogIn");
        }

        public bool ValidateLogin(string password, Users user)
        {
            SocialNetDataBaseEntities socialNet = new SocialNetDataBaseEntities();
            int loginCount = (from record in socialNet.Users
                              where record.Login == user.Login
                              select record.Login).Count();
            if (loginCount > 0)
            {
                ModelState.AddModelError("Login", "Such login already exists");
            }
            if (password.Length < 5 || password.Length > 15)
            {
                ModelState.AddModelError("Password", "Password should be between 5 and 15 included");
            }
            return ModelState.IsValid;
        }

        public ActionResult Registrate()
        {
            return View("Registrate");
        }

        [HttpPost]
        public ActionResult Registrate(string login, string password, Users user)
        {
            user.Login = user.Hash(login);
            user.Password = user.Hash(password);
            if(ValidateLogin(password, user) && usersDAO.Create(user))
            { 
                albumsDAO.CreateAlbum(user.ID, "Avatars");
                return RedirectToAction("LogIn", new { login, password });
            }
            else
            {
                return View();
            }
        }
    }
}
