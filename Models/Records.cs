//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class Records
    {
        public int ID { get; set; }
        public string Text { get; set; }
        public int Author { get; set; }
        public int Owner { get; set; }
    
        public virtual Users Authors { get; set; }
        public virtual Users Owners { get; set; }
    }
}
