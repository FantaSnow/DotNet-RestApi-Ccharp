using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Courses.Exceptions;
using Application.CourseUsers.Exceptions;
using Domain.Courses;
using Domain.CoursesUsers;
using MediatR;

namespace Application.CourseUsers.Commands;

public record DeleteCourseUserCommand : IRequest<Result<CourseUser, CourseUserException>>
{
    public required Guid CourseUserId { get; init; }
}

public class DeleteCourseUserCommandHandler(ICourseUserRepository courseUserRepository)
    : IRequestHandler<DeleteCourseUserCommand, Result<CourseUser, CourseUserException>>
{
    public async Task<Result<CourseUser, CourseUserException>> Handle(
        DeleteCourseUserCommand request,
        CancellationToken cancellationToken)
    {
        var courseUserId = new CourseUserId(request.CourseUserId);

        var existingCourseUser = await courseUserRepository.GetById(courseUserId, cancellationToken);

        return await existingCourseUser.Match<Task<Result<CourseUser, CourseUserException>>>(
            async u => await DeleteEntity(u, cancellationToken),
            () => Task.FromResult<Result<CourseUser, CourseUserException>>(new CourseUserNotFoundException(courseUserId)));
    }

    public async Task<Result<CourseUser, CourseUserException>> DeleteEntity(CourseUser courseUser, CancellationToken cancellationToken)
    {
        try
        {
            return await courseUserRepository.Delete(courseUser, cancellationToken);
        }
        catch (Exception exception)
        {
            return new CourseUserUnknownException(courseUser.Id, exception);
        }
    }
}