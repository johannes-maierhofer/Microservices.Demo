using BuildingBlocks.Core.Domain;
using BuildingBlocks.Core.Domain.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BuildingBlocks.EfCore.Interceptors
{
    /// <summary>
    /// Dispatches domain events before DbContext saves changes.
    /// </summary>
    public class SavingChangesDomainEventDispatchingInterceptor : SaveChangesInterceptor
    {
        private readonly IDomainEventPublisher _domainEventPublisher;

        public SavingChangesDomainEventDispatchingInterceptor(IDomainEventPublisher domainEventPublisher)
        {
            _domainEventPublisher = domainEventPublisher;
        }

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            return SavingChangesAsync(eventData, result)
                .GetAwaiter()
                .GetResult();
        }

        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData, 
            InterceptionResult<int> result,
            CancellationToken cancellationToken = new CancellationToken())
        {
            if (eventData.Context == null) return result;

            var dbContext = eventData.Context;
            await DispatchDomainEvents(dbContext);
            
            return result;
        }

        private async Task DispatchDomainEvents(DbContext? context)
        {
            if (context == null) return;

            var entities = context.ChangeTracker
                .Entries<IHasDomainEvents>()
                .Where(e => e.Entity.DomainEvents.Any())
                .Select(e => e.Entity)
                .ToList();

            var domainEvents = entities
                .SelectMany(e => e.DomainEvents)
                .ToList();

            entities
                .ToList()
                .ForEach(e => e.ClearDomainEvents());

            foreach (var domainEvent in domainEvents)
                await _domainEventPublisher.Publish(domainEvent);
        }
    }
}
