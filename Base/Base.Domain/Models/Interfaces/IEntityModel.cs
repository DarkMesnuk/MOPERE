namespace Base.Domain.Models.Interfaces;

public interface IEntityModel<T> : IEntityModel
{
    T Id { get; set; }
}

public interface IEntityModel;