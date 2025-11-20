using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HV_NIX.Models
{
    public class Cart
    {
        [Key]
        public int CartID { get; set; }
        public int UserID { get; set; }

        public virtual Users User { get; set; }
        public virtual ICollection<CartItems> CartItems { get; set; }
    }
}
