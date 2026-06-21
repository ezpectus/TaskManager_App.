using FluentValidation;

namespace TaskManager.Application.Validators.Comments;

public class CreateCommentRequestValidator : AbstractValidator<CreateCommentRequest>
{
    public CreateCommentRequestValidator()
    {
        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Content is required")
            .MaximumLength(2000).WithMessage("Content must not exceed 2000 characters");

        RuleFor(x => x.TaskId)
            .NotEqual(Guid.Empty).WithMessage("TaskId is required");
    }
}
