using Microsoft.SqlServer.Server;
using SocialNetwork.DAO;
using SocialNetwork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;

namespace SocialNetwork.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UsersDAO usersDAO = new UsersDAO();
        private readonly RequestsDAO requestsDAO = new RequestsDAO();
        private readonly RecordsDAO recordsDAO = new RecordsDAO();
        private readonly ImagesDAO imagesDAO = new ImagesDAO();

        public bool Validate(Users user)
        {
            if(user.Surname.Length == 0)
            {
                ModelState.AddModelError("Surname", "Input new surname");
            }
            if (user.Name.Length == 0)
            {
                ModelState.AddModelError("Surname", "Input new name");
            }
            if (user.Middle_name.Length == 0)
            {
                ModelState.AddModelError("Surname", "Input new middle name");
            }
            return ModelState.IsValid;
        }

        public ActionResult MyProfile(int userID)
        {
            return View("MyProfile",usersDAO.GetByID(userID));
        }

        public ActionResult ShowProfile(int userID)
        {
            FriendListDAO friendListDAO = new FriendListDAO();
            int myID = ((Users)Session["session"]).ID;
            Users user = usersDAO.GetByID(userID);
            ViewData["ownerID"] = user;
            ViewData["areFriends"] = friendListDAO.AreFriends(myID, userID);
            return View("UserProfile", user);
        }

        public ActionResult EditProfile(int userID)
        {
            return View(usersDAO.GetByID(userID));
        }

        [HttpPost]
        public ActionResult EditProfile(Users user)
        {
            if (Validate(user) && usersDAO.EditUser(user))
            {
                return RedirectToAction("MyProfile", new { userID = user.ID });
            }
            else
            {
                return View();
            }
        }

        public ActionResult ShowRequests(int userID)
        {
            return PartialView("RequestsList", usersDAO.GetRequests(userID));
        }

        public ActionResult ChangeAvatar(HttpPostedFileBase avatar, int userID)
        {
            imagesDAO.AddNewAvatar(avatar, userID);
            int id = ((Users)Session["session"]).ID;
            return RedirectToAction("MyProfile", new { userID = id });
        }

        public ActionResult ShowRecords(int userID)
        {
            ViewData["records"] = recordsDAO.GetRecordList(userID);
            return PartialView("RecordsList");
        }

        public ActionResult WriteRecordOnMyWall(string text, int userID)
        {
            if(recordsDAO.WriteRecord(text, userID, userID))
            {
                return RedirectToAction("ShowRecords", new { userID });
            }
            else
            {
                return View();
            }
        }

        public ActionResult WriteRecordOnAlienWall(string text, int authorID, int ownerID)
        {
            if(recordsDAO.WriteRecord(text, authorID, ownerID))
            {
                return RedirectToAction("ShowRecords", new { userID = ownerID });
            }
            else
            {
                return View();
            }
        }

        public ActionResult Accept(int recepientID, int senderID)
        {
            if (requestsDAO.RequestAccepted(recepientID, senderID))
            {
                return PartialView("Accepted");
            }
            else
            {
                return View("MyProfile", new { userID = recepientID });
            }
        }

        public ActionResult Decline(int recepientID, int senderID)
        {
            if(requestsDAO.RequestDeclined(recepientID, senderID))
            {
                return PartialView("Declined");
            }
            else
            {
                return View("MyProfile", new { userID = recepientID });
            }
        }
    }
}
