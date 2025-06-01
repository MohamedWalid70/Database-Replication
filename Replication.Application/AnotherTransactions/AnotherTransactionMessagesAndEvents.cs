using Replication.Domain.Entities.AnotherTransactionReplication;

namespace Replication.Application.AnotherTransactions;

public record AnotherTransactionAddedEvent(Guid transactionId, int transactionData);
public record AnotherTransactionDeletedEvent(Guid transactionId);
public record AnotherTransactionUpdatedEvent(Guid transactionId, int transactionData);
public record ReplicateAnotherTransaction(Guid id, int data);
public record ReplicateAnotherTransactionUpdate(Guid id,int data);
public record ReplicateAnotherTransactionDelete(Guid id);
public record AnotherTransactionReplicationSuccess(Guid id);
public record AnotherTransactionUpdateReplicationSuccess(Guid id);
public record AnotherTransactionDeleteReplicationSuccess(Guid id);
public record AnotherTransactionReplicationFailed(Guid id, int data);
public record AnotherTransactionUpdateReplicationFailed(Guid id, int data);
public record AnotherTransactionDeleteReplicationFailed(Guid id);
