using Microsoft.AspNetCore.Mvc.Rendering;
using OrdersApp.Models;
using OrdersApp.Models.Entities;

namespace OrdersApp.ViewModels
{
    public class OrdersVM
    {
        public IEnumerable<Order>? Orders { get; set; } = Enumerable.Empty<Order>();

        public IEnumerable<Provider>? Providers { get; set; } = Enumerable.Empty<Provider>();

        public Criteria? Criteria { get; set; }

		public SelectList? ProvidersList { get; set; } = null;
	}
}
