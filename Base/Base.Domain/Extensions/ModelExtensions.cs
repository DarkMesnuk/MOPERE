using Base.Domain.Models.Interfaces;

namespace Base.Domain.Extensions;

public static class ModelExtensions
{
    public static bool IsNullOrDefault<T>(this IEntityModel<T>? model)
    {
        if (model == null) 
            return true;

        if ((typeof(T) == typeof(int) || typeof(T) == typeof(long)) && model.Id.Equals(0))
            return true;
        
        return false;
    }

    public static bool IsAvailable<T>(this IEntityModel<T>? model)
    {
        return !model.IsNullOrDefault();
    }
}