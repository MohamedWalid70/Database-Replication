
using Replication.Application.Abstractions.Messaging;

namespace Replication.Application.AnotherTransactions.Commands.Replicate;

public sealed record ReplicateAnotherTransactionCommand(Guid transactionId, int transactionData) : ICommand;
public sealed record ReplicateAnotherTransactionUpdateCommand(Guid transactionId, int transactionData) : ICommand;
public sealed record ReplicateAnotherTransactionDeleteCommand(Guid transactionId) : ICommand;
