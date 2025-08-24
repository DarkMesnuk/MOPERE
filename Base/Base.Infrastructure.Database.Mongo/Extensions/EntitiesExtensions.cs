using Base.Infrastructure.Database.Mongo.Entities.Interfaces;

namespace Base.Infrastructure.Database.Mongo.Extensions;

public static class EntitiesExtensions
{
    public static bool IsNullOrDefault<T>(this IMongoEntity<T>? model)
    {
        if (model == null) 
            return true;

        if ((typeof(T) == typeof(int) || typeof(T) == typeof(long)) && model.Id.Equals(0))
            return true;
        
        return false;
    }

    public static bool IsAvailable<T>(this IMongoEntity<T> model)
    {
        return !model.IsNullOrDefault();
    }
}