using Domain.Faculties;
using Optional;

namespace Application.Common.Interfaces.Queries;

public interface IFacultyQueries
{
    Task<IReadOnlyList<Faculty>> GetAll(CancellationToken cancellationToken);
    Task<Option<Faculty>> GetById(FacultyId id, CancellationToken cancellationToken);

}