using FluentResults;
using Replication.Domain.Entities.Transaction;
using Replication.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Replication.Domain.Entities.AnotherTransaction
{
    public class AnotherTransactionService
    {
        IRepository<AnotherTransaction> _repository;

        public AnotherTransactionService(IRepository<AnotherTransaction> repository)
        {
            _repository = repository;
        }

        public IQueryable<AnotherTransaction> GetAllAnotherTransactions()
        {
            return _repository.GetAll();
        }

        public async ValueTask<Result<AnotherTransaction>> AddAnotherTransactionAsync(int data)
        {

            AnotherTransaction anotherTransaction = new(data);

            await _repository.AddAsync(anotherTransaction);

            return Result.Ok(anotherTransaction);
        }

        public async ValueTask<Result<AnotherTransaction>> UpdateAnotherTransactionAsync(Guid id, int data)
        {

            AnotherTransaction? anotherTransaction = await _repository.GetByIdAsync(id);

            if (anotherTransaction == null)
            {
                return Result.Fail($"No such anotherTransaction with an id of {id} exists");
            }

            _repository.BeginUpdate(anotherTransaction);

            anotherTransaction.Update(data);

            return Result.Ok(anotherTransaction);
        }

        public async ValueTask<Result<AnotherTransaction>> DeleteAnotherTransactionAsync(Guid id)
        {

            AnotherTransaction? anotherTransaction = await _repository.GetByIdAsync(id);

            if (anotherTransaction == null)
            {
                return Result.Fail($"No such anotherTransaction with an id of {id} exists");
            }

            _repository.Delete(anotherTransaction);

            return Result.Ok(anotherTransaction);
        }

    }
}
