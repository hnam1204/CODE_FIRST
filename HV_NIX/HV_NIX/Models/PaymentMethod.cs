using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HV_NIX.Models
{
    public class PaymentMethod
    {
        [Key]
        public int MethodID { get; set; }

        public int UserID { get; set; }

        public string MethodType { get; set; }    // "Bank", "MoMo", "Cash"
        public string Provider { get; set; }       // Vietcombank / MOMO / …
        public string AccountNumber { get; set; }  // Số thẻ
        public bool IsDefault { get; set; }

        [ForeignKey("UserID")]
        public virtual Users User { get; set; }
    }
}
