using Microsoft.AspNetCore.Mvc.Rendering;
using OrdersApp.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace OrdersApp.ViewModels
{
    public class AddEditOrderVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Order Number is required")]
        [DataType(DataType.Text)]
        public string Number { get; set; } = string.Empty;

        [Required(ErrorMessage = "Order date is required")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Order Provider is required")]
        public int ProviderId { get; set; }

        public SelectList? Providers { get; set; } = null;

        public AddEditOrderVM() { }

		public AddEditOrderVM(IEnumerable<Provider> providers)
		{
			SetProvidersList(providers);
            Date = DateTime.Now;
		}

		public AddEditOrderVM(Order order, IEnumerable<Provider> providers) {
            Id = order.Id;
            Number = order.Number;
            Date = order.Date;
            ProviderId = order.Provider.Id;
            SetProvidersList(providers);   
        }

        public void SetProvidersList(IEnumerable<Provider> providers) {
            Providers = new SelectList(providers, "Id", "Name");
        }

        public string? Action { get; set; }

        public string ActionName { get => $"{Action ?? "View"}Order"; }
    }
}
