using Api.Dtos;
using FluentValidation;

namespace Api.Modules.Validators;

public class CourseUserDtoValidator : AbstractValidator<CourseUserDto>
{
    public CourseUserDtoValidator()
    {
        RuleFor(x => x.CourseId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Raiting)
            .NotEmpty()
            .InclusiveBetween(0, 100)
            .WithMessage("Rating must be a number between 0 and 100.");
        RuleFor(x => x.JoinAt)
            .NotEmpty()
            .Must(BeUtc).WithMessage("FinishAt must be in UTC.");
        RuleFor(x => x.EndAt)
            .NotEmpty()
            .Must(BeUtc).WithMessage("FinishAt must be in UTC.");
        
    }
    private bool BeUtc(DateTime dateTime)
    {
        return dateTime.Kind == DateTimeKind.Utc;
    }
}