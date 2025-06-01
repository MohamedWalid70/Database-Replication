using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Replication.API.Dtos.AnotherTransaction;
using Replication.Application.AnotherTransactions.Commands.Add;
using Replication.Application.AnotherTransactions.Commands.Delete;
using Replication.Application.AnotherTransactions.Commands.Update;
using Replication.Application.AnotherTransactions.Queries.ReadAll;
using Replication.Domain.Entities.AnotherTransaction;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Replication.API.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class AnotherTransactionsController : ControllerBase
    {
        private readonly ISender _sender;

        public AnotherTransactionsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet("all")]
        public async ValueTask<ActionResult> ReadAll()
        {
           Result<IQueryable<AnotherTransaction>> result = await _sender.Send(new ReadAllAnotherTransactionsQuery());

            IAsyncEnumerable<AnotherTransaction> values = result.Value.AsAsyncEnumerable();

           if (result.IsSuccess)
                return Ok(values);

            return new StatusCodeResult(StatusCodes.Status500InternalServerError) ;
        }


        [HttpPost("new-another-transaction")]
        public async ValueTask<ActionResult> Add(AnotherTransactionDto anotherTransaction)
        {
            Result additionResult = await _sender.Send(new AddAnotherTransactionCommand(anotherTransaction.Data));

            return additionResult.IsSuccess ? Created() : BadRequest();

        }

        [HttpPatch("specific-another-transaction/{id:guid}")]
        public async ValueTask<ActionResult> Update(Guid id, [FromBody] AnotherTransactionDto anotherTransaction)
        {
            Result additionResult = await _sender.Send(new UpdateAnotherTransactionCommand(id,anotherTransaction.Data));

            return additionResult.IsSuccess ? Ok() : BadRequest();
        }

        // DELETE api/<AnotherTransactionsController>/5
        [HttpDelete("specific-another-transaction/{id:guid}")]
        public async ValueTask<ActionResult> Delete(Guid id)
        {
            Result additionResult = await _sender.Send(new DeleteAnotherTransactionCommand(id));

            return additionResult.IsSuccess ? Ok() : BadRequest();
        }
    }
}
