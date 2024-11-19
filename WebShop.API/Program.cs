using Repository.Repository;
using WebShop.Notifications;
using WebShop.Repository;
using WebShop.Repository.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var databaseConnectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new Exception("Not a valid connection");

builder.Services.AddControllers();
// Registrera Unit of Work i DI-container
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<INotificationObserver, EmailNotification>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.UseAuthorization();

app.MapControllers();

app.Run();
