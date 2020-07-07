using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.DynamicData;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using SocialNetwork.Models;
using System.Linq.Expressions;
using System.Linq.Dynamic;
using System.Text;
using System.Data.SqlClient;
using System.Xml.Linq;
using System.Data.Entity.Migrations;

namespace SocialNetwork.DAO
{
    public class UsersDAO
    {
        private readonly SocialNetDataBaseEntities socialNet = new SocialNetDataBaseEntities();
       
        public Users GetByID(int userID)
        {
            Users user = (from record in socialNet.Users
                          where record.ID == userID
                          select record).First();
            return user;
        }
        
        public void SetNewAvatar(int userID, int avatarID)
        {
            Users user = (from record in socialNet.Users
                          where record.ID == userID
                          select record).First();
            user.Avatar = avatarID;
            socialNet.SaveChanges();
        }

        public void ChangeAvatar(int userID, int imageID)
        {
            Users user = GetByID(userID);
            user.Avatar = imageID;
            socialNet.SaveChanges();
        }

        public void OnlineChange(int userID)
        {
            Users user = GetByID(userID);
            if (user.Online == 1)
            {
                user.Online = 0;
            }
            else
            {
                user.Online = 1;
            }
            socialNet.SaveChanges();
        }

        public bool Create(Users user)
        {
            bool created;
            try
            {
                user.ChangeOnline();
                socialNet.Users.Add(user);
                socialNet.SaveChanges();
                created = true;
            }
            catch
            {
                created = false;
            }
            return created;
        }

        public bool DoExists(string hashLogin, string hashPassword)
        {
            bool doExists;
            int user = (from record in socialNet.Users
                        where record.Login == hashLogin & record.Password == hashPassword
                        select record).Count();
            if (user > 0)
            {
                doExists = true;
            }
            else
            {
                doExists = false;
            }
            return doExists;
        }

        public List<Users> GetRequests(int userID)
        {
            List<Users> users = new List<Users>();
            Users user = GetByID(userID);
            foreach(RequestsList request in user.RequestsList)
            {
                Users oneUser = (from record in socialNet.Users
                              where record.ID == request.SenderID
                              select record).First();
                users.Add(oneUser);
            }
            return users;
        }

        public Users GetByLogin(string hashLogin)
        {
            Users user = new Users();
            user = (from record in socialNet.Users
                    where record.Login == hashLogin
                    select record).First();
            return user;
        }

        public List<Users> GetFriends(Users user)
        {
            List<Users> usersList = new List<Users>();
            List<FriendList> friendList = user.FriendList.ToList();
            foreach (FriendList oneFriend in friendList)
            {
                Users users = (from record in socialNet.Users
                               where record.ID == oneFriend.FriendID
                               select record).First();
                usersList.Add(users);
            }
            return usersList;
        }

        public bool EditUser(Users user)
        {
            bool edited;
            try 
            {
                Users previosUser = (from record in socialNet.Users
                                     where record.ID == user.ID
                                     select record).First();
                previosUser.Surname = user.Surname;
                previosUser.Name = user.Name;
                previosUser.Middle_name = user.Middle_name;
                socialNet.SaveChanges();
                edited = true;
            }
            catch
            {
                edited = false;
            }
            return edited;
        }

        public List<Users> FindUsers(string surname, string name, string middleName)
        {
            List<Users> users = new List<Users>();
            if (surname.Length == 0 && name.Length == 0 && middleName.Length == 0)
            {
                return users;
            }
            else
            {
                if (middleName.Length == 0)
                {
                    users = (from record in socialNet.Users
                             select record).ToList();
                }
                else
                {
                    users = (from record in socialNet.Users
                             where record.Middle_name == middleName
                             select record).ToList();
                }
                if (surname.Length != 0)
                {
                    users = users.Where(user => user.Surname == surname).ToList();
                }
                if (name.Length != 0)
                {
                    users = users.Where(user => user.Name == name).ToList();
                }
            }
            return users;
        }
    }
}