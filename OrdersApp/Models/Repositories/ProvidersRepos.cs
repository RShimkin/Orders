using OrdersApp.Models.Contexts;
using OrdersApp.Models.Entities;

namespace OrdersApp.Models.Repositories
{
	public class ProvidersRepository : AbstractRepository<Provider>
	{
		public ProvidersRepository(AppDbContext context) : base(context) => Table = context.Providers;
	}
}