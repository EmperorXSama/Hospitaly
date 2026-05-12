using ErrorOr;
using MediatR;

namespace Hospitaly.Common.Application.Abstraction.Messaging;

public interface ICommand : IRequest<ErrorOr<Success>>, IBaseCommand;
public interface ICommand<TResponse> : IBaseCommand, IRequest<ErrorOr<TResponse>>;
public interface IBaseCommand;