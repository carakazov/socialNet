using SocialNetwork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SocialNetwork.DAO;
using System.Data;
using System.Web.UI.WebControls;
using System.Runtime.Remoting.Messaging;
using System.Net.Sockets;
using System.Reflection.Emit;

namespace SocialNetwork.Controllers
{
    public class FriendManagementController : Controller
    {
        private readonly UsersDAO usersDAO = new UsersDAO();
        private readonly RequestsDAO requestsDAO = new RequestsDAO();
        private readonly GroupsDAO groupsDAO = new GroupsDAO();
        private readonly GroupUserDAO groupUserDAO = new GroupUserDAO();
        public ActionResult FriendList(string login)
        {
            Users currentUser = usersDAO.GetByLogin(login);
            Session["session"] = currentUser;
            IEnumerable<Groups> groups = groupsDAO.GetUserGroups(currentUser.ID);
            ViewBag.Groups = new SelectList(groups, "ID", "Titile");
            return View(usersDAO.GetFriends(currentUser));
        }
       
        public string InGroup(int friendID)
        {
            Users currentUser = (Users)Session["session"];
            Groups currentGroup = groupUserDAO.GetHavingGroup(currentUser.ID, friendID);
            return currentGroup.Titile;
        }

        public ActionResult GetDropList()
        {
            Users user = (Users)Session["session"];
            IEnumerable<Groups> groups = groupsDAO.GetUserGroups(user.ID);
            ViewBag.Groups = new SelectList(groups, "ID", "Titile");
            return PartialView("Groups");
        }

        public ActionResult FindFriend()
        {
            return View("FindFriend");
        }

        public ActionResult FindedUsers(string name, string surname, string middleName)
        {
            List<Users> findedUsersList = usersDAO.FindUsers(surname, name, middleName);
            Users currentUser = (Users)Session["session"];
            findedUsersList.RemoveAll(user => user.ID == currentUser.ID);
            return PartialView(findedUsersList);
        }

        public ActionResult AddFriend(int newFriendId)
        {
            int userID = ((Users)Session["session"]).ID; 
            if (requestsDAO.AddRequest(newFriendId, userID))
            {
                return RedirectToAction("Label");
            }
            else
            {
                return View();
            }
        }

        public ActionResult Label()
        {
            return PartialView("Label");
        }

        private bool ValidateTitle(string title)
        {
            bool valid; 
            if(title.Replace(" ", "").Length > 0)
            {
                valid = true; 
            }
            else
            {
                valid = false; 
            }
            return valid;
        }

        [HttpPost]
        public ActionResult CreateGroup(string title, int userID)
        {
            if(ValidateTitle(title) && groupsDAO.AddNewGroup(title, userID))
            {
                Users user = usersDAO.GetByID(userID);
                return RedirectToAction("FriendList", new { login = user.Login });
            }
            else
            {
                Users user = usersDAO.GetByID(userID);
                return RedirectToAction("FriendList", new { login = user.Login });
            }
        }

        public ActionResult ChangeGroup(int friendID, string groupID, int userID)
        {
            if(groupUserDAO.AddUserInGroup(friendID, Convert.ToInt32(groupID), userID))
            {
                Users user = usersDAO.GetByID(userID);
                return RedirectToAction("FriendList", new { login = user.Login });
            }
            else
            {
                return RedirectToAction("LogIn", "Accaunt");
            }
        }
    }
}
