using OrdersApp.Exceptions;
using OrdersApp.Models;
using OrdersApp.Models.Entities;
using OrdersApp.Models.Repositories;

namespace OrdersApp.Services
{
    public class BusinessService : IService
    {
        private readonly IRepository<Order> _ordersRepo;
        public BusinessService(IRepository<Order> ordersRepository) => _ordersRepo = ordersRepository;

		async public Task<int> AddOrderAsync(string number, DateTime date, Provider? provider)
		{
            if (provider == null) throw new EntityNotFound("Provider is null");
            if (!(await checkUniqueness(number, date, provider)))
				throw new BusinessConstraintException($"Order '{number}' by {provider.Name} already exists");

			Order newOrder = new() { Number = number, Date = date, Provider = provider };
            return await _ordersRepo.AddAsync(newOrder);
		}

		async public Task<bool> UpdateOrderAsync(int id, string number, DateTime date, Provider provider)
		{
            Order? order = await _ordersRepo.GetAsync(id);
            if (order == null)
                return false;
                //throw new EntityNotFound($"Order #{id} not found");
            if (!(await checkUniqueness(number, date, provider)))
                throw new BusinessConstraintException($"Order '{number}' by {provider.Name} already exists");
			(order.Number, order.Date, order.Provider) = (number, date, provider);
            await _ordersRepo.UpdateAsync(order);
            return true;
		}

		async public Task<Order?> GetOrderByIdAsync(int id)
        {
            return await _ordersRepo.GetAsync(id);
        }

        async public Task<IEnumerable<Order>> GetOrdersFilteredAsync(Criteria? criteria)
        {
            var all = await _ordersRepo.GetAllAsync();
            if (criteria != null)
            {
                if (criteria.ProviderId != null)
                {
                    all = all.Where(x => x.Provider?.Id == criteria.ProviderId);
                }
                if (criteria.DateFrom != null)
                {
                    all = all.Where(x => x.Date >= criteria.DateFrom.Value);
                }
                if (criteria.DateTo != null)
                {
                    all = all.Where(x => x.Date <= criteria.DateTo.Value);
                }
            }
            return all;
        }
	
        async public Task<bool> DeleteOrderAsync(int id) {
			Order? order = await _ordersRepo.GetAsync(id);
            if (order == null)
                return false;

            await _ordersRepo.DeleteAsync(id);
            return true;
		}
        
        async private Task<bool> checkUniqueness(string number, DateTime date, Provider provider) {
			var alike = (await _ordersRepo.GetAllAsync()).Where(o => o.Number == number && o.Provider?.Id == provider.Id);
            return !alike.Any();
		}
    }
}
