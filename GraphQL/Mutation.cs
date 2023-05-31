namespace Shops.GraphQl.Mutation;
public class Mutation
{

    public async Task<AddCategoryPayload> AddCategory([Service] IShopService shopService, AddCategoryInput input)
    {
        var newCategory = new Category()
        {
            Name = input.name
        };
        var created = await shopService.AddCategory(newCategory);
        return new AddCategoryPayload(created);
    }

    //     mutation AddCategory
    //     {
    //         addCategory(input: { name: "123456789"}) {
    //     category{
    //       name
    //     }
    //   }
    // }


}