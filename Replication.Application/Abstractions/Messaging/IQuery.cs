using FluentResults;
using MediatR;

namespace Replication.Application.Abstractions.Messaging
{
    internal interface IQuery<TResponse> : IRequest<Result<TResponse>>
    {

    }
}
