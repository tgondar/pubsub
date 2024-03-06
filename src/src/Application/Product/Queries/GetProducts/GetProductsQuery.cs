using AutoMapper;
using AutoMapper.QueryableExtensions;
using EasyCaching.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using src.Application.Common.Interfaces;

namespace src.Application.Product.Queries.GetProducts;

public class GetProductsQuery : IRequest<List<ProductDto>>
{
    public string SearchString { get; set; }
}

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, List<ProductDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IHybridCachingProvider _cacheService;
    private readonly string _cacheKey = "products";

    public GetProductsQueryHandler(IApplicationDbContext context, IMapper mapper, IHybridCachingProvider cache)
    {
        _context = context;
        _mapper = mapper;
        _cacheService = cache;
    }

    public async Task<List<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var data = await _cacheService.GetAsync<List<ProductDto>>(_cacheKey, cancellationToken);

        if (data == null)
        {
            var products = await GetProducts(cancellationToken);

            await _cacheService.SetAsync(_cacheKey, products, TimeSpan.FromMinutes(2), cancellationToken);

            return products;
        }

        return GetFilteredProducts(data.Value, request);
    }

    private async Task<List<ProductDto>> GetProducts(CancellationToken cancellationToken)
    {
        return await _context.Product
            .AsNoTracking()
            .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
    }

    private static List<ProductDto> GetFilteredProducts(List<ProductDto> products, GetProductsQuery request)
    {
        return products
            .Where(x => x.Reference.Contains(request.SearchString)
                        || x.Designation.Contains(request.SearchString))
            .ToList();
    }
}
