using Domain.Courses;
using Domain.Faculties;
using Optional;

namespace Application.Common.Interfaces.Queries;

public interface ICourseQueries
{
    Task<IReadOnlyList<Course>> GetAll(CancellationToken cancellationToken);
    Task<Option<Course>> GetById(CourseId id, CancellationToken cancellationToken);

}