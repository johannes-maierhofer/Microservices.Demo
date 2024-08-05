using MediatR;

namespace Argo.MD.BuildingBlocks.Mediatr;

public interface IQuery<out T> : IRequest<T>
    where T : notnull
{
}
