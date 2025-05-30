namespace Replication.Application.Transactions;

public record TransactionAddedEvent(Guid transactionId, string transactionData);
public record SendTransactionData(Guid transactionId, string transactionData);
public record TransactionDataReplicatedEvent(Guid transactionId);
public record TransactionReplicationFailedEvent(Guid transactionId, string transactionData);
