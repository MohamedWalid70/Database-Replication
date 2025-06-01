using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using Rebus.Bus;
using Rebus.Handlers;
using Replication.Application.AnotherTransactions.Commands.Replicate;

namespace Replication.Application.AnotherTransactions
{
    internal class ReplicateAnotherTransactionHandler : IHandleMessages<ReplicateAnotherTransaction>
    {

        private readonly ISender _sender;
        private readonly IBus _bus;
        private readonly ILogger _logger;

        public ReplicateAnotherTransactionHandler(ISender sender, IBus bus, ILogger<ReplicateAnotherTransactionHandler> logger)
        {
            _sender = sender;
            _bus = bus;
            _logger = logger;
        }

        public async Task Handle(ReplicateAnotherTransaction message)
        {
            Result replicationResult = await _sender.Send(new ReplicateAnotherTransactionCommand(message.id, message.data));

            if (replicationResult.IsSuccess)
            {
                _logger.LogInformation($"-------------Transaction data of Id: {message.id} is replicated successfully------------");

                await _bus.Send(new AnotherTransactionReplicationSuccess(message.id));
            }
            else
            {
                _logger.LogInformation($"-------------Data replication failure of Transaction id: {message.id}---------------");

                foreach (var error in replicationResult.Errors)
                    _logger.LogError(error.Message);

                await _bus.Publish(new AnotherTransactionReplicationFailed(message.id, message.data));
            }
        }


    }
}
