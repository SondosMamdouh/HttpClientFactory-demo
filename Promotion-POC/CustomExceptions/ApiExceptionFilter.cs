using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Promotion_POC.CustomExceptions
{
	public class ApiExceptionFilter : IExceptionFilter
	{
		public void OnException(ExceptionContext context)
		{
			var statusCode = 500; 

			if (context.Exception is ApiException apiException)
			{
				statusCode = apiException.StatusCode;
			}

			var response = new
			{
				StatusCode = statusCode,
				Message = context.Exception.Message,
				Details = context.Exception.InnerException?.Message
			};

			context.Result = new JsonResult(response)
			{
				StatusCode = statusCode
			};

			context.ExceptionHandled = true;
		}
	}
}
