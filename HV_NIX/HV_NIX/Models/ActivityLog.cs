using System;
using System.ComponentModel.DataAnnotations;

namespace HV_NIX.Models
{
    public class ActivityLog
    {
        [Key]                     // 🔥 BẮT BUỘC CÓ
        public int LogID { get; set; }

        public int UserID { get; set; }

        [Required]
        public string Action { get; set; }

        public DateTime Date { get; set; }

        // FK
        public virtual Users User { get; set; }
    }
}
