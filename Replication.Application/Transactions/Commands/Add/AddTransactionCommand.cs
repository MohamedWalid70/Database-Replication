using Replication.Application.Abstractions.Messaging;

namespace Replication.Application.Transactions.Commands.Add;

public sealed record AddTransactionCommand(string transactionData) : ICommand;
