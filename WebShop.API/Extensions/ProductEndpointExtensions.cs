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
                if (id != 0)
                {
                    await productService.RemoveProduct(id);
                    return Results.Ok($"Removed product {id}");
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
            group.MapGet("{orderId}/products", async (IProductServices productService, int orderId) =>
            {
                var products = await productService.GetAllProductsFromOrder(orderId);
                return Results.Ok(products);
            });
            return app;
        }
    }
}
