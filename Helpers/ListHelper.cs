using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using SocialNetwork.DAO;
using SocialNetwork.Models;

namespace SocialNetwork.Helpers
{
    public static class ListHelper
    {
        public static HtmlString GetList(this HtmlHelper helper, int userID)
        {
            GroupUserDAO groupUserDAO = new GroupUserDAO();
            GroupsDAO groupsDAO = new GroupsDAO();
            StringBuilder builder = new StringBuilder();
            string linkText = @"https://localhost:44373/Chat/ToChat?interlocutorID=param";
            TagBuilder div = new TagBuilder("div");
            List<Groups> groups = groupsDAO.GetUserGroups(userID).ToList();
            foreach(Groups group in groups)
            {
                List<Users>friendList = groupUserDAO.GetFriendsInGroup(group.ID);
                if (friendList.Count() > 0)
                {
                    TagBuilder ul = new TagBuilder("ul");
                    TagBuilder label = new TagBuilder("label");
                    string groupName = group.Titile + ", " + groupUserDAO.CountMembers(group.ID).ToString() + " total:";
                    label.SetInnerText(groupName);
                    div.InnerHtml += label;
                    foreach (Users user in friendList)
                    { 
                        TagBuilder li = new TagBuilder("li");
                        TagBuilder a = new TagBuilder("a");
                        builder.Clear();
                        builder.Append(linkText);
                        builder.Replace("param", user.ID.ToString());
                        a.Attributes.Add("href", builder.ToString());
                        string friendName = user.Name + " " + user.Surname;
                        a.SetInnerText(friendName);
                        li.InnerHtml = a.ToString();
                        ul.InnerHtml += li;
                    }
                    div.InnerHtml += ul;
                }
                else
                {
                    TagBuilder label = new TagBuilder("label");
                    string messageText = group.Titile + " has no members";
                    label.SetInnerText(messageText);
                    TagBuilder br = new TagBuilder("br");
                    div.InnerHtml += label;
                    div.InnerHtml += br;
                }
            }
            return new HtmlString(div.ToString());
        }
    }
}