using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Courses.Exceptions;
using Application.CourseUsers.Exceptions;
using Domain.Courses;
using Domain.CoursesUsers;
using Domain.Users;
using MediatR;
using Optional;

namespace Application.CourseUsers.Commands;

public record UpdateCourseUserCommand : IRequest<Result<CourseUser, CourseUserException>>
{
    public required Guid Id { get; init; }
    public required Guid CourseId { get; init; }
    public required Guid UserId { get; init; }
    public required int Raiting { get; init;  }   
    public required DateTime JoinAt { get; init; }
    public required DateTime EndAt { get; init; }
}

public class UpdateCourseUserCommandHandler(
    ICourseUserRepository courseUserRepository) : IRequestHandler<UpdateCourseUserCommand, Result<CourseUser, CourseUserException>>
{
    public async Task<Result<CourseUser, CourseUserException>> Handle(
        UpdateCourseUserCommand request,
        CancellationToken cancellationToken)
    {
        var courseId = new CourseUserId(request.Id);
        var course = await courseUserRepository.GetById(courseId, cancellationToken);

        return await course.Match(
            async f =>
            {
                var existingCourseUser = await CheckDuplicated(courseId, cancellationToken);

                return await existingCourseUser.Match(
                    ef => Task.FromResult<Result<CourseUser, CourseUserException>>(new CourseUserAlreadyExistsException(ef.Id)),
                    async () => await UpdateEntity(f, new CourseId(request.CourseId), new UserId(request.UserId) ,request.Raiting,request.JoinAt,request.EndAt, cancellationToken));
            },
            () => Task.FromResult<Result<CourseUser, CourseUserException>>(new CourseUserNotFoundException(courseId)));
    }

    private async Task<Result<CourseUser, CourseUserException>> UpdateEntity(
        CourseUser course,
        CourseId courseId,
        UserId userId,
        int raiting,
        DateTime joinAt,
        DateTime endAt,
        CancellationToken cancellationToken)
    {
        try
        {
            course.UpdateDetails(courseId,userId, raiting, joinAt,endAt);

            return await courseUserRepository.Update(course, cancellationToken);
        }
        catch (Exception exception)
        {
            return new CourseUserUnknownException(course.Id, exception);
        }
    }

    private async Task<Option<CourseUser>> CheckDuplicated(
        CourseUserId courseId,
        CancellationToken cancellationToken)
    {
        var course = await courseUserRepository.GetById(courseId, cancellationToken);

        return course.Match(
            f => f.Id == courseId ? Option.None<CourseUser>() : Option.Some(f), 
            Option.None<CourseUser>);
    }
}