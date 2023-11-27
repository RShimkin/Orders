using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OrdersApp.Models.Entities
{
    [Table("Provider")]
    public class Provider : IEntity
    {
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[Column(TypeName = "nvarchar(max)"), Required]
		public string? Name { get; set; }
    }
}
