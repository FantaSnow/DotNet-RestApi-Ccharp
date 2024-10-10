using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Courses.Exceptions;
using Application.CourseUsers.Exceptions;
using Domain.CoursesUsers;
using MediatR;

namespace Application.CourseUsers.Commands;

public record EndCourseUserCommand : IRequest<Result<CourseUser, CourseUserException>>
{
    public required Guid Id { get; init; }
    public required int Raiting { get; init;  }   

}

public class EndCourseUserCommandHandler(
    ICourseUserRepository courseUserRepository) : IRequestHandler<EndCourseUserCommand, Result<CourseUser, CourseUserException>>
{
    public async Task<Result<CourseUser, CourseUserException>> Handle(
        EndCourseUserCommand request,
        CancellationToken cancellationToken)
    {
        var courseUserId = new CourseUserId(request.Id);
        var courseUser = await courseUserRepository.GetById(courseUserId, cancellationToken);

        return await courseUser.Match(
            async f =>
                await UpdateRaitingEntity(f,request.Raiting, cancellationToken),
            () => Task.FromResult<Result<CourseUser, CourseUserException>>(new CourseUserNotFoundException(courseUserId)));
    }

    private async Task<Result<CourseUser, CourseUserException>> UpdateRaitingEntity(
        CourseUser course,
        int raiting,
        CancellationToken cancellationToken)
    {
        try
        {
            course.UpdateRaitingForEnd(raiting);

            return await courseUserRepository.Update(course, cancellationToken);
        }
        catch (Exception exception)
        {
            return new CourseUserUnknownException(course.Id, exception);
        }
    }

    
}