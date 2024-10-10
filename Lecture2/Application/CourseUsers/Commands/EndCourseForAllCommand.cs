using Application.Common;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Courses.Exceptions;
using Application.CourseUsers.Exceptions;
using Domain.Courses;
using Domain.CoursesUsers;
using MediatR;

namespace Application.CourseUsers.Commands;

public record EndCourseForAllCommand : IRequest<List<CourseUser>>
{
    public required Guid Id { get; init; }
    
}

public class EndCourseForAllCommandHandler(
    ICourseUserRepository courseUserRepository , ICourseUserQueries courseUserQueries) : IRequestHandler<EndCourseForAllCommand, List<CourseUser>>
{
    public async Task<List<CourseUser>> Handle(
        EndCourseForAllCommand request,
        CancellationToken cancellationToken)
    {
        var courseId = new CourseId(request.Id);
        var course = await courseUserQueries.GetAllCourses(courseId, cancellationToken);

        course.ForEach(cours => cours.EndCourseForAll());
        
        var coursessss = await courseUserRepository.UpdateAllCourses(course, cancellationToken);
        return coursessss;
    }
    
}