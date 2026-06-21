using FluentValidation;

namespace TaskManager.Application.Validators.Tasks;

public class CreateTaskRequestValidator : AbstractValidator<CreateTaskRequest>
{
    public CreateTaskRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .MaximumLength(2000).WithMessage("Description must not exceed 2000 characters");

        RuleFor(x => x.Deadline)
            .GreaterThan(DateTime.UtcNow).WithMessage("Deadline must be in the future");

        RuleFor(x => x.UserId)
            .NotEqual(Guid.Empty).When(x => x.UserId.HasValue)
            .WithMessage("UserId must be a valid GUID");
    }
}
