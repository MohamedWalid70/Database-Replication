using Rebus.Sagas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Replication.Application.Sagas.AnotherTransactionReplication
{
    internal class AnotherTransactionSagaData : ISagaData
    {
        public Guid Id { get; set; }
        public int Revision { get; set; }
        public Guid AnotherTransactionId { get; set; }
        public TransactionReplicationStatusType AnotherTransactionReplicationStatus { get; set; }
        public int AnotherTransactionReplicationRetries { get; private set; } = 3;

        public void TryReplication()
        {
            AnotherTransactionReplicationRetries--;
        }
    }
}
