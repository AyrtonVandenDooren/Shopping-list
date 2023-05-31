using System;

namespace Shops.Models;
public class Product
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]

    public string? Id { get; set; }
    public string? Name { get; set; }
    public int Stock { get; set; }
    public Category? category{get; set;}
}
