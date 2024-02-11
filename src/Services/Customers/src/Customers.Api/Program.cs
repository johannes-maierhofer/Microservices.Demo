using BuildingBlocks.Web;
using Customers.Api;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddMinimalEndpoints(assemblies: typeof(CustomersApiRoot).Assembly);
builder.AddInfrastructure();

var app = builder.Build();

app.MapMinimalEndpoints();
//app.UseAuthentication();
//app.UseAuthorization();
app.UseInfrastructure();
app.UseDatabase();

app.Run();

namespace Customers.Api
{
    public partial class Program
    {
    }
}
