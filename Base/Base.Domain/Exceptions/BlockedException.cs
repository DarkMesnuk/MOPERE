using System.Net;
using Base.Domain.Attributes;
using Base.Domain.Exceptions.Base;
using Base.Domain.Helpers;

namespace Base.Domain.Exceptions;

[HttpStatusCode(HttpStatusCode.Unauthorized)]
public class BlockedException(DateTimeOffset? blockedTime = null) : MopereException(StatusCodes.Blocked,
    $"Account blocked - {blockedTime?.ToString() ?? "Unknown"}")
{
    public override string Message => $"Account blocked - {blockedTime?.ToString() ?? "Unknown"}";
}