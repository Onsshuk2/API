using FluentValidation;

namespace pd311_web_api.BLL.DTOs.Role
{
    public class RoleDto
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
    }

    public class RoleValidator : AbstractValidator<RoleDto>
    {
        public RoleValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(256).WithMessage("Maximum length 256 symbols");
        }
    }
}
