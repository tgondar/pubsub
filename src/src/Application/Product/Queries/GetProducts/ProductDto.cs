using AutoMapper;
using src.Domain.Entities;

namespace src.Application.Product.Queries.GetProducts;
public class ProductDto
{
    public int Id { get; set; }
    public string Reference { get; set; }
    public string Designation { get; set; }
    public decimal Price { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime UpdateDate { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Products, ProductDto>();
        }
    }
}
