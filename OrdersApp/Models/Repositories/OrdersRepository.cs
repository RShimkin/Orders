using OrdersApp.Models.Contexts;
using OrdersApp.Models.Entities;

namespace OrdersApp.Models.Repositories
{
	public class OrdersRepository : AbstractRepository<Order>
	{
		public OrdersRepository(AppDbContext context) : base(context) => Table = context.Orders; 
	}
}