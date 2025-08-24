using Base.Application.Dtos.Interfaces;

namespace Application.Dtos.Authentication;

public class ClaimDto : IEntityDto
{
    public string Id { get; set; }
    public string Type { get; set; }
    public string Value { get; set; }
}