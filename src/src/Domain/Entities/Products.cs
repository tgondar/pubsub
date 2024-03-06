namespace src.Domain.Entities;

public class Products
{
    public int Id { get; set; }
    public string Reference { get; set; }
    public string Designation { get; set; }
    public decimal Price { get; set; }
    public DateTime CreateDate{ get; set; }
    public DateTime UpdateDate{ get; set; }
}
