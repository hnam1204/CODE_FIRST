using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HV_NIX.Models
{
    public class Users
    {
        [Key]
        public int UserID { get; set; }

        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; }

        // One-to-One
        public virtual UserInfo UserInfo { get; set; }
    }
}
