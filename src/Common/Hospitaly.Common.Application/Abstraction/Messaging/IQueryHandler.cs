using ErrorOr;
using MediatR;

namespace Hospitaly.Common.Application.Abstraction.Messaging;

public interface IQueryHandler<in TQuery , TResponse> : IRequestHandler<TQuery,ErrorOr<TResponse>>
    where TQuery: IQuery<TResponse>;