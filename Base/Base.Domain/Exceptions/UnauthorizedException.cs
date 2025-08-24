using System.Net;
using Base.Domain.Attributes;
using Base.Domain.Exceptions.Base;
using Base.Domain.Helpers;

namespace Base.Domain.Exceptions;

[HttpStatusCode(HttpStatusCode.Unauthorized)]
public class UnauthorizedException() : MopereException(StatusCodes.Unauthorized)
{
    public override string Message => "Unauthorized";
}