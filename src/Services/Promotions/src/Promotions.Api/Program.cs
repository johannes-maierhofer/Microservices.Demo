using Argo.MD.BuildingBlocks.Web;
using Argo.MD.EmailSender.Messages.Commands;
using Argo.MD.Promotions;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddMinimalEndpoints(assemblies: typeof(PromotionsRoot).Assembly);
builder.AddInfrastructure();

// for using send we have to explicitly map endpoints
EndpointConvention.Map<SendEmail>(new Uri("queue:email-sender-service--send-email"));

var app = builder.Build();

app.MapMinimalEndpoints();
//app.UseAuthentication();
//app.UseAuthorization();
app.UseInfrastructure();

app.Run();