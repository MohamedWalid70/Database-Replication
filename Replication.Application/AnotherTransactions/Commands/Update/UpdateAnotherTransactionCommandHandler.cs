using FluentResults;
using Rebus.Bus;
using Replication.Application.Abstractions.Data;
using Replication.Application.Abstractions.Messaging;
using Replication.Application.AnotherTransactions.Commands.Delete;
using Replication.Application.Transactions;
using Replication.Domain.Entities.AnotherTransaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Replication.Application.AnotherTransactions.Commands.Update
{
    internal sealed class UpdateAnotherTransactionCommandHandler : ICommandHandler<UpdateAnotherTransactionCommand>
    {
        private readonly AnotherTransactionService _anotherTransactionService;
        private readonly IBus _bus;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateAnotherTransactionCommandHandler(AnotherTransactionService anotherTransactionService,
            IBus bus,
            IUnitOfWork unitOfWork)
        {
            _anotherTransactionService = anotherTransactionService;
            _bus = bus;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateAnotherTransactionCommand request, CancellationToken cancellationToken)
        {
            Result<AnotherTransaction> transactionAdditionResult = await _anotherTransactionService.UpdateAnotherTransactionAsync(request.id, request.data);

            if (transactionAdditionResult.IsFailed)
            {
                return transactionAdditionResult.ToResult();
            }

            int savingResult = await _unitOfWork.SaveChangesAsync();

            if (savingResult == 0)
            {
                return Result.Fail("Transaction saving has failed or no update occurred to the instance");
            }

            await _bus.Send(new AnotherTransactionUpdatedEvent(transactionAdditionResult.Value.Id, transactionAdditionResult.Value.Data));

            return transactionAdditionResult.ToResult();
        }
    }
}
