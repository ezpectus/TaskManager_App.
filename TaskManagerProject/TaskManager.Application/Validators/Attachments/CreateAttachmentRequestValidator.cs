using FluentValidation;

namespace TaskManager.Application.Validators.Attachments;

public class CreateAttachmentRequestValidator : AbstractValidator<CreateAttachmentRequest>
{
    public CreateAttachmentRequestValidator()
    {
        RuleFor(x => x.FileName)
            .NotEmpty().WithMessage("FileName is required")
            .MaximumLength(100).WithMessage("FileName must not exceed 100 characters");

        RuleFor(x => x.Url)
            .NotEmpty().WithMessage("Url is required");

        RuleFor(x => x.TaskId)
            .NotEqual(Guid.Empty).WithMessage("TaskId is required");
    }
}
