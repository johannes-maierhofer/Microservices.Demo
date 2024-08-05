using Argo.MD.BuildingBlocks.Web;
using Argo.MD.Customers.Api;

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

namespace Argo.MD.Customers.Api
{
    public partial class Program
    {
    }
}
