﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SocialNetwork.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class SocialNetDataBaseEntities : DbContext
    {
        public SocialNetDataBaseEntities()
            : base("name=SocialNetDataBaseEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Albums> Albums { get; set; }
        public virtual DbSet<FriendList> FriendList { get; set; }
        public virtual DbSet<Groups> Groups { get; set; }
        public virtual DbSet<GroupUser> GroupUser { get; set; }
        public virtual DbSet<ImageAlbum> ImageAlbum { get; set; }
        public virtual DbSet<Images> Images { get; set; }
        public virtual DbSet<MessagesList> MessagesList { get; set; }
        public virtual DbSet<Records> Records { get; set; }
        public virtual DbSet<RequestsList> RequestsList { get; set; }
        public virtual DbSet<Users> Users { get; set; }
    }
}
