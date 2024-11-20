using Repository.Models;
using WebShop.Services.Services;

namespace WebShop.API.Extensions
{
    public static class ProductEndpointExtensions
    {
        public static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/products").WithDisplayName("Product Management");

            group.MapPost("", async (IProductServices productService, Product product) =>
            {
                if (product != null)
                {
                    await productService.AddProduct(product);
                    return Results.Ok($"Added product {product}");
                }
                return Results.Problem();
            });

            group.MapGet("", async (IProductServices productService) =>
            {
                var products = await productService.GetAllProducts();
                return Results.Ok(products);
            });

            group.MapGet("{id}", async (IProductServices productService, int id) =>
            {
                var product = await productService.GetProductById(id);
                return product is not null ? Results.Ok(product) : Results.NotFound();
            });

            group.MapDelete("{id}", async (IProductServices productService, int id) =>
            {
                var product = await productService.GetProductById(id);
                if (product is not null)
                {
                    await productService.RemoveProduct(product);
                    return Results.Ok($"Removed product {product}");
                }
                return Results.NotFound();
            });

            group.MapPatch("update-stock", async (IProductServices productService, int id, int stock) =>
            {
                await productService.UpdateStockQuantity(id, stock);
            });

            group.MapPut("", async (IProductServices productService, Product product) =>
            {
                if (product != null)
                {
                    await productService.UpdateProduct(product);
                    return Results.Ok($"Updated product {product}");
                }
                return Results.Problem();
            });
            return app;
        }
    }
}
