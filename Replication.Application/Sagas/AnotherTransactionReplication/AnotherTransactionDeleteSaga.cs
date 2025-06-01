using Rebus.Bus;
using Rebus.Handlers;
using Rebus.Sagas;
using Replication.Application.AnotherTransactions;

namespace Replication.Application.Sagas.AnotherTransactionReplication
{
    internal class AnotherTransactionDeleteSaga : Saga<AnotherTransactionSagaData>,
        IAmInitiatedBy<AnotherTransactionDeletedEvent>,
        IHandleMessages<AnotherTransactionDeleteReplicationSuccess>,
        IHandleMessages<AnotherTransactionDeleteReplicationFailed>
    {
        private readonly IBus _bus;
        public AnotherTransactionDeleteSaga(IBus bus)
        {
            _bus = bus;
        }

        protected override void CorrelateMessages(ICorrelationConfig<AnotherTransactionSagaData> config)
        {
            config.Correlate<AnotherTransactionDeletedEvent>(e => e.transactionId, s => s.AnotherTransactionId);
            config.Correlate<AnotherTransactionDeleteReplicationSuccess>(e => e.id, s => s.AnotherTransactionId);
            config.Correlate<AnotherTransactionDeleteReplicationFailed>(e => e.id, s => s.AnotherTransactionId);

        }
        public async Task Handle(AnotherTransactionDeletedEvent message)
        {
            if (!IsNew)
                return;

            Data.AnotherTransactionReplicationStatus = TransactionReplicationStatusType.Pending;

            await _bus.Send(new ReplicateAnotherTransactionDelete(message.transactionId));
        }

        public async Task Handle(AnotherTransactionDeleteReplicationFailed message)
        {
            if (Data.AnotherTransactionReplicationRetries == 3)
            {
                Data.AnotherTransactionReplicationStatus = TransactionReplicationStatusType.Failed;
            }

            // Failure logic where retrying can occur

            if (Data.AnotherTransactionReplicationRetries > 0)
            {

                Data.TryReplication();

                await _bus.Send(new ReplicateAnotherTransactionDelete(message.id));
            }

            return;
        }

        public Task Handle(AnotherTransactionDeleteReplicationSuccess message)
        {
            Data.AnotherTransactionReplicationStatus = TransactionReplicationStatusType.Sent;

            MarkAsComplete();

            return Task.CompletedTask;
        }

    }
}
