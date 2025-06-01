using Rebus.Bus;
using Rebus.Handlers;
using Rebus.Sagas;
using Replication.Application.AnotherTransactions;
using Replication.Application.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Replication.Application.Sagas.AnotherTransactionReplication
{
    internal class AnotherTransactionCreateSaga : Saga<AnotherTransactionSagaData>,
        IAmInitiatedBy<AnotherTransactionAddedEvent>,
        IHandleMessages<AnotherTransactionReplicationSuccess>,
        IHandleMessages<AnotherTransactionReplicationFailed>
    {

        private readonly IBus _bus;
        protected override void CorrelateMessages(ICorrelationConfig<AnotherTransactionSagaData> config)
        {
            config.Correlate<AnotherTransactionAddedEvent>(e => e.transactionId, s => s.AnotherTransactionId);
            config.Correlate<AnotherTransactionReplicationFailed>(e => e.id, s => s.AnotherTransactionId);
            config.Correlate<AnotherTransactionReplicationSuccess>(e => e.id, s => s.AnotherTransactionId);

        }
        public AnotherTransactionCreateSaga(IBus bus)
        {
            _bus = bus;
        }

        public async Task Handle(AnotherTransactionAddedEvent message)
        {
            if (!IsNew)
            {
                return;
            }

            Data.AnotherTransactionReplicationStatus = TransactionReplicationStatusType.Pending;

            await _bus.Send(new ReplicateAnotherTransaction(message.transactionId, message.transactionData));
        }

        public Task Handle(AnotherTransactionReplicationSuccess message)
        {
            Data.AnotherTransactionReplicationStatus = TransactionReplicationStatusType.Sent;

            MarkAsComplete();

            return Task.CompletedTask;
        }

        public async Task Handle(AnotherTransactionReplicationFailed message)
        {
            if (Data.AnotherTransactionReplicationRetries == 3)
            {
                Data.AnotherTransactionReplicationStatus = TransactionReplicationStatusType.Failed;
            }

            // Failure logic where retrying can occur

            if (Data.AnotherTransactionReplicationRetries > 0)
            {

                Data.TryReplication();

                await _bus.Send(new ReplicateAnotherTransaction(message.id, message.data));
            }

            return;

        }

 
    }
}
