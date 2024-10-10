using Domain.Faculties;

namespace Api.Dtos;

public record FacultyDto(Guid? Id, string Name, DateTime? UpdatedAt)
{
    public static FacultyDto FromDomainModel(Faculty faculty)
        => new(
            Id: faculty.Id.Value,
            Name: faculty.Name,
            UpdatedAt: faculty.UpdatedAt);
}