using Repository.Models;
using WebShop.Services.Services;

namespace WebShop.API.Extensions
{
    public static class CustomerEndpointExtensions
    {
        public static IEndpointRouteBuilder MapCustomerEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/customers").WithDisplayName("Customer Management");

            group.MapGet("", async (ICustomerService customerService) =>
            {
                var customers = await customerService.GetAllCustomers();
                return Results.Ok(customers);
            });

            group.MapGet("{Id}", async (ICustomerService customerService, int Id) =>
            {
                if(Id != 0)
                {
                    var customer = await customerService.GetCustomerById(Id);
                    return customer is not null ? Results.Ok(customer) : Results.NotFound();
                }
                return Results.Problem();
            });
            group.MapPost("", async (ICustomerService customerService, Customer customer) =>
            {
                if (customer != null)
                {
                    await customerService.AddCustomer(customer);
                    return Results.Ok($"Added customer {customer}");
                }
                return Results.Problem();
            });
            group.MapDelete("{Id}", async (ICustomerService customerService, int Id) =>
            {
                var customer = await customerService.GetCustomerById(Id);
                if (customer is not null)
                {
                    await customerService.RemoveCustomer(customer);
                    return Results.Ok($"Removed customer {customer}");
                }
                return Results.NotFound();
            });
            group.MapPut("", async (ICustomerService customerService, Customer customer) =>
            {
                if (customer != null)
                {
                    await customerService.UpdateCustomer(customer);
                    return Results.Ok($"Updated customer {customer}");
                }
                return Results.Problem();
            });
            group.MapPatch("update-email", async (ICustomerService customerService, int Id, string email) =>
            {
                await customerService.UpdateCustomerEmail(Id, email);
            });
            group.MapPatch("update-phone", async (ICustomerService customerService, int Id, string phone) =>
            {
                await customerService.UpdateCustomerPhone(Id, phone);
            });
            return app;
        }
    }
}
