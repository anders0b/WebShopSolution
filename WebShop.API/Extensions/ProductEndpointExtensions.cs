using Repository.Models;
using WebShop.Services.Services;

namespace WebShop.API.Extensions
{
    public static class ProductEndpointExtensions
    {
        public static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/products").WithDisplayName("Product Management");
            group.MapPost("", AddProduct);
            group.MapGet("", GetAllProducts);
            group.MapGet("{id}", GetProductById);
            group.MapDelete("{id}", RemoveProduct);
            group.MapPatch("update-stock", UpdateStockQuantity);
            group.MapPut("", UpdateProduct);
            group.MapGet("{orderId}/products", GetAllProductsFromOrder);
            return app;
        }
        public static async Task<IResult> AddProduct(IProductServices productService, Product product)
        {
            if (product != null)
            {
                await productService.AddProduct(product);
                return Results.Ok($"Added product {product}");
            }
            return Results.Problem();
        }
        public static async Task<IResult> GetAllProducts(IProductServices productService)
        {
            var products = await productService.GetAllProducts();
            return Results.Ok(products);
        }
        public static async Task<IResult> GetProductById(IProductServices productService, int id)
        {
            var product = await productService.GetProductById(id);
            if(product == null)
            {
                return Results.NotFound();
            }
            return Results.Ok(product);
        }
        public static async Task<IResult> RemoveProduct(IProductServices productService, int id)
        {
            if (id != 0)
            {
                await productService.RemoveProduct(id);
                return Results.Ok($"Removed product {id}");
            }
            return Results.NotFound();
        }
        public static async Task<IResult> UpdateStockQuantity(IProductServices productService, int id, int stock)
        {
            if(id != 0)
            {
                await productService.UpdateStockQuantity(id, stock);
                return Results.Ok($"Updated stock quantity for product {id}");
            }
            return Results.Problem();
        }
        public static async Task<IResult> UpdateProduct(IProductServices productService, Product product)
        {
            if (product != null)
            {
                await productService.UpdateProduct(product);
                return Results.Ok($"Updated product {product}");
            }
            return Results.Problem();
        }
        public static async Task<IResult> GetAllProductsFromOrder(IProductServices productService, int orderId)
        {
            var products = await productService.GetAllProductsFromOrder(orderId);
            return Results.Ok(products);
        }
    }
}
