using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Replication.API.Dtos.Transaction;
using Replication.Application.Transactions.Commands.Add;
using System.Threading.Tasks;

namespace Replication.API.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ISender _sender;

        public TransactionsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("specific-transaction")]
        public async Task<ActionResult> AddTransaction(TransactionDto transactionDto)
        {
            var addTransactionCommand = new AddTransactionCommand(transactionDto.Data);

            Result result = await _sender.Send(addTransactionCommand);

            return result.IsSuccess ? Created() : BadRequest();

        }
    }
}
