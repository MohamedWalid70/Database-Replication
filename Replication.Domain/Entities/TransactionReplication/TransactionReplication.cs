using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Replication.Domain.Entities.TransactionReplication
{
    public class TransactionReplication
    {
        public Guid Id { get; private set; }
        public string Data { get; private set; }

        internal TransactionReplication(Guid id, string data)
        {
            Id = id;
            Data = data;
        }
    }
}
