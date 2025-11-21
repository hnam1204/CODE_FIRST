using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HV_NIX.Models
{
    public class Reviews
    {
        [Key]
        public int ReviewID { get; set; }

        public int ProductID { get; set; }
        public int UserID { get; set; }
        public int OrderID { get; set; }

        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime ReviewDate { get; set; }

        public virtual Products Product { get; set; }
        public virtual Users User { get; set; }
        public virtual Orders Order { get; set; }
    }
}
