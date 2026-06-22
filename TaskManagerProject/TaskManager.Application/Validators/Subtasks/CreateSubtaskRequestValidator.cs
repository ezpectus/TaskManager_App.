using FluentValidation;
using TaskManager.Application.DTOs.Subtasks;

namespace TaskManager.Application.Validators.Subtasks;

public class CreateSubtaskRequestValidator : AbstractValidator<CreateSubtaskRequest>
{
    public CreateSubtaskRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters");

        RuleFor(x => x.TaskId)
            .NotEqual(Guid.Empty).WithMessage("TaskId is required");
    }
}
