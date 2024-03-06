using FluentValidation;
using Microsoft.EntityFrameworkCore;
using src.Application.Common.Interfaces;

namespace src.Application.Product.Command.UpdateProduct;
public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateProductCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Reference)
            .NotEmpty().WithMessage("Reference is required.")
            .MaximumLength(200).WithMessage("Reference must not exceed 200 characters.")
            .MustAsync(BeUniqueTitle).WithMessage("The specified Reference already exists.");
    }

    public async Task<bool> BeUniqueTitle(UpdateProductCommand model, string reference, CancellationToken cancellationToken)
    {
        return await _context.Product
                        .Where(l => l.Id != model.Id)
                        .AllAsync(l => l.Reference != reference, cancellationToken);
    }
}
