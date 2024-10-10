using Domain.Courses;
using Domain.CoursesUsers;
using Optional;

namespace Application.Common.Interfaces.Queries;

public interface ICourseUserQueries
{
    Task<IReadOnlyList<CourseUser>> GetAll(CancellationToken cancellationToken);
    Task<List<CourseUser>> GetAllCourses(CourseId courseId, CancellationToken cancellationToken);
    Task<Option<CourseUser>> GetById(CourseUserId id, CancellationToken cancellationToken);
}