using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Replication.Domain.Entities.AnotherTransaction
{
    public class AnotherTransaction
    {
        public AnotherTransaction(int data)
        {
            Data = data;
        }
        public Guid Id { get; private set; }
        public int Data { get; private set; }

        internal void Update(int data) 
        {
            Data = data;
        }
    }
}
