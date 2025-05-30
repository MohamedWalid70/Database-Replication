
using System;

public record MessageToBeSent(Guid transactionId, string transactionData);

public record OperationStatusResponse(bool success);
