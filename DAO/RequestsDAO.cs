using SocialNetwork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialNetwork.DAO
{
    public class RequestsDAO
    {
        private readonly SocialNetDataBaseEntities socialNet = new SocialNetDataBaseEntities();

        public bool AddRequest(int recepientID, int senderID)
        {
            bool requestAdded;
            try
            {
                RequestsList request = new RequestsList
                {
                    RecepientID = recepientID,
                    SenderID = senderID
                };
                socialNet.RequestsList.Add(request);
                socialNet.SaveChanges();
                requestAdded = true;
            }
            catch
            {
                requestAdded = false;
            }
            return requestAdded;
        }

        public bool RequestAccepted(int recepientID, int senderID)
        {
            bool accepted;
            try
            {
                UsersDAO usersDAO = new UsersDAO();
                RequestsList request = (from record in socialNet.RequestsList
                                        where record.RecepientID == recepientID && record.SenderID == senderID
                                        select record).First();
                socialNet.RequestsList.Remove(request);
                int newID = (from record in socialNet.FriendList
                             select record.ID).Max();
                FriendList firstRecord = new FriendList
                {
                    UserID = recepientID,
                    FriendID = senderID,
                };
                FriendList secondRecord = new FriendList
                {
                    UserID = senderID,             
                    FriendID = recepientID,
                };
                socialNet.FriendList.Add(firstRecord);
                socialNet.FriendList.Add(secondRecord);
                socialNet.SaveChanges();
                accepted = true; 
            }
            catch
            {
                accepted = false;
            }
            return accepted;
        }

        public bool RequestDeclined(int recepientID, int senderID)
        {
            bool declined;
            try
            {
                RequestsList request = (from record in socialNet.RequestsList
                                        where record.RecepientID == recepientID && record.SenderID == senderID
                                        select record).First();
                socialNet.RequestsList.Remove(request);
                socialNet.SaveChanges();
                declined = true;
            }
            catch
            {
                declined = false; 
            }
            return declined; 
        }
    }
}