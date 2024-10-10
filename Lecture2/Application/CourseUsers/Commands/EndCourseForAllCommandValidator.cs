using FluentValidation;

namespace Application.CourseUsers.Commands;

public class EndCourseForAllCommandValidator : AbstractValidator<EndCourseUserCommand>
{
    public EndCourseForAllCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Raiting)
            .NotEmpty()
            .InclusiveBetween(0, 100)
            .WithMessage("Rating must be a number between 0 and 100.");
    }
}