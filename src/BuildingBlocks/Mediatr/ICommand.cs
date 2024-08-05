using MediatR;

namespace Argo.MD.BuildingBlocks.Mediatr;

public interface ICommand : ICommand<Unit>
{
}

public interface ICommand<out T> : IRequest<T>
    where T : notnull
{
}
