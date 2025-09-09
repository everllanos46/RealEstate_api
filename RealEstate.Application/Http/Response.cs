using System.Net;

namespace RealEstate.Application;

public class Response<T>
{
    public Response(HttpStatusCode httpStatusCode)
    {
        HttpStatusCode = httpStatusCode;
    }

    public Response(string message, HttpStatusCode httpStatusCode, T data)
    {
        Message = message;
        HttpStatusCode = httpStatusCode;
        Data = data;
    }

    public Response(string message, HttpStatusCode httpStatusCode)
    {
        Message = message;
        HttpStatusCode = httpStatusCode;
    }

    public string Message { get; set; } = default!;
    public HttpStatusCode HttpStatusCode { get; set; }
    public T Data { get; set; } = default!;
}