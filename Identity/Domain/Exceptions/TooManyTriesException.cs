using System.Net;
using Base.Domain.Attributes;
using Base.Domain.Exceptions.Base;
using Base.Domain.Helpers;

namespace Domain.Exceptions;

[HttpStatusCode(HttpStatusCode.BadRequest)]
public class TooManyTriesException() : MopereException(StatusCodes.TooManyTries);