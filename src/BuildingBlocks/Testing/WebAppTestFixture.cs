﻿using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Argo.MD.BuildingBlocks.Testing
{
    public class WebAppTestFixture<TEntryPoint> : IAsyncLifetime
        where TEntryPoint : class
    {
        public WebAppTestFixture()
        {
            Factory = new WebApplicationFactory<TEntryPoint>()
                .WithWebHostBuilder(builder =>
                {
                    builder.UseEnvironment("test");
                    builder.ConfigureAppConfiguration(AddCustomAppSettings);
                    builder.ConfigureServices(ConfigureCustomServices);
                });
        }

        public WebApplicationFactory<TEntryPoint> Factory { get; }

        public IServiceProvider ServiceProvider => Factory.Services;

        protected virtual void AddCustomAppSettings(IConfigurationBuilder configuration)
        {
        }

        protected virtual void ConfigureCustomServices(IServiceCollection services)
        {
        }

        public virtual Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        public virtual Task DisposeAsync()
        {
            return Task.CompletedTask;
        }

        protected async Task ExecuteScopeAsync(Func<IServiceProvider, Task> action)
        {
            using var scope = ServiceProvider.CreateScope();
            await action(scope.ServiceProvider);
        }

        public async Task<T> ExecuteScopeAsync<T>(Func<IServiceProvider, Task<T>> action)
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
