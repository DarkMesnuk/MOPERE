namespace Base.API.Responses;

public abstract class BaseResponse<T>(T Dto)
{
    public T Result { get; private set; } = Dto;
}