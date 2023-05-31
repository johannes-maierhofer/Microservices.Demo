using MediatR;

namespace BuildingBlocks.Mediatr;

public interface IQuery<out T> : IRequest<T>
    where T : notnull
{
}
