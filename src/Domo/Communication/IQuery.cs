using System;

namespace Domo.Communication
{
    public interface IQuery
    {
        Guid TransactionId { get; set; }
    }
}
