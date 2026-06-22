using FluentValidation;
using TaskManager.Application.DTOs.Activity;

namespace TaskManager.Application.Validators.Activity;

public class ActivityLogCreateDtoValidator : AbstractValidator<ActivityLogCreateDto>
{
    public ActivityLogCreateDtoValidator()
    {
        RuleFor(x => x.ActionType)
            .NotEmpty().WithMessage("ActionType is required")
            .MaximumLength(50).WithMessage("ActionType must not exceed 50 characters");

        RuleFor(x => x.TaskId)
            .NotEqual(Guid.Empty).WithMessage("TaskId is required");
    }
}
