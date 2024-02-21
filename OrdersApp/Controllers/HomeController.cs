using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using OrdersApp.ActionFilters;
using OrdersApp.Exceptions;
using OrdersApp.Models.Entities;
using OrdersApp.Models.Repositories;
using OrdersApp.Services;
using OrdersApp.ViewModels;
using System.Diagnostics;

namespace OrdersApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IService _service;
		private readonly IRepository<Provider> _providersRepo;
		private readonly IRepository<OrderItem> _orderItemsRepo;

		public HomeController(
            IService service,
            IRepository<Provider> providersRepository,
            IRepository<OrderItem> orderItemsRepository
        )
        {
            (_providersRepo, _orderItemsRepo, _service) = 
                (providersRepository, orderItemsRepository, service);
        }

        [HttpGet]
        async public Task<IActionResult> Index(OrdersVM input) {
            await CheckForData();
            var filtered = await _service.GetOrdersFilteredAsync(input.Criteria);
            OrdersVM vm = new() { 
                Orders = filtered, 
                Providers = await _providersRepo.GetAllAsync(),
                Criteria = new()
            };
            vm.Criteria.ProviderId = input.Criteria?.ProviderId ?? vm.Providers.FirstOrDefault()?.Id;
            vm.Criteria.DateFrom = input.Criteria?.DateFrom ?? DateTime.Now - TimeSpan.FromDays(30);
            vm.Criteria.DateTo = input.Criteria?.DateTo ?? DateTime.Now;
            vm.ProvidersList = new SelectList(vm.Providers, "Id", "Name");
            return View(vm); 
        }

        [HttpGet]
        async public Task<IActionResult> ViewOrder(int id) {
            Order? order = await _service.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound();
            ViewOrderVM vm = new() { Order = order };
            return View(vm);
        }

		[HttpGet]
        [ImportModelState]
		async public Task<IActionResult> AddOrder()
		{
			AddEditOrderVM vm = new(await _providersRepo.GetAllAsync());
            vm.Action = "Add";
			return View("AddEditOrder", vm);
		}

		[HttpPost]
        [ExportModelState]
		async public Task<IActionResult> AddOrder([FromForm] AddEditOrderVM vm)
		{
			Provider? provider = await _providersRepo.GetAsync(vm.ProviderId);
            int id = -1;
            try
            {
                id = await _service.AddOrderAsync(vm.Number, vm.Date, provider!);
            }
            catch (BusinessConstraintException bce) {
				ModelState.AddModelError("Number", bce.Message);
                vm.SetProvidersList(await  _providersRepo.GetAllAsync());
                return View("AddEditOrder", vm);
			}
			return RedirectToAction("ViewOrder", new { id = id});
		}

		[HttpGet]
        [ImportModelState]
        async public Task<IActionResult> EditOrder(int id) {
            Order? order = await _service.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound();
            AddEditOrderVM vm = new(order, await _providersRepo.GetAllAsync());
			vm.Action = "Edit";
			return View("AddEditOrder", vm);
        }

		[HttpPost]
        [ExportModelState]
		async public Task<IActionResult> EditOrder([FromRoute] int id, [FromForm] AddEditOrderVM vm)
		{
			Provider? provider = await _providersRepo.GetAsync(vm.ProviderId);
            try
            {
                await _service.UpdateOrderAsync(id, vm.Number, vm.Date, provider!);
            }
            catch (BusinessConstraintException bce)
            {
                ModelState.AddModelError("Number", bce.Message);
            }
			return RedirectToAction("EditOrder", new { id = id});
		}

        [HttpGet]
        async public Task<IActionResult> DeleteOrder(int id)
        {
            await _service.DeleteOrderAsync(id);
            return RedirectToAction("Index");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        async private Task CheckForData() {

            int id = 0;

		    var providers = await _providersRepo.GetAllAsync();
            id = (providers.Any()) 
                ? providers.First().Id : await _providersRepo.AddAsync(new() { Name = "SomeProvider1" });

            if ((await _providersRepo.GetAllAsync()).Count() < 3) {
                await _providersRepo.AddAsync(new() { Name = "OtherProvider" });
                await _providersRepo.AddAsync(new() { Name = "BigCompany" });
            };

			var orders = await _service.GetOrdersFilteredAsync(null);
            if (orders.Count() < 2) {
                await _service.AddOrderAsync("AA123", DateTime.Now, await _providersRepo.GetAsync(id));
				await _service.AddOrderAsync("Old_Order", DateTime.Now - TimeSpan.FromDays(5), await _providersRepo.GetAsync(++id));
            }
        }
    }
}