using FluentValidation;

namespace TaskManager.Application.Validators.Roles;

public class RoleDtoValidator : AbstractValidator<RoleDto>
{
    public RoleDtoValidator()
    {
        RuleFor(x => x.RoleName)
            .NotEmpty().WithMessage("RoleName is required")
            .MaximumLength(30).WithMessage("RoleName must not exceed 30 characters");
    }
}
