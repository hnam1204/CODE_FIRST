using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HV_NIX.Models
{
    public class ShippingAddress
    {
        [Key]
        public int AddressID { get; set; }

        public int UserID { get; set; }

        [Required]
        public string ReceiverName { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string AddressLine { get; set; }

        [Required]
        public string City { get; set; }

        public bool IsDefault { get; set; }

        // FK
        [ForeignKey("UserID")]
        public virtual Users User { get; set; }
    }
}
