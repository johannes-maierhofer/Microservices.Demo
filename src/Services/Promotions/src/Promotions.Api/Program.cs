using BuildingBlocks.Contracts.Messages;
using BuildingBlocks.Web;
using MassTransit;
using Promotions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddMinimalEndpoints(assemblies: typeof(PromotionsRoot).Assembly);
builder.AddInfrastructure();

// for using send we have to explicitly map endpoints
EndpointConvention.Map<EmailSenderContracts.SendEmail>(new Uri("queue:email-sender-service--send-email"));

var app = builder.Build();

app.MapMinimalEndpoints();
//app.UseAuthentication();
//app.UseAuthorization();
app.UseInfrastructure();

app.Run();