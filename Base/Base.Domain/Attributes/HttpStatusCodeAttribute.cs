using System.Net;

namespace Base.Domain.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public class HttpStatusCodeAttribute(
    HttpStatusCode statusCode
) : Attribute
{
    public HttpStatusCode StatusCode { get; } = statusCode;
}