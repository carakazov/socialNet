using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Models;

namespace SocialNetwork.DAO
{
    public class RecordsDAO
    {
        private SocialNetDataBaseEntities socialNet = new SocialNetDataBaseEntities();
        public List<string> GetRecordList(int ownerID)
        {
            UsersDAO usersDAO = new UsersDAO();
            StringBuilder builder = new StringBuilder();
            List<string> records = new List<string>();
            IEnumerable<Records> recordsList = (from record in socialNet.Records
                                                where record.Owner == ownerID
                                                select record);
            foreach(Records record in recordsList)
            {
                builder.Clear();
                string writerName = usersDAO.GetByID(record.Author).Name;
                builder.Append(writerName);
                builder.Append(": ");
                builder.Append(record.Text);
                records.Add(builder.ToString());
            }
            return records;
        }

        public bool WriteRecord(string text, int authorID, int ownerID)
        {
            bool done; 
            try
            {
                Records record = new Records
                {
                    Text = text,
                    Author = authorID,
                    Owner = ownerID
                };
                socialNet.Records.Add(record);
                socialNet.SaveChanges();
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