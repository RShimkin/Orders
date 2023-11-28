using Microsoft.AspNetCore.Mvc;
using OrdersApp.Models.Entities;
using OrdersApp.Models.Repositories;
using OrdersApp.ViewModels;
using System.Diagnostics;

namespace OrdersApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository<Order> _ordersRepo;
				private readonly IRepository<Provider> _providersRepo;
				private readonly IRepository<OrderItem> _orderItemsRepo;

		public HomeController(
            IRepository<Order> ordersRepository,
            IRepository<Provider> providersRepository,
            IRepository<OrderItem> orderItemsRepository
        )
        {
            (_ordersRepo, _providersRepo, _orderItemsRepo) = (ordersRepository, providersRepository, orderItemsRepository);
        }

        [HttpGet]
        async public Task<IActionResult> Index(OrdersVM input) {
            await CheckForData();
            OrdersVM vm = new() { Orders = await _ordersRepo.GetAllAsync(), Providers = await _providersRepo.GetAllAsync() };
            return View(vm); 
        }

		[HttpGet]
		async public Task<IActionResult> AddOrder()
		{
			OrderVM vm = new(await _providersRepo.GetAllAsync());
            vm.ActionName = "AddOrder";
			return View("AddEditOrder", vm);
		}

		[HttpPost]
		async public Task<IActionResult> AddOrder([FromForm] OrderVM vm)
		{
			//if (!ModelState.IsValid)
			Provider provider = await _providersRepo.GetAsync(vm.ProviderId);
            Order order = new() { Number = vm.Number, Date = vm.Date, Provider = provider };
			int id = await _ordersRepo.AddAsync(order);
			return RedirectToAction("EditOrder", id);
		}

		[HttpGet]
        async public Task<IActionResult> EditOrder(int id) {
            OrderVM vm = new(await _ordersRepo.GetAsync(id), await _providersRepo.GetAllAsync());
			vm.ActionName = "EditOrder";
			return View("AddEditOrder", vm);
        }

		[HttpPost]
		async public Task<IActionResult> EditOrder([FromRoute] int id, [FromForm] OrderVM vm)
		{
			Order order = await _ordersRepo.GetAsync(id);
            Provider provider = await _providersRepo.GetAsync(vm.ProviderId);
            (order.Number, order.Date, order.Provider) = (vm.Number, vm.Date, provider);
            await _ordersRepo.UpdateAsync(order);
			return RedirectToAction("EditOrder", id);
		}

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        async private Task CheckForData() {

            int id = 0;

		    var providers = await _providersRepo.GetAllAsync();
            id = providers.Any() 
                ? providers.First().Id : await _providersRepo.AddAsync(new() { Name = "SomeProvider1" });

			var orders = await _ordersRepo.GetAllAsync();
            if (!orders.Any()) {
                await _ordersRepo.AddAsync(new() { 
                    Date = DateTime.Now, Number = "AA123", Provider = await _providersRepo.GetAsync(id) });
            }
        }
    }
}