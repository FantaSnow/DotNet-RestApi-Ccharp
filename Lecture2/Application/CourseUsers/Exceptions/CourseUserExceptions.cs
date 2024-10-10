using Domain.Courses;
using Domain.CoursesUsers;
using Domain.Users;

namespace Application.CourseUsers.Exceptions;

public abstract class CourseUserException : Exception
{
    public CourseId? CourseId { get; }
    public CourseUserId? CourseUserId { get; }
    public UserId? UserId  { get; }
    protected CourseUserException(CourseId? courseId, string message, Exception? innerException = null)
        : base(message, innerException)
    {
        CourseId = courseId;
    }
    protected CourseUserException(CourseUserId? courseUserId, string message, Exception? innerException = null)
        : base(message, innerException)
    {
        CourseUserId = courseUserId;
    }
    
    protected CourseUserException(UserId? userId, string message, Exception? innerException = null)
        : base(message, innerException)
    {
        UserId = userId;
    }
}

public class CourseUserNotFoundException(CourseUserId id) : CourseUserException(id, $"CourseUser under id: {id} not found");

public class CourseUserAlreadyExistsException(CourseUserId id) : CourseUserException(id, $"CourseUser already exists: {id}");

public class CourseMaxStudentsExceededException(CourseId id) : CourseUserException( id, $"Course already full: {id}");

public class CourseNotFoundException(CourseId id) : CourseUserException( id, $"Course Not Found: {id}");

public class UserNotFoundException(UserId id) : CourseUserException( id, $"User Not Found: {id}");



public class CourseUserUnknownException(CourseUserId id, Exception innerException)
    : CourseUserException(id, $"Unknown exception for the Course under id: {id}", innerException);