namespace Base.Domain.Entities.Interfaces;

public interface IEntity<T> : IEntity
{
    T Id { get; set; }
}

public interface IEntity;