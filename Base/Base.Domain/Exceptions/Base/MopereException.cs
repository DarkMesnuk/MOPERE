using Base.Domain.Helpers;
using Base.Domain.Helpers.Models;

namespace Base.Domain.Exceptions.Base;

public abstract class MopereException : Exception
{
    protected MopereException() : this(StatusCodes.SomethingWentWrong)
    {
    }
    
    protected MopereException(string error) : this(StatusCodes.SomethingWentWrong, error)
    {
    }
    
    protected MopereException(StatusCodes code, string error) : base(error)
    {
        var result = ApplicationResponseStatuses.Statuses.GetValueOrDefault(code);
        ErrorResponse = new ApplicationResponse().SetData(result!);
        ErrorResponse.SetAdditionalMessage(error);
    }
    
    protected MopereException(StatusCodes code)
    {
        var result = ApplicationResponseStatuses.Statuses.GetValueOrDefault(code);
        ErrorResponse = new ApplicationResponse().SetData(result!);
    }

    public ApplicationResponse ErrorResponse { get; }
}