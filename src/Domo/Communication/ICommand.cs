using System;

namespace Domo.Communication
{
    public interface ICommand
    {
        Guid TransactionId { get; set; }
    }
}