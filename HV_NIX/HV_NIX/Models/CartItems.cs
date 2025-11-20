using System.ComponentModel.DataAnnotations;

namespace HV_NIX.Models
{
    public class CartItems
    {
        [Key]
        public int CartItemsID { get; set; }
        public int CartID { get; set; }
        public int ProductID { get; set; }
        public int Qty { get; set; }

        public virtual Cart Cart { get; set; }
        public virtual Products Product { get; set; }
    }
}
