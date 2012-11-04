using System;

namespace Domo.Messaging
{
    public interface IMessage
    {
        Guid Id { get; set; }
    }
}