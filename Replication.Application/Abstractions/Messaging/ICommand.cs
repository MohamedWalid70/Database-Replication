using FluentResults;
using MediatR;

namespace Replication.Application.Abstractions.Messaging;

internal interface ICommand : IRequest<Result>
{
}

internal interface ICommand<TResponse> : IRequest<Result<TResponse>>
{

}

