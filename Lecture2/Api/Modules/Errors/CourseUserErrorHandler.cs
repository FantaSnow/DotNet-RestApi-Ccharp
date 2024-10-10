using Application.Courses.Exceptions;
using Application.CourseUsers.Exceptions;
using Application.Faculties.Exceptions;
using Microsoft.AspNetCore.Mvc;
using CourseNotFoundException = Application.CourseUsers.Exceptions.CourseNotFoundException;

namespace Api.Modules.Errors;

public static class CourseUserErrorHandler
{
    public static ObjectResult ToObjectResult(this CourseUserException exception)
    {
        return new ObjectResult(exception.Message)
        {
            StatusCode = exception switch
            {
                CourseUserNotFoundException => StatusCodes.Status404NotFound,
                CourseUserAlreadyExistsException => StatusCodes.Status409Conflict,
                CourseUserUnknownException => StatusCodes.Status500InternalServerError,
                CourseMaxStudentsExceededException => StatusCodes.Status503ServiceUnavailable,
                UserNotFoundException => StatusCodes.Status404NotFound,
                CourseNotFoundException => StatusCodes.Status404NotFound,
                _ => throw new NotImplementedException("CourseUser error handler does not implemented")
            }
        };
    }
}