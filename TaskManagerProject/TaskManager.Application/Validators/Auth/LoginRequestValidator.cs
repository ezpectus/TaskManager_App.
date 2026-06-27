using FluentValidation;
using System.Text.RegularExpressions;
using TaskManager.Application.DTOs.Auth;

namespace TaskManager.Application.Validators.Auth;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    private static readonly Regex EmailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);
    private static readonly Regex UsernameRegex = new(@"^[a-zA-Z0-9_.\-]{3,50}$", RegexOptions.Compiled);

    public LoginRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email or username is required")
            .Must(x => EmailRegex.IsMatch(x) || UsernameRegex.IsMatch(x))
            .WithMessage("Must be a valid email address or username (3-50 chars, alphanumeric, dots, underscores, hyphens)");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required");
    }
}
