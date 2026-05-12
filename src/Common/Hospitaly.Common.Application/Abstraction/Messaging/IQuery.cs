using ErrorOr;
using MediatR;

namespace Hospitaly.Common.Application.Abstraction.Messaging;

public interface IQuery<TResponse>: IRequest<ErrorOr<TResponse>>;