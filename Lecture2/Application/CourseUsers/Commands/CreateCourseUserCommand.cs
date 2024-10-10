using Application.Common;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Courses.Commands;
using Application.Courses.Exceptions;
using Application.CourseUsers.Exceptions;
using Application.Faculties.Exceptions;
using Domain.Courses;
using Domain.CoursesUsers;
using Domain.Faculties;
using Domain.Users;
using MediatR;
using CourseNotFoundException = Application.CourseUsers.Exceptions.CourseNotFoundException;

namespace Application.CourseUsers.Commands;

public record CreateCourseUserCommand : IRequest<Result<CourseUser, CourseUserException>>
{
    public required Guid CourseId { get; init; }
    public required Guid UserId { get; init; }
    public required int Raiting { get; init;  }   
    public required DateTime JoinAt { get; init; }
    public required DateTime EndAt { get; init; }
}

public class CreateCourseUserCommandHandler(
    ICourseUserRepository courseUserRepository,
    ICourseUserQueries courseUserQueries,
    ICourseQueries courseQueries,
    IUserQueries userQueries)
    : IRequestHandler<CreateCourseUserCommand, Result<CourseUser, CourseUserException>>
{
    public async Task<Result<CourseUser, CourseUserException>> Handle(
        CreateCourseUserCommand request,
        CancellationToken cancellationToken)
    {
        
        var courseId = new CourseId(request.CourseId);
        var userId = new UserId(request.UserId);
        
        var existingCourseUser = await courseUserRepository.GetByCourseIdAndUserId(
            courseId, 
            userId, 
            cancellationToken);
        
        return await existingCourseUser.Match(
            cu => Task.FromResult<Result<CourseUser, CourseUserException>>(new CourseUserAlreadyExistsException(cu.Id)),
            async () =>
            {
                var courseResult = await courseQueries.GetById(courseId, cancellationToken);
                return await courseResult.Match(
                    async c =>
                    {
                        var userResult = await userQueries.GetById(userId, cancellationToken);
                        return await userResult.Match(
                            async u =>
                            {
                                var courseUsers = await courseUserQueries.GetAllCourses(courseId, cancellationToken);
                                var course = courseUsers.FirstOrDefault()?.Course;
                                if (course != null && courseUsers.Count >= course?.MaxStudents)
                                {
                                    return await Task.FromResult<Result<CourseUser, CourseUserException>>(new CourseMaxStudentsExceededException(course.Id));
                                }
                                return await CreateEntity(courseId, userId, request.Raiting, request.JoinAt, request.EndAt,
                                    cancellationToken);;

                            },
                            async () => await  Task.FromResult<Result<CourseUser, CourseUserException>>(new UserNotFoundException(userId)));
                    },
                    async () => await  Task.FromResult<Result<CourseUser, CourseUserException>>(new CourseNotFoundException(courseId))
                );
            });
    }
    
    private async Task<Result<CourseUser, CourseUserException>> CreateEntity(
        CourseId courseId, 
        UserId userId, 
        int raiting,
        DateTime joinAt, 
        DateTime endAt,
        CancellationToken cancellationToken)
    {
        try
        {
            var entity = CourseUser.New(CourseUserId.New(), courseId, userId, raiting, joinAt, endAt);
            return await courseUserRepository.Add(entity, cancellationToken);
        }
        catch (Exception exception)
        {
            return new CourseUserUnknownException(CourseUserId.Empty, exception);
        }
    }
}
