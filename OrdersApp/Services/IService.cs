using OrdersApp.Models;
using OrdersApp.Models.Entities;

namespace OrdersApp.Services
{
    public interface IService
    {
        Task<IEnumerable<Order>> GetOrdersFilteredAsync(Criteria? criteria);

		Task<Order?> GetOrderByIdAsync(int id);

        Task<int> AddOrderAsync(string number, DateTime date, Provider? provider);

		Task<bool> UpdateOrderAsync(int id, string number, DateTime date, Provider provider);

        Task<bool> DeleteOrderAsync(int id);
	}
}
