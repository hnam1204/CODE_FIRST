using System;
using System.ComponentModel.DataAnnotations;

namespace HV_NIX.Models
{
    public class OrderHistory
    {
        [Key]
        public int OrderHistoryID { get; set; }
        public int OrderID { get; set; }
        public string Status { get; set; }
        public DateTime UpdatedAt { get; set; }

        public virtual Orders Order { get; set; }
    }
}
