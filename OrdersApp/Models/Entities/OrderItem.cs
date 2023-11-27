using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrdersApp.Models.Entities
{
    [Table("OrderItem")]
    public class OrderItem : IEntity
    {
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[ForeignKey("OrderId"), Required]
        public Order? Order { get; set; }

		[Column(TypeName = "nvarchar(max)"), Required]
		public string? Name { get; set; }

		[Column(TypeName = "decimal(18,3)"), Required]
		public decimal Quantity { get; set; }

		[Column(TypeName = "nvarchar(max)")]
		public string? Unit { get; set; }
    }
}
