
using FluentResults;
using Rebus.Bus;
using Replication.Application.Abstractions.Data;
using Replication.Application.Abstractions.Messaging;
using Replication.Domain.Entities.AnotherTransactionReplication;

namespace Replication.Application.AnotherTransactions.Commands.Replicate;
internal sealed class AddTransactionDeleteCommandHandler : ICommandHandler<ReplicateAnotherTransactionDeleteCommand>
{
    private readonly AnotherTransactionReplicationService _anotherTransactionReplicationService;
    private readonly IBus _bus;
    private readonly IUnitOfWork _unitOfWork;

    public AddTransactionDeleteCommandHandler(
        IBus bus,
        IUnitOfWork unitOfWork,
        AnotherTransactionReplicationService anotherTransactionReplicationService)
    {
        _bus = bus;
        _unitOfWork = unitOfWork;
        _anotherTransactionReplicationService = anotherTransactionReplicationService;
    }

    public async Task<Result> Handle(ReplicateAnotherTransactionDeleteCommand request, CancellationToken cancellationToken)
    {

        Result<AnotherTransactionReplication> anotherTransactionReplicatiomResult = 
            await _anotherTransactionReplicationService.ReplicateAnotherTransactionDeleteAsync(request.transactionId);

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

