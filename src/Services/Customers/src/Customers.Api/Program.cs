using BuildingBlocks.Web;
using Customers;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false);

// Add services to the container.
builder.AddMinimalEndpoints(assemblies: typeof(CustomersRoot).Assembly);
builder.AddInfrastructure();

var app = builder.Build();

app.MapMinimalEndpoints();
//app.UseAuthentication();
//app.UseAuthorization();
app.UseInfrastructure();
app.UseDatabase();

app.Run();
