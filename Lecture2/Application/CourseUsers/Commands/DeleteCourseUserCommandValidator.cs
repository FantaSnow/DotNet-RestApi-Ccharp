using FluentValidation;

namespace Application.CourseUsers.Commands;

public class DeleteCourseUserCommandValidator : AbstractValidator<DeleteCourseUserCommand>
{
    public DeleteCourseUserCommandValidator()
    {
        RuleFor(x => x.CourseUserId).NotEmpty();
    }
}