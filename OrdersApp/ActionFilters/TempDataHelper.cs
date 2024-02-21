using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json;

namespace OrdersApp.ActionFilters
{
	/*public static class TempDataHelper
	{
		public static void Put<T>(this ITempDataDictionary tempData, string key, T value) where T : class
		{
			tempData[key] = JsonSerializer.Serialize(value);
		}

		public static T Get<T>(this ITempDataDictionary tempData, string key) where T : class
		{
			tempData.TryGetValue(key, out object o);
			return o == null ? null : JsonSerializer.Deserialize<T>((string)o);
		}

		public static T Peek<T>(this ITempDataDictionary tempData, string key) where T : class
		{
			object o = tempData.Peek(key);
			return o == null ? null : JsonSerializer.Deserialize<T>((string)o);
		}
	}*/

	public class ModelStateTransferValue
	{
		public string? Key { get; set; }
		public string? AttemptedValue { get; set; }
		public object? RawValue { get; set; }
		public ICollection<string>? ErrorMessages { get; set; } = new List<string>();
	}

	public static class ModelStateHelpers
	{
		public static string SerialiseModelState(ModelStateDictionary modelState)
		{
			var errorList = modelState
				.Select(kvp => new ModelStateTransferValue
				{
					Key = kvp.Key,
					AttemptedValue = kvp.Value?.AttemptedValue,
					RawValue = kvp.Value?.RawValue,
					ErrorMessages = kvp.Value?.Errors.Select(err => err.ErrorMessage).ToList(),
				});

			var res = JsonSerializer.Serialize(errorList);
			return res;
		}

		public static ModelStateDictionary? DeserialiseModelState(string serialisedErrorList)
		{
			List<ModelStateTransferValue>? errorList = null;
			try { 
				errorList = JsonSerializer.Deserialize<List<ModelStateTransferValue>>(serialisedErrorList); 
			}
			catch {
				return null;
			}
			if (errorList == null) return null;
			var modelState = new ModelStateDictionary();

			foreach (var item in errorList)
			{
				if (item.Key == null || item.ErrorMessages == null) continue;
				modelState.SetModelValue(item.Key, item.RawValue, item.AttemptedValue);
				foreach (var error in item.ErrorMessages)
				{
					modelState.AddModelError(item.Key, error);
				}
			}
			return modelState;
		}
	}
}
