using System;

namespace Domo.Messaging
{
    public interface IMessage
    {
        Guid TransactionId { get; set; }
    }
}