namespace Base.Domain.Entities.Interfaces;

public interface IEntityDateTime
{
    DateTime CreatedAt { get; set; }
    DateTime ModifiedAt { get; set; }   
}