using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Replication.Domain.Entities.AnotherTransactionReplication
{
    public class AnotherTransactionReplication
    {
        public Guid Id { get; private set; }
        public int Data { get; private set; }

        internal AnotherTransactionReplication(Guid id, int data)
        {
            Id = id;
            Data = data;
        }

        internal void Update(int data)
        {
            Data = data;
        }
    }
}
