using System;

namespace Shops.Models;
public class Category
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]

    public string? Id { get; set; }
    public string? Name { get; set; }
    public Shop? shop { get; set; }
}
