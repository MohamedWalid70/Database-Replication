using FluentResults;
using Rebus.Bus;
using Replication.Application.Abstractions.Data;
using Replication.Application.Abstractions.Messaging;
using Replication.Application.Transactions;
using Replication.Domain.Entities.AnotherTransaction;

namespace Replication.Application.AnotherTransactions.Commands.Delete
{
    internal sealed class DeleteAnotherTransactionCommandHandler : ICommandHandler<DeleteAnotherTransactionCommand>
    {
        private readonly AnotherTransactionService _anotherTransactionService;
        private readonly IBus _bus;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteAnotherTransactionCommandHandler(AnotherTransactionService anotherTransactionService,
            IBus bus,
            IUnitOfWork unitOfWork)
        {
            _anotherTransactionService = anotherTransactionService;
            _bus = bus;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(DeleteAnotherTransactionCommand request, CancellationToken cancellationToken)
        {
            Result<AnotherTransaction> transactionAdditionResult = await _anotherTransactionService.DeleteAnotherTransactionAsync(request.id);

            if (transactionAdditionResult.IsFailed)
            {
                return transactionAdditionResult.ToResult();
            }

            int savingResult = await _unitOfWork.SaveChangesAsync();

            if (savingResult == 0)
            {
                return Result.Fail("Transaction saving has failed");
            }

            await _bus.Send(new AnotherTransactionDeletedEvent(transactionAdditionResult.Value.Id));

            return transactionAdditionResult.ToResult();
        }
    }
}
