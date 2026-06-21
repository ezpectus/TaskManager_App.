using FluentValidation;

namespace TaskManager.Application.Validators.Tasks;

public class UpdateTaskRequestValidator : AbstractValidator<UpdateTaskRequest>
{
    public UpdateTaskRequestValidator()
    {
        When(x => x.Title != null, () =>
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title cannot be empty")
                .MaximumLength(200).WithMessage("Title must not exceed 200 characters");
        });

        When(x => x.Description != null, () =>
        {
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description cannot be empty")
                .MaximumLength(2000).WithMessage("Description must not exceed 2000 characters");
        });

        When(x => x.Deadline.HasValue, () =>
        {
            RuleFor(x => x.Deadline)
                .GreaterThan(DateTime.UtcNow).WithMessage("Deadline must be in the future");
        });
    }
}
