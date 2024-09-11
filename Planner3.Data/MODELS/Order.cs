using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planner3.Data.MODELS;
[Table("Order")]
public class Order
{
    [Key]
    public int Id { get; set; }
    public int ProductId { get; set; }
    [ForeignKey("ProductId")]
    public Product Product { get; set; }
    public int Quantity { get; set; }
    public bool IsCompleted { get; set; } = false;
    public int UserId { get; set; }

}
