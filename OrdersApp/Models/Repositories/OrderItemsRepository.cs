using OrdersApp.Models.Contexts;
using OrdersApp.Models.Entities;

namespace OrdersApp.Models.Repositories
{
		public class OrderItemsRepository : AbstractRepository<OrderItem>
		{
				public OrderItemsRepository(AppDbContext context) : base(context) => Table = context.OrderItems;
		}
}