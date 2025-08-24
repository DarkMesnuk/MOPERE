using System.Net;
using Base.Domain.Attributes;
using Base.Domain.Exceptions.Base;
using Base.Domain.Helpers;

namespace Base.Domain.Exceptions;

[HttpStatusCode(HttpStatusCode.BadRequest)]
public class UpdateFailedException() : MopereException(StatusCodes.UpdateFailed);