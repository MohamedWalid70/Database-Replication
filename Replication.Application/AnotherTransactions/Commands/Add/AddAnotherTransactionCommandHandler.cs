
using FluentResults;
using Rebus.Bus;
using Replication.Application.Abstractions.Data;
using Replication.Application.Abstractions.Messaging;
using Replication.Domain.Entities.AnotherTransaction;
using Replication.Domain.Entities.Transaction;

namespace Replication.Application.AnotherTransactions.Commands.Add;
internal sealed class AddAnotherTransactionCommandHandler : ICommandHandler<AddAnotherTransactionCommand>
{
    private readonly AnotherTransactionService _anotherTransactionService;
    private readonly IBus _bus;
    private readonly IUnitOfWork _unitOfWork;

    public AddAnotherTransactionCommandHandler(AnotherTransactionService anotherTransactionService, 
        IBus bus, 
        IUnitOfWork unitOfWork)
    {
        _anotherTransactionService = anotherTransactionService;
        _bus = bus;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(AddAnotherTransactionCommand request, CancellationToken cancellationToken)
    {
        Result<AnotherTransaction> transactionAdditionResult = await _anotherTransactionService.AddAnotherTransactionAsync(request.data);

        if (transactionAdditionResult.IsFailed)
        {
            return transactionAdditionResult.ToResult();
        }

        int savingResult = await _unitOfWork.SaveChangesAsync();

        if(savingResult == 0)
        {
            return Result.Fail("Transaction saving has failed");
        }

        await _bus.Send(new AnotherTransactionAddedEvent(transactionAdditionResult.Value.Id, transactionAdditionResult.Value.Data));

        return transactionAdditionResult.ToResult();
    }
}

