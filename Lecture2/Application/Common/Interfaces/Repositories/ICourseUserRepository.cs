using Domain.Courses;
using Domain.CoursesUsers;
using Domain.Users;
using Optional;

namespace Application.Common.Interfaces.Repositories;

public interface ICourseUserRepository
{
    Task<Option<CourseUser>> GetById(CourseUserId id, CancellationToken cancellationToken);
    Task<CourseUser> Add(CourseUser course, CancellationToken cancellationToken);
    Task<CourseUser> Update(CourseUser course, CancellationToken cancellationToken);
    Task<CourseUser> Delete(CourseUser course, CancellationToken cancellationToken);
    Task<Option<CourseUser>> GetByCourseIdAndUserId(CourseId courseId, UserId userId, CancellationToken cancellationToken);
    Task<List<CourseUser>> UpdateAllCourses(List<CourseUser> courses, CancellationToken cancellationToken);
    
}