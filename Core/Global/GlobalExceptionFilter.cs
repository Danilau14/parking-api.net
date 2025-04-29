namespace ParkingApi.Core.Global;

public class GlobalExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {

        int statusCode;
        object response;

        if (context.Exception is EipexException eipexException)
        {
            statusCode = (int)eipexException.StatusCode;
            response = eipexException.ErrorResponse;
        }
        else
        {
            statusCode = (int)HttpStatusCode.InternalServerError;
            response = new ErrorResponse
            {
                Message = context.Exception.Message,
                ErrorCode = "UNHANDLED_EXCEPTION"
            };
        }

        context.Result = new ObjectResult(response)
        {
            StatusCode = statusCode
        };

        context.ExceptionHandled = true;
    }
}
