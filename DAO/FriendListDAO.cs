using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetwork.Models;

namespace SocialNetwork.DAO
{
    public class FriendListDAO
    {
        private readonly SocialNetDataBaseEntities socialNet = new SocialNetDataBaseEntities();
        public IEnumerable<FriendList> GetAllFriends(Users user)
        {
            IEnumerable<FriendList> friendList = (from record in socialNet.FriendList
                                              where record.UserID == user.ID
                                              select record);
            return friendList;
        }

        public bool AreFriends(int userID, int friendID)
        {
            bool areFriends;
            int number = (from record in socialNet.FriendList
                          where record.FriendID == friendID && record.UserID == userID
                          select record).Count();
            if(number > 0)
            {
                areFriends = true;
            }
            else
            {
                areFriends = false;
            }
            return areFriends;
        }
    }
}