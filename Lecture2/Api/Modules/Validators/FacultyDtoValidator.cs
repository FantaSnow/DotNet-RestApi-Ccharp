using Api.Dtos;
using FluentValidation;

namespace Api.Modules.Validators;

public class FacultyDtoValidator : AbstractValidator<FacultyDto>
{
    public FacultyDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(255).MinimumLength(3);
    }
}