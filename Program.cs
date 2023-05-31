var builder = WebApplication.CreateBuilder(args);

var monogoSetting = builder.Configuration.GetSection("MongoConnection");
builder.Services.Configure<DatabaseSettings>(monogoSetting);

builder.Services.AddTransient<IMongoContext, MongoContext>();
builder.Services.AddTransient<IShopRepository, ShopRepository>();
builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();
builder.Services.AddTransient<IProductRepository, ProductRepository>();
builder.Services.AddTransient<IBasketRepository, BasketRepository>();

builder.Services.AddTransient<IShopService, ShopService>();

builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();


// swagger builder service
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var authenticationSettings = builder.Configuration.GetSection("AuthenticationSettings");
builder.Services.Configure<AuthenticationSettings>(authenticationSettings);

builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<ProductValidator>());

builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CategoryValidator>());

builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<ShopValidator>());

builder.Services.AddAuthorization(options => { });

builder.Services.AddAuthentication("Bearer").AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["AuthenticationSettings:Issuer"],
            ValidAudience = builder.Configuration["AuthenticationSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(builder.Configuration["AuthenticationSettings:SecretForKey"]))
        };
    }
);

builder.Services
    .AddGraphQLServer()
    .AddQueryType<Querries>()
    .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = true)
    .AddMutationType<Mutation>();

var app = builder.Build();

app.MapSwagger();
app.UseSwaggerUI();

// GraphQL
app.MapGraphQL();
// http://localhost:3000/graphql/
// authenticatie
app.UseAuthentication();
app.UseAuthorization();

// var app = builder.Build();
// app.MapGet("/helloworld", () => "Hello World");

app.MapGet("/", () => "Hello World!");

//[Authorize]
app.MapGet("/setup", async (IShopService ShopService) => await ShopService.SetupDummyData());
//[Authorize]
app.MapGet("/shops", async (IShopService ShopService) => await ShopService.GetShops());
//[Authorize]
app.MapGet("/categorys", async (IShopService ShopService) => {
    var results = await ShopService.GetCategories();
    return Results.Ok(results);
});


//[Authorize]
app.MapGet("/products", async (IShopService ShopService) => {
    try{
        var results= await ShopService.GetProducts();
        return Results.Ok(results);
    }
    catch(Exception ex){

        return Results.Ok(ex);
    }
});
//[Authorize]
app.MapGet("/shop/{id}", async (IShopService ShopService, string id) => await ShopService.GetShopById(id));
//[Authorize]
app.MapGet("/category/{id}", async (IShopService ShopService, string id) => await ShopService.GetCategory(id));
//[Authorize]
app.MapGet("/product/{id}", async (IShopService ShopService, string id) => await ShopService.GetProduct(id));

app.MapDelete("/shop/{id}", async (IShopService ShopService, string id) => await ShopService.DeleteShop(id));


//[Authorize]
app.MapPost("/products", async (IValidator<Product> validator, IShopService ShopService, Product product) =>
{
    var result = validator.Validate(product);
    if (result.IsValid)
    {
        var created = await ShopService.AddProduct(product);
        return Results.Created($"/products/{created.Id}", created);
    }
    else
    {
        var errors = result.Errors.Select(e => new { errors = e.ErrorMessage });
        return Results.BadRequest(errors);
    }
});
//[Authorize]
app.MapPost("/categorys", async (IValidator<Category> validator, IShopService ShopService, Category category) =>
{
    var result = validator.Validate(category);
    if (result.IsValid)
    {
        var created = await ShopService.AddCategory(category);
        return Results.Created($"/categorys/{created.Id}", created);
    }
    else
    {
        var errors = result.Errors.Select(e => new { errors = e.ErrorMessage });
        return Results.BadRequest(errors);
    }
});
//[Authorize]
app.MapPost("/shops", async (IValidator<Shop> validator, IShopService ShopService, Shop shop) =>
{
    var result = validator.Validate(shop);
    if (result.IsValid)
    {
        var created = await ShopService.AddShop(shop);
        return Results.Created($"/shops/{created.Id}", created);
    }
    else
    {
        var errors = result.Errors.Select(e => new { errors = e.ErrorMessage });
        return Results.BadRequest(errors);
    }
});

app.MapPost("/basket", async (IShopService shopService, Basket basket) =>
{
    var result = await shopService.AddBasket(basket);
    return Results.Created($"/order/{result.BasketId}", result);
});


app.MapPost("/authenticate",
(IAuthenticationService service, AuthenticationRequestBody auth) =>
{
    var resp = service.Authenticate(auth);
    if (resp is null)
    {
        return Results.Unauthorized();
    }
    else
    {
        return Results.Ok(resp);
    }
});

app.MapPut("/product/{productId}", async (IShopService shopService, string productId, Product product) => await shopService.UpdateProduct(productId, product));

app.MapGet("/test", async (IShopService ShopService) => 
{
    try{
        throw new ArgumentException();
    }
    catch(System.Exception ex)
    {
        await WriteNewCsvFile("/test.csv", ex.InnerException.ToString());
    }
});



//GraphQL.SchemaExtensions.RegisterTypeMapping<Brand,BrandType>();
// Er mag maar 1 app.Run zijn voor dit te testen daarom dat dit in commentaar moet!!!
// app.Run("http://0.0.0.0:3000");
//http://localhost:3000/Swagger/index.html?fbclid=IwAR2_gvtz3niteXbG4xVqXhC1nNQZI-EFuHhDV5D1QWBpriI5GUinJGvAH3c

app.Run();
//Hack om testen te doen werken 
public partial class Program { }
