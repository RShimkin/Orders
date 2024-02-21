using Microsoft.EntityFrameworkCore;
using OrdersApp.Models.Contexts;
using OrdersApp.Models.Entities;

namespace OrdersApp.Models.Repositories
{
	public class OrdersRepository : AbstractRepository<Order>
	{
		public OrdersRepository(AppDbContext context) : base(context) => Table = context.Orders;

		public override async Task<Order?> GetAsync(int id) => 
			await Table.Include(x => x.Provider).FirstOrDefaultAsync(e => e.Id == id);
	}
}