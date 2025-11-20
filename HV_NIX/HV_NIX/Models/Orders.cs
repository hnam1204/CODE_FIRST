using HV_NIX.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class Orders
{
    [Key]
    public int OrderID { get; set; }

    public int UserID { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal Total { get; set; }

    // 👉 THÊM PROPERTY NÀY
    public string Status { get; set; }

    public virtual Users User { get; set; }
    public virtual ICollection<OrderDetails> Details { get; set; }
}
