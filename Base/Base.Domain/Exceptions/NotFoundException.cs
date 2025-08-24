using System.Net;
using Base.Domain.Attributes;
using Base.Domain.Exceptions.Base;
using Base.Domain.Helpers;

namespace Base.Domain.Exceptions;

[HttpStatusCode(HttpStatusCode.BadRequest)]
public class NotFoundException : MopereException
{
    public NotFoundException(string entityType, string name)
        : base(StatusCodes.NotFound, $"{entityType} {name} is not found")
    {
    }
    
    public NotFoundException(string entityType)
        : base(StatusCodes.NotFound, $"{entityType} by filter is not found")
    {
    }
}