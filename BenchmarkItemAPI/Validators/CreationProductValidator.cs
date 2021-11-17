﻿using BenchmarkItemAPI.Dtos;
using FluentValidation;

namespace BenchmarkItemAPI.Validators
{
    public class CreationProductValidator : AbstractValidator<CreationProduct>
    {
        public CreationProductValidator()
        {
            RuleFor(p => p.Code)
                .NotNull()
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(15);

            RuleFor(p => p.Name)
                .NotNull()
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(65);

            RuleFor(p => p.Price)
                .NotNull()
                .NotEmpty()
                .GreaterThanOrEqualTo(0);

            RuleFor(p => p.DiscountPrice)
                .GreaterThanOrEqualTo(0);
        }
    }
}
