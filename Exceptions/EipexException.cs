namespace ParkingApi.Exceptions;

public class EipexException : Exception
{
    public HttpStatusCode StatusCode { get; }
    public ErrorResponse ErrorResponse { get; }

    public EipexException(
        ErrorResponse errorResponse, 
        HttpStatusCode statusCode = HttpStatusCode.BadRequest
        )
    : base(errorResponse.Message)
    {
        ErrorResponse = errorResponse;
        StatusCode = statusCode;
    }
}
