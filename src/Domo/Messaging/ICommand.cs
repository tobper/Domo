using System;

namespace Domo.Messaging
{
    public interface ICommand
    {
        Guid TransactionId { get; set; }
    }
}