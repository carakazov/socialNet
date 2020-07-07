using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SocialNetwork.Models;

namespace SocialNetwork.Helpers
{
    public static class PictureHelper
    {
        public static HtmlString DrowPicture(this HtmlHelper html, Images image)
        {
            TagBuilder img = new TagBuilder("img");
            string attribute = "data:image;base64," + Convert.ToBase64String(image.Image);
            string height = "500px";
            string width = "500px";
            img.Attributes.Add("src", attribute);
            img.Attributes.Add("height", height);
            img.Attributes.Add("width", width);
            return new HtmlString(img.ToString());
        }
    }
}