
using FluentResults;
using Rebus.Bus;
using Replication.Application.Abstractions.Data;
using Replication.Application.Abstractions.Messaging;
using Replication.Domain.Entities.TransactionReplication;

namespace Replication.Application.Transactions.Commands.Replicate;
internal sealed class AddTransactionCommandHandler : ICommandHandler<ReplicateTransactionCommand>
{
    private readonly TransactionReplicationService _transactionReplicationService;
    private readonly IBus _bus;
    private readonly IUnitOfWork _unitOfWork;

    public AddTransactionCommandHandler(TransactionReplicationService transactionReplicationService, 
        IBus bus, 
        IUnitOfWork unitOfWork)
    {
        _transactionReplicationService = transactionReplicationService;
        _bus = bus;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(ReplicateTransactionCommand request, CancellationToken cancellationToken)
    {

        Result<TransactionReplication> transactionReplicatiomResult = 
            await _transactionReplicationService.ReplicateTransactionAsync(request.transactionId, request.transactionData);

        if (transactionReplicatiomResult.IsFailed)
        {
            return transactionReplicatiomResult.ToResult();
        }

        int savingResult = await _unitOfWork.SaveChangesAsync();

        if(savingResult == 0)
        {
            return Result.Fail("Transaction saving has failed");
        }

        return transactionReplicatiomResult.ToResult();
    }
}

