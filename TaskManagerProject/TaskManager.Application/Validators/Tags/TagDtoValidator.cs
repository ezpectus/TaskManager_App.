using FluentValidation;
using TaskManager.Application.DTOs.Tags;

namespace TaskManager.Application.Validators.Tags;

public class TagDtoValidator : AbstractValidator<TagDto>
{
    public TagDtoValidator()
    {
        RuleFor(x => x.TagName)
            .NotEmpty().WithMessage("TagName is required")
            .MaximumLength(50).WithMessage("TagName must not exceed 50 characters");

        RuleFor(x => x.TagColor)
            .NotEmpty().WithMessage("TagColor is required")
            .Matches("^#[0-9A-Fa-f]{6}$").WithMessage("TagColor must be a valid HEX color code");
    }
}
