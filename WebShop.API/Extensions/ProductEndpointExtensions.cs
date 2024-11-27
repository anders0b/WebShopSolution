using Repository.Models;
using WebShop.Services.Services;

namespace WebShop.API.Extensions
{
    public static class ProductEndpointExtensions
    {
        public static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/products").WithDisplayName("Product Management");
            group.MapPost("", AddProduct).WithSummary("Add product");
            group.MapGet("", GetAllProducts).WithSummary("Get all products");
            group.MapGet("{id}", GetProductById).WithSummary("Get product by id");
            group.MapDelete("{id}", RemoveProduct).WithSummary("Remove product");
            group.MapPatch("update-stock", UpdateStockQuantity).WithSummary("Update product stock quantity");
            group.MapPut("", UpdateProduct).WithSummary("Update product");
            group.MapGet("{orderId}/products", GetAllProductsFromOrder).WithSummary("Get all products from order id");
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
            var existingProduct = await productService.GetProductById(id);
            if (existingProduct.Id != 0)
            {
                return Results.Ok(existingProduct);
            }
            return Results.NotFound();
        }
        public static async Task<IResult> RemoveProduct(IProductServices productService, int id)
        {
            var existingProduct = await productService.GetProductById(id);
            if (existingProduct.Id != 0)
            {
                await productService.RemoveProduct(id);
                return Results.Ok($"Removed product {id}");
            }
            return Results.NotFound();
        }
        public static async Task<IResult> UpdateStockQuantity(IProductServices productService, int id, int stock)
        {
            var existingProduct = await productService.GetProductById(id);
            if (existingProduct.Id != 0)
            {
                await productService.UpdateStockQuantity(id, stock);
                return Results.Ok($"Updated stock quantity for product {id}");
            }
            return Results.Problem();
        }
        public static async Task<IResult> UpdateProduct(IProductServices productService, Product product)
        {
            var existingProduct = await productService.GetProductById(product.Id);
            if (existingProduct.Id != 0)
            {
                await productService.UpdateProduct(product);
                return Results.Ok($"Updated product {product}");
            }
            return Results.Problem("Please enter a valid product Id");
        }
        public static async Task<IResult> GetAllProductsFromOrder(IProductServices productService, int orderId)
        {
            var products = await productService.GetAllProductsFromOrder(orderId);
            return Results.Ok(products);
        }
    }
}
