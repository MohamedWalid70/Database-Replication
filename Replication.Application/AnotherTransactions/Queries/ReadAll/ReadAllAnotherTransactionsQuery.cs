using Replication.Application.Abstractions.Messaging;
using Replication.Domain.Entities.AnotherTransaction;

namespace Replication.Application.AnotherTransactions.Queries.ReadAll;
public record ReadAllAnotherTransactionsQuery() : IQuery<IQueryable<AnotherTransaction>>;
