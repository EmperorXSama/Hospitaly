using ErrorOr;
using MediatR;

namespace Hospitaly.Common.Application.Abstraction.Messaging;

public interface ICommandHandler<in TCommand , TResponse> : IRequestHandler<TCommand , ErrorOr<TResponse>>
where TCommand : ICommand<TResponse>;

public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand , ErrorOr<Success>>
where TCommand : ICommand;