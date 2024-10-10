using FluentValidation;

namespace Application.CourseUsers.Commands;

public class EndCourseUserCommandValidator : AbstractValidator<EndCourseUserCommand>
{
    public EndCourseUserCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Raiting)
            .NotEmpty()
            .InclusiveBetween(0, 100)
            .WithMessage("Rating must be a number between 0 and 100.");
    }
}