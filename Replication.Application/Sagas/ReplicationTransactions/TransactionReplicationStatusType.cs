using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Replication.Application.Sagas.ReplicationTransactions;

public enum TransactionReplicationStatusType : byte
{
    Failed,
    Sent,
    Pending
}
