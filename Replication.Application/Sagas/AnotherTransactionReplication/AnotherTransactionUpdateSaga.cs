using Rebus.Bus;
using Rebus.Handlers;
using Rebus.Sagas;
using Replication.Application.AnotherTransactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Replication.Application.Sagas.AnotherTransactionReplication
{
    internal class AnotherTransactionUpdateSaga : Saga<AnotherTransactionSagaData>,
        IAmInitiatedBy<AnotherTransactionUpdatedEvent>,
        IHandleMessages<AnotherTransactionUpdateReplicationSuccess>,
        IHandleMessages<AnotherTransactionUpdateReplicationFailed>
    {
        private readonly IBus _bus;

        public AnotherTransactionUpdateSaga(IBus bus)
        {
            _bus = bus;
        }

        protected override void CorrelateMessages(ICorrelationConfig<AnotherTransactionSagaData> config)
        {
            config.Correlate<AnotherTransactionUpdatedEvent>(e => e.transactionId, s => s.AnotherTransactionId);
            config.Correlate<AnotherTransactionUpdateReplicationSuccess>(e => e.id, s => s.AnotherTransactionId);
            config.Correlate<AnotherTransactionUpdateReplicationFailed>(e => e.id, s => s.AnotherTransactionId);

        }
        public async Task Handle(AnotherTransactionUpdatedEvent message)
        {
            if (!IsNew)
            {
                return;
            }

            Data.AnotherTransactionReplicationStatus = TransactionReplicationStatusType.Pending;

            await _bus.Send(new ReplicateAnotherTransactionUpdate(message.transactionId, message.transactionData));

        }

        public Task Handle(AnotherTransactionUpdateReplicationSuccess message)
        {
            Data.AnotherTransactionReplicationStatus = TransactionReplicationStatusType.Sent;

            MarkAsComplete();

            return Task.CompletedTask;
        }

        public async Task Handle(AnotherTransactionUpdateReplicationFailed message)
        {
            if (Data.AnotherTransactionReplicationRetries == 3)
            {
                Data.AnotherTransactionReplicationStatus = TransactionReplicationStatusType.Failed;
            }

            // Failure logic where retrying can occur

            if (Data.AnotherTransactionReplicationRetries > 0)
            {

                Data.TryReplication();

                await _bus.Send(new ReplicateAnotherTransactionUpdate(message.id, message.data));
            }

            return;
        }


    }
}
