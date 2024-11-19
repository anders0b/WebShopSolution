using Repository.Models;
using WebShop.Services.Services;

namespace WebShop.API.Extensions
{
    public static class ProductEndpointExtensions
    {
        public static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/products");

            group.MapPost("", async (IProductServices productService, Product product) =>
            {
                if (product != null)
                {
                    await productService.AddProduct(product);
                    return Results.Ok();
                }
                return Results.Problem();
            });
            group.MapGet("", async (IProductServices productService) =>
            {
                var products = await productService.GetAllProducts();
                return Results.Ok(products);
            });
            return app;
        }
    }
}
