using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;

namespace OrdersApp.ActionFilters
{
	public class ExportModelStateAttribute : ModelStateTransfer
	{
		public override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			Controller? controller = filterContext.Controller as Controller;
			if (controller != null && !controller.ViewData.ModelState.IsValid) {
				if ((filterContext.Result is RedirectResult) || (filterContext.Result is RedirectToActionResult))
				{
					if (controller != null && filterContext.ModelState != null)
					{
						var modelState = ModelStateHelpers.SerialiseModelState(filterContext.ModelState);
						controller.TempData[Key] = modelState;
					}
				}
			}
			base.OnActionExecuted(filterContext);
		}
	}
}
