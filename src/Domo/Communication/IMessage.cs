using System;

namespace Domo.Communication
{
    public interface IMessage
    {
        Guid TransactionId { get; set; }
    }
}