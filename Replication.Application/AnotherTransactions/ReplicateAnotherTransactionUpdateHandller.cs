using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using Rebus.Bus;
using Rebus.Handlers;
using Replication.Application.AnotherTransactions.Commands.Replicate;

namespace Replication.Application.AnotherTransactions
{
    internal class ReplicateAnotherTransactionUpdateHandller : IHandleMessages<ReplicateAnotherTransactionUpdate>
    {
        private readonly ISender _sender;
        private readonly IBus _bus;
        private readonly ILogger _logger;

        public ReplicateAnotherTransactionUpdateHandller(ISender sender, IBus bus, ILogger<ReplicateAnotherTransactionUpdateHandller> logger)
        {
            _sender = sender;
            _bus = bus;
            _logger = logger;
        }

        public async Task Handle(ReplicateAnotherTransactionUpdate message)
        {
            Result replicationResult = await _sender.Send(new ReplicateAnotherTransactionUpdateCommand(message.id, message.data));

            if (replicationResult.IsSuccess)
            {
                _logger.LogInformation($"-------------Transaction data of Id: {message.id} is update replicated successfully------------");

                await _bus.Send(new AnotherTransactionUpdateReplicationSuccess(message.id));
            }
            else
            {
                _logger.LogInformation($"-------------Data update replication failure of Transaction id: {message.id}---------------");

                foreach (var error in replicationResult.Errors)
                    _logger.LogError(error.Message);

                await _bus.Publish(new AnotherTransactionUpdateReplicationFailed(message.id, message.data));
            }
        }
    }
}
