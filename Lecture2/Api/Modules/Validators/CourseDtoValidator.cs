using Api.Dtos;
using FluentValidation;

namespace Api.Modules.Validators;

public class CourseDtoValidator : AbstractValidator<CourseDto>
{
    public CourseDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(255)
            .MinimumLength(3);

        RuleFor(x => x.MaxStudents)
            .NotEmpty()
            .InclusiveBetween(0, 100)
            .WithMessage("MaxStudents must be a number between 0 and 100.");
            
        RuleFor(x => x.StartAt)
            .NotEmpty()
            .Must(BeUtc).WithMessage("StartAt must be in UTC.")
            .LessThan(x => x.FinishAt).WithMessage("StartAt must be earlier than FinishAt.");

        RuleFor(x => x.FinishAt)
            .NotEmpty()
            .Must(BeUtc).WithMessage("FinishAt must be in UTC.");
        
    }
    private bool BeUtc(DateTime dateTime)
    {
        return dateTime.Kind == DateTimeKind.Utc;
    }
}