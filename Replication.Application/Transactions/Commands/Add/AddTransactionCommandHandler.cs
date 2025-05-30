
using FluentResults;
using Rebus.Bus;
using Replication.Application.Abstractions.Data;
using Replication.Application.Abstractions.Messaging;
using Replication.Domain.Entities.Transaction;
using Replication.Domain.Repositories;

namespace Replication.Application.Transactions.Commands.Add;
internal sealed class AddTransactionCommandHandler : ICommandHandler<AddTransactionCommand>
{
    private readonly InfrastructureTransactionService _infrastructureTransactionService;
    private readonly IBus _bus;
    private readonly IUnitOfWork _unitOfWork;

    public AddTransactionCommandHandler(InfrastructureTransactionService infrastructureTransactionService, 
        IBus bus, 
        IUnitOfWork unitOfWork)
    {
        _infrastructureTransactionService = infrastructureTransactionService;
        _bus = bus;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(AddTransactionCommand request, CancellationToken cancellationToken)
    {
        Result<InfrastructureTransaction> transactionAdditionResult = await _infrastructureTransactionService.AddTransactionAsync(request.transactionData);

        if (transactionAdditionResult.IsFailed)
        {
            return transactionAdditionResult.ToResult();
        }

        int savingResult = await _unitOfWork.SaveChangesAsync();

        if(savingResult == 0)
        {
            return Result.Fail("Transaction saving has failed");
        }

        await _bus.Send(new TransactionAddedEvent(transactionAdditionResult.Value.Id, transactionAdditionResult.Value.Data));

        return transactionAdditionResult.ToResult();
    }
}

