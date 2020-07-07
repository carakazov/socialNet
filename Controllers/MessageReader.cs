using SocialNetwork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using System.Threading;
using System.Threading.Tasks;

namespace SocialNetwork.Controllers
{
    public class MessageReader
    {
        private int userID;
        private int interlocutorID;

        public bool IsDone { get; set; }

        public MessageReader(int userID, int interlocutorID)
        {
            this.userID = userID;
            this.interlocutorID = interlocutorID;
        }

        public void GetNewMessages()
        {
            SocialNetDataBaseEntities socialNet = new SocialNetDataBaseEntities();
            ChatController chat = new ChatController();
            while (true)
            {
                
                IEnumerable<MessagesList> messages = (from record in socialNet.MessagesList
                                                      where record.Seen == 0 && record.RecepientID == userID
                                                      && record.SenderID == interlocutorID
                                                      select record);
                if (messages.Count() > 0)
                {
                    foreach (MessagesList message in messages)
                    {
                        message.Seen = 1;
                    }
                    socialNet.SaveChanges();
                    IsDone = true;
                }
            }
        }
    }
}