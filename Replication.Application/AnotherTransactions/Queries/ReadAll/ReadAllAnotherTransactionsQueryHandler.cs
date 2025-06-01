using FluentResults;
using MediatR;
using Replication.Application.Abstractions.Messaging;
using Replication.Domain.Entities.AnotherTransaction;

namespace Replication.Application.AnotherTransactions.Queries.ReadAll
{
    internal class ReadAllAnotherTransactionsQueryHandler : IQueryHandler<ReadAllAnotherTransactionsQuery, IQueryable<AnotherTransaction>>
    {
        private readonly AnotherTransactionService _anotherTransactionService;

        public ReadAllAnotherTransactionsQueryHandler(AnotherTransactionService anotherTransactionService)
        {
            _anotherTransactionService = anotherTransactionService;
        }

        public async Task<Result<IQueryable<AnotherTransaction>>> Handle(ReadAllAnotherTransactionsQuery request, CancellationToken cancellationToken)
        {
            IQueryable<AnotherTransaction> transactions = _anotherTransactionService.GetAllAnotherTransactions();

            return Result.Ok(transactions);
        }
    }
}
