
using FluentResults;
using Rebus.Bus;
using Replication.Application.Abstractions.Data;
using Replication.Application.Abstractions.Messaging;
using Replication.Domain.Entities.AnotherTransactionReplication;

namespace Replication.Application.AnotherTransactions.Commands.Replicate;
internal sealed class AddTransactionCommandHandler : ICommandHandler<ReplicateAnotherTransactionCommand>
{
    private readonly AnotherTransactionReplicationService _anotherTransactionReplicationService;
    private readonly IBus _bus;
    private readonly IUnitOfWork _unitOfWork;

    public AddTransactionCommandHandler(
        IBus bus,
        IUnitOfWork unitOfWork,
        AnotherTransactionReplicationService anotherTransactionReplicationService)
    {
        _bus = bus;
        _unitOfWork = unitOfWork;
        _anotherTransactionReplicationService = anotherTransactionReplicationService;
    }

    public async Task<Result> Handle(ReplicateAnotherTransactionCommand request, CancellationToken cancellationToken)
    {

        Result<AnotherTransactionReplication> anotherTransactionReplicatiomResult = 
            await _anotherTransactionReplicationService.ReplicateAnotherTransactionAsync(request.transactionId, request.transactionData);

        if (anotherTransactionReplicatiomResult.IsFailed)
        {
            return anotherTransactionReplicatiomResult.ToResult();
        }

        int savingResult = await _unitOfWork.SaveChangesAsync();

        if(savingResult == 0)
        {
            return Result.Fail("Another Transaction replication saving has failed");
        }

        return anotherTransactionReplicatiomResult.ToResult();
    }
}

