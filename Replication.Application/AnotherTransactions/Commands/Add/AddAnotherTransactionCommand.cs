using Replication.Application.Abstractions.Messaging;

namespace Replication.Application.AnotherTransactions.Commands.Add;

public sealed record AddAnotherTransactionCommand(int data) : ICommand;
