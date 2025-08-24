using Microsoft.AspNetCore.Mvc;

namespace Base.Domain.Helpers.Models;

public class ApplicationResponse : ErrorResult
{
    public int HttpCode { get; set; }

    public bool IsSucceeded => HttpCode == 200;

    public ApplicationResponse()
    {
        HttpCode = 200;
    }

    public ApplicationResponse(int httpCode, string message)
    {
        Message = message;
        HttpCode = httpCode;
    }

    public ApplicationResponse(string status, string message, int httpCode = 200)
    {
        Status = status;
        Message = message;
        HttpCode = httpCode;
    }

    public void SetData(string status, string message, int httpCode = 200)
    {
        Status = status;
        Message = message;
        HttpCode = httpCode;
    }

    public ApplicationResponse SetAdditionalMessage(string message)
    {
        Errors.Add(message);

        return this;
    }

    public ApplicationResponse SetAdditionalMessage(IEnumerable<string> messages)
    {
        Errors.AddRange(messages);

        return this;
    }

    public ApplicationResponse SetAdditionalMessage(StatusCodes code)
    {
        var result = ApplicationResponseStatuses.Statuses.GetValueOrDefault(code);
        SetAdditionalMessage(result.Message);

        return this;
    }

    public ApplicationResponse SetData(ApplicationResponse response)
    {
        Status = response.Status;
        Message = response.Message;
        HttpCode = response.HttpCode;
        Type = response.Type;
        Errors.AddRange(response.Errors);

        return this;
    }
    
    public ApplicationResponse SetData(StatusCodes code)
    {
        var result = ApplicationResponseStatuses.Statuses.GetValueOrDefault(code);
        SetData(result!);

        return this;
    }

    public ApplicationResponse SetData(StatusCodes code, string message)
    {
        SetData(code);
        
        if (!string.IsNullOrEmpty(message))
        {
            Message = message;
        }

        return this;
    }
    
    public virtual IActionResult GetActionResult()
    {
        var result = new ObjectResult(GenerateDefaultObjectForActionResult())
        {
            StatusCode = HttpCode
        };
        
        return result;
    }
    
    public ErrorResult GetErrorResult()
    {
        return this;
    }

    public ApplicationResponse SetFieldErrorMessage(string propertyName, string message)
    {
        Errors.Add($"Field: {propertyName}, Error: {message}");

        return this;
    }

    protected object GenerateDefaultObjectForActionResult() => new { 
        Status,
        Message,
        Errors,
        Type
    };
}

public class ApplicationResponse<TResponse> : ApplicationResponse
    where TResponse : ApplicationResponse
{
    protected ApplicationResponse()
    {
        SetData(StatusCodes.Success);
    }
    
    public new TResponse SetAdditionalMessage(string message)
        => (TResponse) base.SetAdditionalMessage(message);
    
    public new TResponse SetAdditionalMessage(IEnumerable<string> messages)
        => (TResponse) base.SetAdditionalMessage(messages);
    
    public new TResponse SetAdditionalMessage(StatusCodes code)
        => (TResponse) base.SetAdditionalMessage(code);
    
    public new TResponse SetData(ApplicationResponse response)
        => (TResponse) base.SetData(response);
    
    public new TResponse SetData(StatusCodes code)
        => (TResponse) base.SetData(code);

    public new TResponse SetData(StatusCodes code, string message)
        => (TResponse) base.SetData(code, message);
    
    
    public new TResponse SetFieldErrorMessage(string propertyName, string message)
        => (TResponse) base.SetFieldErrorMessage(propertyName, message);
}

public class ErrorResult
{
    public string Status { get; set; }
    public string Type { get; set; }
    public string Message { get; set; }
    public List<string> Errors { get; } = new();
}