using System.Net;
using Base.Domain.Attributes;
using Base.Domain.Exceptions.Base;

namespace Base.Domain.Exceptions;

[HttpStatusCode(HttpStatusCode.BadRequest)]
public class SomethingWentWrongException(string message) : MopereException(message);