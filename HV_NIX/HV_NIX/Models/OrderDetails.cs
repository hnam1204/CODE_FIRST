using HV_NIX.Models;
using System.ComponentModel.DataAnnotations;

public class OrderDetails
{
    [Key]
    public int OrderDetailID { get; set; }

    public int OrderID { get; set; }
    public int ProductID { get; set; }

    // 👉 BẮT BUỘC phải có theo code của bạn
    public int Quantity { get; set; }
    public decimal Price { get; set; }

    public virtual Orders Order { get; set; }
    public virtual Products Product { get; set; }
}
