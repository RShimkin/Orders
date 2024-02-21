using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace OrdersApp.ActionFilters
{
	public class ImportModelStateAttribute : ModelStateTransfer
	{
		public override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			Controller? controller = filterContext.Controller is Controller 
				? filterContext.Controller as Controller : null;
			string? modelStateStr = controller?.TempData[Key] is string ? controller?.TempData[Key] as string : null;
			if (modelStateStr != null) {
				if (filterContext.Result is ViewResult) {
					var modelState = ModelStateHelpers.DeserialiseModelState(modelStateStr);
					if (modelState != null)
						filterContext.ModelState.Merge(modelState);
				} else {
					controller?.TempData.Remove(Key);
				}
			}
			base.OnActionExecuted(filterContext);
		}
	}
}
