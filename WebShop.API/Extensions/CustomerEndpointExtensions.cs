using Repository.Models;
using WebShop.Services.Services;

namespace WebShop.API.Extensions
{
    public static class CustomerEndpointExtensions
    {
        public static IEndpointRouteBuilder MapCustomerEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/customers").WithDisplayName("Customer Management");
            group.MapPost("", AddCustomer).WithSummary("Add Customer");
            group.MapGet("", GetAllCustomers).WithSummary("Get all customers");
            group.MapGet("{id}", GetCustomerById).WithSummary("Get customer by id");
            group.MapDelete("{id}", RemoveCustomer).WithSummary("Remove customer");
            group.MapPut("", UpdateCustomer).WithSummary("Update customer");
            group.MapPatch("update-email", UpdateCustomerEmail).WithSummary("Update customer email");
            group.MapPatch("update-phone", UpdateCustomerPhone).WithSummary("Update customer phone");
            return app;

        }
        public static async Task<IResult> AddCustomer(ICustomerService customerService, Customer customer)
        {
            if (customer != null)
            {
                await customerService.AddCustomer(customer);
                return Results.Ok($"Added customer {customer}");
            }
            return Results.Problem();
        }

        public static async Task<IResult> GetAllCustomers(ICustomerService customerService)
        {
            var customers = await customerService.GetAllCustomers();
            return Results.Ok(customers);
        }

        public static async Task<IResult> GetCustomerById(ICustomerService customerService, int id)
        {
            if (id != 0)
            {
                var existingCustomer = await customerService.GetCustomerById(id);
                if (existingCustomer != null)
                {
                    return Results.Ok(existingCustomer);
                }
            }
            return Results.BadRequest("Please enter a valid customer ID");
        }

        public static async Task<IResult> RemoveCustomer(ICustomerService customerService, int id)
        {
            if(id != 0)
            {
                var existingCustomer = await customerService.GetCustomerById(id);
                if (existingCustomer.Id != 0)
                {
                    await customerService.RemoveCustomer(id);
                    return Results.Ok($"Removed customer {id}");
                }
            }

            return Results.BadRequest("Customer not found");
        }

        public static async Task<IResult> UpdateCustomer(ICustomerService customerService, Customer customer)
        {
            var existingCustomer = await customerService.GetCustomerById(customer.Id);
            if (existingCustomer != null)
            {
                await customerService.UpdateCustomer(customer);
                return Results.Ok($"Updated customer {customer}");
            }

            return Results.BadRequest("Customer not found");
        }

        public static async Task<IResult> UpdateCustomerEmail(ICustomerService customerService, int id, string email)
        {
            var existingCustomer = await customerService.GetCustomerById(id);
            if(existingCustomer != null)
            {
                if (email.Contains('@') == false || email.Contains('.') == false)
                {
                    return Results.BadRequest("Email must contain @ and .");
                }
                if (email == null)
                {
                    return Results.BadRequest("Email cannot be null");
                }
                await customerService.UpdateCustomerEmail(id, email);
                return Results.Ok($"Customer Id: {id} new email: {email}");
            }
            return Results.BadRequest("Customer not found");
        }

        public static async Task<IResult> UpdateCustomerPhone(ICustomerService customerService, int id, string phone)
        {
            var existingCustomer = await customerService.GetCustomerById(id);
            if(existingCustomer != null)
            {
                if (phone.Length != 10)
                {
                    return Results.BadRequest("Phone number must be 10 digits");
                }
                if (!phone.All(char.IsDigit))
                {
                    return Results.BadRequest("Phone number must be digits only");
                }
                await customerService.UpdateCustomerPhone(id, phone);
                return Results.Ok($"Customer Id: {id} new phone number: {phone}");
            }
            return Results.BadRequest("Customer not found");
        }
    }
}
