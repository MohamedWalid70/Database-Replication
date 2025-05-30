
using FluentResults;
using Replication.Domain.Repositories;

namespace Replication.Domain.Entities.Transaction;
public sealed class InfrastructureTransactionService
{
    IRepository<InfrastructureTransaction> _repository;

    public InfrastructureTransactionService(IRepository<InfrastructureTransaction> repository)
    {
        _repository = repository;
    }

    public async ValueTask<Result<InfrastructureTransaction>> AddTransactionAsync(string transactionData)
    {

        if (transactionData == null)
        {
            return Result.Fail("transaction data is not provided");
        }

        InfrastructureTransaction transaction = new(transactionData);

        await _repository.AddAsync(transaction);

        return Result.Ok(transaction);
    }
}

