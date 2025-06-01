using FluentResults;
using MediatR;

namespace Replication.Application.Abstractions.Messaging
{
    internal interface IQueryHandler<TQuery,TResponse> : IRequestHandler<TQuery , Result<TResponse>> 
        where TQuery : IQuery<TResponse>
    {

    }
}
