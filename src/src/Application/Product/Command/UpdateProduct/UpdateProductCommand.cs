using MediatR;
using src.Application.Common.Exceptions;
using src.Application.Common.Interfaces;

namespace src.Application.Product.Command.UpdateProduct;
public class UpdateProductCommand : IRequest
{
    public UpdateProductCommand(int id, string reference, string designation, decimal price)
    {
        Id = id;
        Reference = reference;
        Designation = designation;
        Price = price;
    }

    public int Id { get; set; }
    public string Reference { get; set; }
    public string Designation { get; set; }
    public decimal Price { get; set; }
}

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateProductCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Product
            .FindAsync(new object[] { request.Id }, cancellationToken)
                ?? throw new NotFoundException(nameof(Domain.Entities.Products), request.Id);

        entity.Reference = request.Reference;
        entity.Designation = request.Designation;
        entity.Price = request.Price;
        entity.UpdateDate = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
