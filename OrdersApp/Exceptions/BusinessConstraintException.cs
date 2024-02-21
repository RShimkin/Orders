namespace OrdersApp.Exceptions
{
	public class BusinessConstraintException : Exception
	{
		public BusinessConstraintException(string message) : base(message) { }
	}
}
