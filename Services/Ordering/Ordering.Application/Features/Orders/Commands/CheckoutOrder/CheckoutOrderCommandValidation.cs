﻿using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder
{
    public class CheckoutOrderCommandValidation:AbstractValidator<CheckoutOrderCommand>
    {
        public CheckoutOrderCommandValidation()
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
