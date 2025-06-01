using Replication.Application.Abstractions.Messaging;

namespace Replication.Application.AnotherTransactions.Commands.Update;

public sealed record UpdateAnotherTransactionCommand(Guid id, int data) : ICommand;
