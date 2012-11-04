using System;

namespace Domo.Messaging
{
    public interface ICommand
    {
        Guid Id { get; set; }
    }
}