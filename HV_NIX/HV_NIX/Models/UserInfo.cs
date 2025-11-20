using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HV_NIX.Models
{
    public class UserInfo
    {
        [Key]
        [ForeignKey("User")]   // THIS IS THE FIX!!!
        public int UserID { get; set; }

        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

        public virtual Users User { get; set; }
    }
}
