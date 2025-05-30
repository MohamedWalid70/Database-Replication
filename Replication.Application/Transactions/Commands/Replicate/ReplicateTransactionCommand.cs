
using Replication.Application.Abstractions.Messaging;

namespace Replication.Application.Transactions.Commands.Replicate;

public sealed record ReplicateTransactionCommand(Guid transactionId, string transactionData) : ICommand;
