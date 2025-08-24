using Base.Domain.Helpers.Models;

namespace Base.Domain.Helpers;

public enum StatusCodes
{
    Success = 1000,
    NotFound = 1001,
    SomethingWentWrong = 1002,
    SomeFieldsMustBeUnique = 1003,
    WrongDataFormat = 1004,
    UnsupportedContentType = 1005,
    CreationFailed = 1006,
    UpdateFailed = 1007,
    DeleteFailed = 1008,
    FileUploadFailed = 1009,
    ValidationFailed = 1010,
    ServerSomethingWentWrong = 1011,
    AlreadyExists = 1012,
    Conflict = 1013,
    
    Unauthorized = 2001,
    AccessDenied = 2002,
    IncorrectCredentials = 2003,
    Blocked = 2004,
    TooManyTries = 2005,
    InvalidCode = 2006,
    CodeExpired = 2007,
    AccountDeleted = 2008
}

public static class ApplicationResponseStatuses
{
    public static Dictionary<StatusCodes, ApplicationResponse> Statuses;

    static ApplicationResponseStatuses()
    {
        Statuses = new Dictionary<StatusCodes, ApplicationResponse>();

        Statuses
            .CreatePair(StatusCodes.Success, "Success", 200)
            .CreatePair(StatusCodes.NotFound, "Not found", 400)
            .CreatePair(StatusCodes.SomethingWentWrong, "Something went wrong", 400)
            .CreatePair(StatusCodes.SomeFieldsMustBeUnique, "Some fields must be unique", 400)
            .CreatePair(StatusCodes.WrongDataFormat, "Wrong data format", 400)
            .CreatePair(StatusCodes.UnsupportedContentType, "Unsupported content type", 415)
            .CreatePair(StatusCodes.CreationFailed, "Creation failed", 400)
            .CreatePair(StatusCodes.UpdateFailed, "Failed to update", 400)
            .CreatePair(StatusCodes.DeleteFailed, "Failed to delete", 400)
            .CreatePair(StatusCodes.FileUploadFailed, "File upload failed", 400)
            .CreatePair(StatusCodes.ValidationFailed, "Validation failed", 400)
            .CreatePair(StatusCodes.AlreadyExists, "Already exists", 409)
            .CreatePair(StatusCodes.ServerSomethingWentWrong, "Something went wrong", 500)
            .CreatePair(StatusCodes.Conflict, "Conflict", 409)
            
            .CreatePair(StatusCodes.Unauthorized, "Unauthorized", 401)
            .CreatePair(StatusCodes.AccessDenied, "Access denied", 403)
            .CreatePair(StatusCodes.IncorrectCredentials, "Incorrect credentials", 400)
            .CreatePair(StatusCodes.Blocked, "Account blocked", 401)
            .CreatePair(StatusCodes.TooManyTries, "Too many tries", 400)
            .CreatePair(StatusCodes.InvalidCode, "Invalid code", 400)
            .CreatePair(StatusCodes.CodeExpired, "Code expired", 400)
            .CreatePair(StatusCodes.AccountDeleted, "Account have deleted", 400)
            ;
    }
    
    private static Dictionary<StatusCodes, ApplicationResponse> CreatePair(this Dictionary<StatusCodes, ApplicationResponse> statuses, StatusCodes statusCode, string description, int httpCode)
    {
        statuses.Add(statusCode, new ApplicationResponse { Status = statusCode.GetStatusCode(), Type = statusCode.ToString(), Message = description, HttpCode = httpCode} );
        return statuses;
    }

    private static string GetStatusCode(this StatusCodes code)
    {
        return $"{(int)code}";
    }
}