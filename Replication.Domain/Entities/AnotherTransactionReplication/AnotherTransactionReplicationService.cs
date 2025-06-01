using FluentResults;
using Replication.Domain.Repositories;

namespace Replication.Domain.Entities.AnotherTransactionReplication
{
    public class AnotherTransactionReplicationService
    {
        IRepository<AnotherTransactionReplication> _repository;

        public AnotherTransactionReplicationService(IRepository<AnotherTransactionReplication> repository)
        {
            _repository = repository;
        }

        public async ValueTask<Result<AnotherTransactionReplication>> ReplicateAnotherTransactionAsync(Guid transactionId, int transactionData)
        {

            if (transactionId == Guid.Empty)
            {
                return Result.Fail("anotherTransactionReplication data is not provided");
            }

            AnotherTransactionReplication? duplicateTransaction = await _repository.GetByIdAsync(transactionId);

            if (duplicateTransaction != null)
            {
                return Result.Fail("anotherTransactionReplication data is duplicated");
            }

            AnotherTransactionReplication anotherTransaction = new(transactionId, transactionData);

            await _repository.AddAsync(anotherTransaction);

            return Result.Ok(anotherTransaction);
        }

        public async ValueTask<Result<AnotherTransactionReplication>> ReplicateAnotherTransactionUpdateAsync(Guid id, int data)
        {

            AnotherTransactionReplication? anotherTransaction = await _repository.GetByIdAsync(id);

            if (anotherTransaction == null)
            {
                return Result.Fail($"No such anotherTransactionReplication with an id of {id} exists");
            }

            _repository.BeginUpdate(anotherTransaction);

            anotherTransaction.Update(data);

            return Result.Ok(anotherTransaction);
        }

        public async ValueTask<Result<AnotherTransactionReplication>> ReplicateAnotherTransactionDeleteAsync(Guid id)
        {

            AnotherTransactionReplication? anotherTransaction = await _repository.GetByIdAsync(id);

            if (anotherTransaction == null)
            {
                return Result.Fail($"No such anotherTransactionReplication with an id of {id} exists");
            }

            _repository.Delete(anotherTransaction);

            return Result.Ok(anotherTransaction);
        }

    }
}
