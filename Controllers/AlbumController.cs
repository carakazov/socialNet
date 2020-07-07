using SocialNetwork.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialNetwork.Controllers
{
    public class AlbumController : Controller
    {
        private readonly AlbumsDAO albumsDAO = new AlbumsDAO();
        private readonly ImagesDAO imagesDAO = new ImagesDAO();

        public ActionResult ShowAlbums(int userID)
        {
            return View("AlbumsList",albumsDAO.GetAlbums(userID));
        }

        [HttpPost]
        public ActionResult CreateAlbum(int creatorID, string title)
        {
            albumsDAO.CreateAlbum(creatorID, title);
            return RedirectToAction("ShowAlbums", new { userID = creatorID });
        }

        public ActionResult GetImages(int albumID)
        {
            ViewData["albumID"] = albumID;
            return View("Photos", imagesDAO.GetAllImages(albumID));
        }

        [HttpPost]
        public ActionResult AddNewImage(HttpPostedFileBase newImage, int albumID)
        {
            imagesDAO.AddImage(newImage, albumID);
            return RedirectToAction("GetImages", new { albumID });
        }
        
        public ActionResult DeletePhoto(int imageID, int albumID)
        {
            imagesDAO.DeleteImage(imageID, albumID);
            return RedirectToAction("GetImages", new { albumID });
        }

        public ActionResult ChangeAvatar(int imageID, int userID, int albumID)
        {
            imagesDAO.ChangeAvatar(imageID, userID, albumID);
            return RedirectToAction("MyProfile", "Profile", new { userID });
        }

        public ActionResult ShowUsersAlbums(int userID)
        {
            ViewData["userID"] = userID;
            return View("UsersAlbums", albumsDAO.GetAlbums(userID));
        }

        public ActionResult UsersImages(int albumID, int userID)
        {
            ViewData["userID"] = userID;
            return View("UsersPhotos", imagesDAO.GetAllImages(albumID));
        }
    }
}
