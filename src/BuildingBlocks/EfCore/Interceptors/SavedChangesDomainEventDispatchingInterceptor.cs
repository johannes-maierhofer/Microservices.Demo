using BuildingBlocks.Core.Domain;
using BuildingBlocks.Core.Domain.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BuildingBlocks.EfCore.Interceptors
{
    /// <summary>
    /// Dispatches domain events after DbContext saved changes.
    /// </summary>
    public class SavedChangesDomainEventDispatchingInterceptor : SaveChangesInterceptor
    {
        private readonly IDomainEventPublisher _domainEventPublisher;

        public SavedChangesDomainEventDispatchingInterceptor(IDomainEventPublisher domainEventPublisher)
        {
            _domainEventPublisher = domainEventPublisher;
        }

        public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
        {
            return SavedChangesAsync(eventData, result)
                .GetAwaiter()
                .GetResult();
        }

        public override async ValueTask<int> SavedChangesAsync(
            SaveChangesCompletedEventData eventData, 
            int result,
            CancellationToken cancellationToken = new())
        {
            if (eventData.Context == null) return result;

            var dbContext = eventData.Context;
            await DispatchDomainEvents(dbContext);
            
            // save again if domain events caused data to change
            // TODO: not sure if this is a good idea (Interceptor calling SaveChanges again)
            if(dbContext.ChangeTracker.HasChanges())
                await eventData.Context.SaveChangesAsync(cancellationToken);

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
