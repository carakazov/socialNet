using Microsoft.AspNetCore.Builder;
using SocialNetwork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialNetwork.DAO
{
    public class GroupsDAO
    {
        private SocialNetDataBaseEntities socialNet = new SocialNetDataBaseEntities();

        public IEnumerable<Groups> GetUserGroups(int userID)
        {
            IEnumerable<Groups> groups = (from record in socialNet.Groups
                                          where record.Creator == userID
                                          select record);
            return groups;
        }

        public bool AddNewGroup(string groupTitle, int userID)
        {
            bool added; 
            try
            {
                Groups group = new Groups
                { 
                     Titile = groupTitle,
                     Creator = userID
                };

                socialNet.Groups.Add(group);
                socialNet.SaveChanges();
                added = true;
            }
            catch
            {
                added = false;
            }
            return added;
        }

        public Groups GetByID(int groupID)
        {
            Groups group = (from record in socialNet.Groups
                            where record.ID == groupID
                            select record).First();
            return group;
        }
    }
}