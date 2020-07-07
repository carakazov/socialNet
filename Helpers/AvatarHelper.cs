using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SocialNetwork.Models;
using SocialNetwork.DAO;
using System.Web.UI.WebControls;

namespace SocialNetwork.Helpers
{
    public static class AvatarHelper
    {
        public static HtmlString DrowAvatar(this HtmlHelper helper,  int userID)
        {
            UsersDAO usersDAO = new UsersDAO();
            ImagesDAO imagesDAO = new ImagesDAO();
            Users user = usersDAO.GetByID(userID);
            if(user.Avatar != null)
            {
                Images image = imagesDAO.GetImage(user.Avatar);
                TagBuilder img = new TagBuilder("img");
                string attribute = "data:image;base64," + Convert.ToBase64String(image.Image);
                string height = "300px";
                string width = "300px";
                img.Attributes.Add("src", attribute);
                img.Attributes.Add("height", height);
                img.Attributes.Add("width", width);
                return new HtmlString(img.ToString());
            }
            else
            {
                TagBuilder label = new TagBuilder("label");
                label.SetInnerText("No avatar(");
                return new HtmlString(label.ToString());
            }
        }
    }
}