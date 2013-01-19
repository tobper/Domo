using System;

namespace Domo.Communication
{
    public interface IMessage
    {
        Guid Id { get; set; }
    }
}