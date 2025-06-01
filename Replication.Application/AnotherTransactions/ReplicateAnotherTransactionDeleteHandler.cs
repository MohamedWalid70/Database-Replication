using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using Rebus.Bus;
using Rebus.Handlers;
using Replication.Application.AnotherTransactions.Commands.Replicate;

namespace Replication.Application.AnotherTransactions
{
    internal class ReplicateAnotherTransactionDeleteHandler : IHandleMessages<ReplicateAnotherTransactionDelete>
    {
        private readonly ISender _sender;
        private readonly IBus _bus;
        private readonly ILogger _logger;

        public ReplicateAnotherTransactionDeleteHandler(ISender sender, IBus bus, ILogger<ReplicateAnotherTransactionDeleteHandler> logger)
        {
            _sender = sender;
            _bus = bus;
            _logger = logger;
        }

        public async Task Handle(ReplicateAnotherTransactionDelete message)
        {
            Result replicationResult = await _sender.Send(new ReplicateAnotherTransactionDeleteCommand(message.id));

            if (replicationResult.IsSuccess)
            {
                _logger.LogInformation($"-------------Transaction data of Id: {message.id} is delete replicated successfully------------");

                await _bus.Send(new AnotherTransactionDeleteReplicationSuccess(message.id));
            }
            else
            {
                _logger.LogInformation($"-------------Data delete replication failure of Transaction id: {message.id}---------------");

                foreach (var error in replicationResult.Errors)
                    _logger.LogError(error.Message);

                await _bus.Publish(new AnotherTransactionDeleteReplicationFailed(message.id));
            }
        }
    }
}
