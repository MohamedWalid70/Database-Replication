using Replication.Application.Abstractions.Messaging;

namespace Replication.Application.AnotherTransactions.Commands.Delete;

public sealed record DeleteAnotherTransactionCommand(Guid id) : ICommand;

