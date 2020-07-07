using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web;
using Antlr.Runtime.Tree;
using Microsoft.SqlServer.Server;
using SocialNetwork.Models;

namespace SocialNetwork.DAO
{
    public class MessagesDAO
    {
        SocialNetDataBaseEntities socialNet = new SocialNetDataBaseEntities();
        public List<string> GetHistory(int userOneID, int userTwoID, bool firstTime)
        {
            List<string> messageHistory = new List<string>();
            try
            {
                UsersDAO usersDAO = new UsersDAO();
                IEnumerable<MessagesList> messageList;
                messageList = (from record in socialNet.MessagesList
                               where (((record.SenderID == userOneID && record.RecepientID == userTwoID)
                               || (record.SenderID == userTwoID && record.RecepientID == userOneID))) || ((record.SenderID == userOneID) && (record.RecepientID == userTwoID))
                               select record);
                if(firstTime)
                {
                    foreach(MessagesList message in messageList)
                    {
                        message.Seen = 1;
                    }
                    socialNet.SaveChanges();
                }
                messageHistory = GetList(messageList, userOneID, userTwoID);
            }
            catch
            {
                string error = @"Unknown error occured :(";
                messageHistory.Add(error);
            }
            return messageHistory;
        }

        private List<string> GetList(IEnumerable<MessagesList> messages, int userOneID, int userTwoID)
        {
            List<string> messagesInStrings = new List<string>();
            UsersDAO usersDAO = new UsersDAO();
            string userOneName = usersDAO.GetByID(userOneID).Name;
            string userTwoName = usersDAO.GetByID(userTwoID).Name;
            foreach (MessagesList record in messages)
            {
                string message;
                if (record.SenderID == userOneID)
                {
                    message = userOneName + @": " + record.Message;
                }
                else
                {
                    message = userTwoName + @": " + record.Message;
                }
                messagesInStrings.Add(message);
            }
            return messagesInStrings;
        }

        public string GetNewMessage(int userID, int interlocutorID)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("");
            UsersDAO usersDAO = new UsersDAO();
            try
            {
                IEnumerable<MessagesList> messages = (from record in socialNet.MessagesList
                                                      where (record.Seen == 0) && (record.RecepientID == userID)
                                                        && (record.SenderID == interlocutorID)
                                                      select record);
                if(messages.Count() > 0)
                {
                    MessagesList message = messages.First();
                    message.Seen = 1;
                    socialNet.SaveChangesAsync();
                    string interlocutorName = usersDAO.GetByID(interlocutorID).Name;
                    builder.Append(interlocutorName).Append(": ").Append(message.Message);
                }
            }
            catch
            {
                builder.Append("Error :(");
            }
            return builder.ToString();
        }

        public bool AddNewMessage(int senderID, int recepientID, string text)
        {
            bool added; 
            try
            {
                if (text.Length != 0)
                {
                    MessagesList message = new MessagesList
                    {
                        SenderID = senderID,
                        RecepientID = recepientID,
                        Message = text,
                        Seen = 0
                    };
                    socialNet.MessagesList.Add(message);
                    socialNet.SaveChanges();
                }
                added = true;
            }
            catch
            {
                added = true;
            }
            return added;
        }
    }
}