using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using SocialNetwork.Models;

namespace SocialNetwork.DAO
{
    public class GroupUserDAO
    {
        private SocialNetDataBaseEntities socialNet = new SocialNetDataBaseEntities();
        private GroupsDAO groupsDAO = new GroupsDAO();
        public IEnumerable<GroupUser> GetUserIncludedGroups(int userID)
        {
            IEnumerable<GroupUser> groupUsers = (from record in socialNet.GroupUser
                                                 where record.IDUser == userID
                                                 select record);
            return groupUsers;
        }

        public List<Users> GetFriendsInGroup(int groupID)
        {
            List<Users> friendList = new List<Users>();
            UsersDAO usersDAO = new UsersDAO();
            List<int> usersID = (from record in socialNet.GroupUser
                                        where record.IDGroup == groupID
                                        select record.IDUser).ToList();
            foreach(int id in usersID)
            {
                Users user = usersDAO.GetByID(id);
                friendList.Add(user);
            }
            return friendList;
        }

        public int CountMembers(int groupID)
        {
            int number = (from record in socialNet.GroupUser
                          where record.IDGroup == groupID
                          select record).Count();
            return number; 
        }

        public Groups GetHavingGroup(int userID, int interlocutorID)
        {
            IEnumerable<Groups> groups = groupsDAO.GetUserGroups(userID);
            IEnumerable<GroupUser> groupUsers = GetUserIncludedGroups(interlocutorID);
            IEnumerable<int> groupsID = groups.Select(record => record.ID);
            IEnumerable<int> groupUsersID = groupUsers.Select(record => record.IDGroup);
            IEnumerable<int> needed = groupsID.Intersect(groupUsersID);
            Groups group = new Groups
            {
                Titile = "None"
            };
            if(needed.Count() > 0)
            {
                group = groupsDAO.GetByID(needed.First());
            }
            return group;
        }

        public GroupUser GetRecord(int userID, int groupID)
        {
            GroupUser groupUser = (from record in socialNet.GroupUser
                                   where (record.IDGroup == groupID) && (record.IDUser == userID)
                                   select record).First();
            return groupUser;
        }

        public bool AddUserInGroup(int friendID, int groupID, int userID)
        {
            bool done; 
            try
            {
                UsersDAO usersDAO = new UsersDAO();
                Users user = usersDAO.GetByID(friendID);
                IEnumerable<Groups> groups = groupsDAO.GetUserGroups(userID);
                List<int> groupsID = groups.Select(record => record.ID).ToList();
                List<int> groupUserID = user.GroupUser.Select(record => record.IDGroup).ToList();
                List<int> result = groupsID.Intersect(groupUserID).ToList();
                if(result.Count() > 0)
                {
                    GroupUser groupUser = GetRecord(friendID, result.First());
                    groupUser.IDGroup = groupID;
                    socialNet.SaveChanges();
                }
                else
                {
                    GroupUser groupUser = new GroupUser
                    { 
                        IDGroup = groupID, 
                        IDUser = friendID
                    };
                    socialNet.GroupUser.Add(groupUser);
                    socialNet.SaveChanges();
                }
                done = true;
            }
            catch
            {
                done = false;
            }
            return done;
        }
    }
}