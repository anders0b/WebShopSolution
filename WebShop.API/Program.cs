using Microsoft.Data.SqlClient;
using Repository.Repository;
using System.Data;
using WebShop.API.Extensions;
using WebShop.Repository;
using WebShop.Repository.Notifications;
using WebShop.Repository.Repository;
using WebShop.Services.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var databaseConnectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new Exception("Not a valid connection");


builder.Services.AddScoped<IDbConnection>(sp =>
new SqlConnection(databaseConnectionString));

builder.Services.AddScoped<IDbTransaction>(provider =>
{
    var connection = provider.GetRequiredService<IDbConnection>();
    connection.Open();
    return connection.BeginTransaction();
});

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

// Registrera Unit of Work i DI-container
builder.Services.AddScoped<ProductSubject>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IProductServices, ProductServices>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddTransient<INotificationObserver, EmailNotification>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.TagActionsBy(d =>
    {
        return new List<string>() { d.ActionDescriptor.DisplayName! };
    });
});

var app = builder.Build();


// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

//migration of db, uncomment when ready
MigrationHelper.EnsureDatabaseIsAvailableAndUpToDate(databaseConnectionString, app.Logger);


app.UseHttpsRedirection();


app.MapProductEndpoints();
app.MapCustomerEndpoints();
app.MapOrderEndpoints();


app.Run();
