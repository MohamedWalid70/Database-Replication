﻿using FluentResults;
using MediatR;

namespace Replication.Application.Abstractions.Messaging;
internal interface ICommandHandler<TCommand> : IRequestHandler<TCommand, Result>
    where TCommand : ICommand
{

}

internal interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>>
    where TCommand : ICommand<TResponse>
{

}
