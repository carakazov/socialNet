using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.UI.HtmlControls;
using SocialNetwork.DAO;
using SocialNetwork.Models;

namespace SocialNetwork.DAO
{
    public class ImageAlbumDAO
    {
        private readonly SocialNetDataBaseEntities socialNet = new SocialNetDataBaseEntities();
        private readonly AlbumsDAO albumsDAO = new AlbumsDAO();
        public void AddRecord(int imageID, int albumID)
        {
            ImageAlbum imageAlbum = new ImageAlbum
            { 
                IDAlbum = albumID, 
                IDImage = imageID
            };
            socialNet.ImageAlbum.Add(imageAlbum);
            socialNet.SaveChanges();
        }

        public void CountPhotos(int albumID)
        {
            Albums album = albumsDAO.GetByID(albumID);
            int count = (from record in socialNet.ImageAlbum
                         where record.IDAlbum == albumID
                         select record).Count();
            album.Pictures = count;
            socialNet.SaveChanges();
        }

        public IEnumerable<int> GetPhotosID(int albumID)
        {
            IEnumerable<int> photosID = (from record in socialNet.ImageAlbum
                                         where record.IDAlbum == albumID
                                         select record.IDImage);
            return photosID;
        }

        public void DeleteRecord(int imageID, int albumID)
        {
            ImageAlbum imageAlbum = (from record in socialNet.ImageAlbum
                                     where record.IDAlbum == albumID && record.IDImage == imageID
                                     select record).First();
            socialNet.ImageAlbum.Remove(imageAlbum);
            socialNet.SaveChanges();
        }
    }
}