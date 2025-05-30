using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Replication.Domain.Entities.Transaction
{
    public class InfrastructureTransaction
    {
        public InfrastructureTransaction(string data)
        {
            Data = data;
        }

        public Guid Id { get; private set; }
        public string Data { get; private set; }

    }
}
