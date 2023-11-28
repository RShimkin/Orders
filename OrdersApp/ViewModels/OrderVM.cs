using Microsoft.AspNetCore.Mvc.Rendering;
using OrdersApp.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace OrdersApp.ViewModels
{
    public class OrderVM
    {
        [Required(ErrorMessage = "Order Number is required")]
        [DataType(DataType.Text)]
        public string? Number { get; set; }

        [Required(ErrorMessage = "Order date is required")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Order Provider is required")]
        //[Range(0, int.MaxValue, ErrorMessage = "Please enter valid integer")]
        public int ProviderId { get; set; }

        public SelectList? Providers { get; set; } = null;

        public OrderVM() { }

		public OrderVM(IEnumerable<Provider> providers)
		{
			Providers = new SelectList(providers, "Id", "Name");
		}

		public OrderVM(Order order, IEnumerable<Provider> providers) {
            Number = order.Number;
            Date = order.Date;
            ProviderId = order.Provider.Id;
            Providers = new SelectList(providers, "Id", "Name");
        }

        public string? ActionName { get; set; }
    }
}
