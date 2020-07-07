using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetwork.Models;
using SocialNetwork.DAO;
using System.IO;
using System.Drawing;
using System.Web.Razor.Text;
using System.Linq.Dynamic;
using System.Runtime.CompilerServices;

namespace SocialNetwork.DAO
{
    public class ImagesDAO
    {
        private readonly SocialNetDataBaseEntities socialNet = new SocialNetDataBaseEntities();
        private readonly ImageAlbumDAO imageAlbumDAO = new ImageAlbumDAO();
        private readonly AlbumsDAO albumsDAO = new AlbumsDAO();
        private readonly UsersDAO usersDAO = new UsersDAO();
        public Images GetImage(int? imageID)
        {
            Images image = (from record in socialNet.Images
                            where record.ID == imageID
                            select record).First();
            return image;
        }

        public void AddNewAvatar(HttpPostedFileBase image, int userID)
        {
            byte[] data = GetBytes(image);
            Albums album = (from record in socialNet.Albums
                      where record.Creator == userID && record.Title == "Avatars"
                      select record).First();
            int albumID = album.ID;
            albumsDAO.PlusOnePhoto(albumID);
            Images newImage = new Images()
            {
                Image = data
            };
            socialNet.Images.Add(newImage);
            socialNet.SaveChanges();
            int imageID = GetImageID(newImage);
            imageAlbumDAO.AddRecord(imageID, albumID);
            socialNet.SaveChanges();
            usersDAO.ChangeAvatar(userID, imageID);
        }

        public List<Images> GetAllImages(int albumID)
        {
            List<Images> images = new List<Images>();
            IEnumerable<int> photosID = imageAlbumDAO.GetPhotosID(albumID);
            foreach(int id in photosID)
            {
                Images image = GetImage(id);
                images.Add(image);
            }
            return images;
        }

        public void AddImage(HttpPostedFileBase image, int albumID)
        {
            byte[] data = GetBytes(image);
            Images newImage = new Images
            {
                Image = data
            };
            socialNet.Images.Add(newImage);
            socialNet.SaveChanges();
            int imageID = GetImageID(newImage);
            imageAlbumDAO.AddRecord(imageID, albumID);
            albumsDAO.PlusOnePhoto(albumID);
        }

        private int GetImageID(Images image)
        {
            int imageID = (from record in socialNet.Images
                           where record.ID == image.ID
                           select record.ID).First();
            return imageID;
        }

        private byte[] GetBytes(HttpPostedFileBase image)
        {
            Stream stream = image.InputStream;
            BinaryReader reader = new BinaryReader(stream);
            byte[] data = reader.ReadBytes(image.ContentLength);
            return data;
        }

        public void DeleteImage(int imageID, int albumID)
        {
            imageAlbumDAO.DeleteRecord(imageID, albumID);
            albumsDAO.MinusOnePhto(albumID);
            Images image = GetImage(imageID);
            socialNet.Images.Remove(image);
            socialNet.SaveChanges();
        }

        public void ChangeAvatar(int imageID, int userID, int albumID)
        {
            Albums avatarsAlbum = albumsDAO.GetAvatarsAlbum(userID);
            imageAlbumDAO.DeleteRecord(imageID, albumID);
            imageAlbumDAO.AddRecord(imageID, avatarsAlbum.ID);
            usersDAO.SetNewAvatar(userID, imageID);
        }
    }
}