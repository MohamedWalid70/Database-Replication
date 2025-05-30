using FluentResults;
using Replication.Domain.Entities.Transaction;
using Replication.Domain.Repositories;

namespace Replication.Domain.Entities.TransactionReplication
{
    public class TransactionReplicationService
    {

        IRepository<TransactionReplication> _repository;

        public TransactionReplicationService(IRepository<TransactionReplication> repository)
        {
            _repository = repository;
        }

        public async ValueTask<Result<TransactionReplication>> ReplicateTransactionAsync(Guid transactionId, string transactionData)
        {

            if (transactionData == null || transactionId == Guid.Empty)
            {
                return Result.Fail("transaction data is not provided");
            }

            TransactionReplication? duplicateTransaction = await _repository.GetByIdAsync(transactionId);

            if (duplicateTransaction != null)
            {
                return Result.Fail("Transaction data is duplicated");
            }

            TransactionReplication transaction = new TransactionReplication(transactionId, transactionData);

            await _repository.AddAsync(transaction);

            return Result.Ok(transaction);
        }

    }
}




