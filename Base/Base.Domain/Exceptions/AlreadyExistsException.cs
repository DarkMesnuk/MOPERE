using System.Net;
using Base.Domain.Attributes;
using Base.Domain.Exceptions.Base;
using Base.Domain.Helpers;

namespace Base.Domain.Exceptions;

[HttpStatusCode(HttpStatusCode.Conflict)]
public class AlreadyExistsException : MopereException
{
    public AlreadyExistsException(
        string fieldName,
        string fieldValue
    ) : base(StatusCodes.AlreadyExists, $"{fieldName} \"{fieldValue}\" already exists") { }

    public AlreadyExistsException(
        string message
    ) : base(StatusCodes.AlreadyExists, message) { }
}