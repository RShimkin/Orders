using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace OrdersApp.Models.Entities
{
    [Table("Order")]
    public class Order : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(max)"), Required]
        public string? Number { get; set; }

		[Column(TypeName = "datetime2(7)"), Required]
		public DateTime Date { get; set; }

        [ForeignKey("ProviderId")]
        public Provider? Provider { get; set; }

        public List<OrderItem>? OrderItems { get; set; }
    }
}
