using Microsoft.AspNetCore.Mvc.Filters;

namespace OrdersApp.ActionFilters
{
	public class ModelStateTransfer : ActionFilterAttribute
	{
		protected static readonly string Key = typeof(ModelStateTransfer).FullName ?? "ModelStateTransfer";
	}
}
