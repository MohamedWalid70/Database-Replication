using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using Rebus.Bus;
using Rebus.Handlers;
using Replication.Application.Transactions.Commands.Replicate;

namespace Replication.Application.Transactions
{
    public class SendTransactionDataHandler : IHandleMessages<SendTransactionData>
    {
        private readonly ISender _sender;
        private readonly IBus _bus;
        //private readonly ILogger _logger;

        public SendTransactionDataHandler(ISender sender, IBus bus)
        {
            _sender = sender;
            _bus = bus;
            //_logger = logger;
        }

        public async Task Handle(SendTransactionData message)
        {
            Result replicationResult = await _sender.Send(new ReplicateTransactionCommand(message.transactionId, message.transactionData));

            if (replicationResult.IsSuccess)
            {
                //_logger.LogInformation($"Transaction data of Id: {message.transactionId} is replicated successfully");

                await _bus.Send(new TransactionDataReplicatedEvent(message.transactionId));
            }
            else
            {
                //_logger.LogInformation($"Data replication failure of Transaction id: {message.transactionId}");

                //foreach (var error in replicationResult.Errors)
                //    _logger.LogError(error.Message);

                await _bus.Publish(new TransactionReplicationFailedEvent(message.transactionId, message.transactionData));
            }
        }
    }
}
