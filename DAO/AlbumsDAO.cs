using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetwork.Models;

namespace SocialNetwork.DAO
{
    public class AlbumsDAO
    {
        private readonly SocialNetDataBaseEntities socialNet = new SocialNetDataBaseEntities();

        public void CreateAlbum(int creatorID, string title)
        {
            Albums album = new Albums
            { 
                Creator = creatorID, 
                Title = title
            };
            socialNet.Albums.Add(album);
            socialNet.SaveChanges();
        }

        public IEnumerable<Albums> GetAlbums(int userID)
        {
            IEnumerable<Albums> albums = (from record in socialNet.Albums
                                          where record.Creator == userID
                                          select record);
            return albums;
        }

        public Albums GetAvatarsAlbum(int userID)
        {
            Albums avatarAlbum = (from record in socialNet.Albums
                                  where record.Creator == userID && record.Title == "Avatars"
                                  select record).First();
            return avatarAlbum;
        }

        public Albums GetByID(int albumID)
        {
            Albums album = (from record in socialNet.Albums
                            where record.ID == albumID
                            select record).First();
            return album;
        }

        public void PlusOnePhoto(int albumID)
        {
            Albums album = (from record in socialNet.Albums
                            where record.ID == albumID
                            select record).First();
            album.Pictures++;
            socialNet.SaveChanges();
        }

        public void MinusOnePhto(int albumID)
        {
            Albums album = (from record in socialNet.Albums
                            where record.ID == albumID
                            select record).First();
            album.Pictures--;
            socialNet.SaveChanges();
        }
    }
}