using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BuildingBlocks.Testing
{
    public class TestFixture<TEntryPoint> : IAsyncLifetime
        where TEntryPoint : class
    {
        private readonly WebApplicationFactory<TEntryPoint> _factory;

        public TestFixture()
        {
            _factory = new WebApplicationFactory<TEntryPoint>()
                .WithWebHostBuilder(builder =>
                {
                    //builder.ConfigureAppConfiguration(AddCustomAppSettings);

                    builder.UseEnvironment("test");
                    //builder.ConfigureServices(services =>
                    //{
                        //    TestRegistrationServices?.Invoke(services);
                        //    services.ReplaceSingleton(AddHttpContextAccessorMock);

                        //    // add authentication using a fake jwt bearer - we can use SetAdminUser method to set authenticate user to existing HttContextAccessor
                        //    // https://github.com/webmotions/fake-authentication-jwtbearer
                        //    // https://github.com/webmotions/fake-authentication-jwtbearer/issues/14
                        //    //services.AddAuthentication(options =>
                        //    //{
                        //    //    options.DefaultAuthenticateScheme = FakeJwtBearerDefaults.AuthenticationScheme;
                        //    //    options.DefaultChallengeScheme = FakeJwtBearerDefaults.AuthenticationScheme;
                        //    //}).AddFakeJwtBearer();
                    //});
                });
        }

        public IServiceProvider ServiceProvider => _factory.Services;

        public Task InitializeAsync()
        {
            // throw new NotImplementedException();
            return Task.CompletedTask;
        }

        public Task DisposeAsync()
        {
            // throw new NotImplementedException();
            return Task.CompletedTask;
        }

        protected async Task ExecuteScopeAsync(Func<IServiceProvider, Task> action)
        {
            using var scope = ServiceProvider.CreateScope();
            await action(scope.ServiceProvider);
        }

        protected async Task<T> ExecuteScopeAsync<T>(Func<IServiceProvider, Task<T>> action)
        {
            using var scope = ServiceProvider.CreateScope();

            var result = await action(scope.ServiceProvider);

            return result;
        }

        public Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            return ExecuteScopeAsync(sp =>
            {
                var mediator = sp.GetRequiredService<IMediator>();

                return mediator.Send(request);
            });
        }
    }
}
