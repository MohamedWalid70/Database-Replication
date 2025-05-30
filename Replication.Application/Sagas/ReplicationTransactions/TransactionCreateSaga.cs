using Microsoft.Extensions.Logging;
using Rebus.Bus;
using Rebus.Handlers;
using Rebus.Sagas;
using Replication.Application.Transactions;

namespace Replication.Application.Sagas.ReplicationTransactions
{
    internal class TransactionCreateSaga : Saga<TransactionCreateSagaData>, 
        IAmInitiatedBy<TransactionAddedEvent>, 
        IHandleMessages<TransactionDataReplicatedEvent>,
        IHandleMessages<TransactionReplicationFailedEvent>
    {

        private readonly IBus _bus;

        public TransactionCreateSaga(IBus bus)
        {
            _bus = bus;
        }

        protected override void CorrelateMessages(ICorrelationConfig<TransactionCreateSagaData> config)
        {
            config.Correlate<TransactionAddedEvent>(m => m.transactionId, s => s.TransactionId);
            config.Correlate<TransactionDataReplicatedEvent>(m => m.transactionId, s => s.TransactionId);
            config.Correlate<TransactionReplicationFailedEvent>(m => m.transactionId, s => s.TransactionId);
        }

        public async Task Handle(TransactionAddedEvent message)
        {
            if (!IsNew)
            {
                return;
            }

            Data.TransactionReplicationStatus = TransactionReplicationStatusType.Pending;

            await _bus.Send(new SendTransactionData(message.transactionId, message.transactionData)); 
        }

        public Task Handle(TransactionDataReplicatedEvent message)
        {
            Data.TransactionReplicationStatus = TransactionReplicationStatusType.Sent;

            MarkAsComplete();

            return Task.CompletedTask;
        }

        public async Task Handle(TransactionReplicationFailedEvent message)
        {
            if (Data.TransactionReplicationRetries == 3)
            {
                Data.TransactionReplicationStatus = TransactionReplicationStatusType.Failed;
            }

            // Failure logic where retrying can occur

            if (Data.TransactionReplicationRetries > 0)
            {

                Data.TransactionReplicationRetries--;

                await _bus.Send(new SendTransactionData(message.transactionId, message.transactionData));

                return;
            }

        }
    }
}
