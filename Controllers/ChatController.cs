using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SocialNetwork.Models;
using SocialNetwork.DAO;
using System.Runtime.CompilerServices;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Web.Services.Protocols;
using System.Web.Services.Description;
using System.Threading;
using System.ComponentModel;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.IO;

namespace SocialNetwork.Controllers
{
    public class ChatController : Controller
    {
        private readonly UsersDAO usersDAO = new UsersDAO();
        private readonly MessagesDAO messagesDAO = new MessagesDAO();

        public ActionResult ToChat(int interlocutorID)
        {
            ViewData["interlocutor"] = usersDAO.GetByID(interlocutorID);
            return View("Chat");
        }

        public ActionResult GetList(int userID, int interlocutorID, bool firstTime)
        {
            ViewData["history"] = messagesDAO.GetHistory(userID, interlocutorID, firstTime);
            return PartialView("MessagesList");
        }

        public ActionResult SendMessage(int userID, int interlocutorID, string messageText)
        {
            if(messagesDAO.AddNewMessage(userID, interlocutorID, messageText))
            {
                return RedirectToAction("GetList", new { userID, interlocutorID, firstTime = false });
            }
            else
            {
                ViewData["interlocutor"] = usersDAO.GetByID(interlocutorID);
                return View("Chat");
            }
        }

        public string ShowNewMessage(int userID, int interlocutorID)
        {
            return messagesDAO.GetNewMessage(userID, interlocutorID);
        }
    }
}
