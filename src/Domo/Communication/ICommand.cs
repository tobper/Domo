using System;

namespace Domo.Communication
{
    public interface ICommand
    {
        Guid Id { get; set; }
    }
}