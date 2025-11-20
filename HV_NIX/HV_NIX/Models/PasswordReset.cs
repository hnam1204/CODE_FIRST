using System;
using System.ComponentModel.DataAnnotations;

namespace HV_NIX.Models
{
    public class PasswordReset
    {
        [Key]
        public int PasswordResetID { get; set; }
        public int UserID { get; set; }
        public string Token { get; set; }
        public DateTime ExpiredAt { get; set; }

        public virtual Users User { get; set; }
    }
}
