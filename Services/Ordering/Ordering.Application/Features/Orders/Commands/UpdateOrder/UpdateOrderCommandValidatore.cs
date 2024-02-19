using FluentValidation;
namespace Ordering.Application.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommandValidatore:AbstractValidator<UpdateOrderCommand>
    {
        public UpdateOrderCommandValidatore()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("{UserName} is required")
              .NotNull()
              .MaximumLength(50).WithMessage("{UserName} must not exceed 50 charecters");

            RuleFor(x => x.EmailAddress)
                .NotEmpty().WithMessage("{Email} is required");
            RuleFor(x => x.TotalPrice)
                .NotEmpty().WithMessage("{TotalPrice} is required")
                .GreaterThan(0).WithMessage("{TotalPrice} should be greater than 0");
        }
    }
}
