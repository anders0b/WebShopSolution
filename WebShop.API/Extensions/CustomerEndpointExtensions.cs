﻿using Repository.Models;
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
                var customer = await customerService.GetCustomerById(id);
                return customer is not null ? Results.Ok(customer) : Results.NotFound();
            }
            return Results.NotFound();
        }

        public static async Task<IResult> RemoveCustomer(ICustomerService customerService, int id)
        {
            if (id != 0)
            {
                await customerService.RemoveCustomer(id);
                return Results.Ok($"Removed customer {id}");
            }
            return Results.NotFound();
        }

        public static async Task<IResult> UpdateCustomer(ICustomerService customerService, Customer customer)
        {
            if (customer != null)
            {
                await customerService.UpdateCustomer(customer);
                return Results.Ok($"Updated customer {customer}");
            }
            return Results.Problem();
        }

        public static async Task<IResult> UpdateCustomerEmail(ICustomerService customerService, int id, string email)
        {
            await customerService.UpdateCustomerEmail(id, email);
            return Results.Ok();
        }

        public static async Task<IResult> UpdateCustomerPhone(ICustomerService customerService, int id, string phone)
        {
            await customerService.UpdateCustomerPhone(id, phone);
            return Results.Ok();
        }
    }
}
