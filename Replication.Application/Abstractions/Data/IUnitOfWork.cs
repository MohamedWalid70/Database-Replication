using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Replication.Application.Abstractions.Data;
public interface IUnitOfWork
{
    Task<int> SaveChangesAsync();
}

