using Rebus.Sagas;

namespace Replication.Application.Sagas.ReplicationTransactions
{
    internal class TransactionCreateSagaData : ISagaData
    {
        public Guid Id { get; set; }
        public int Revision { get; set; }

        public Guid TransactionId { get; set; }
        public TransactionReplicationStatusType TransactionReplicationStatus { get; set; }

        public int TransactionReplicationRetries { get; set; } = 3;

    }
}
