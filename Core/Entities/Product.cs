using System;

namespace Core.Entities;

public class Product: BaseEntity
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public decimal Price { get; set; }
    public required string PictureUrl { get; set; }
    public required string ProductType { get; set; }
    public required string ProductBrand { get; set; }
    public required int QuantityInStock { get; set; }

}
