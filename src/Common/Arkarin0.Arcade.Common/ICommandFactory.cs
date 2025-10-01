// Created/modified by Arkarin0 under one more more license(s).

using System.Collections.Generic;

namespace Arkarin0.Arcade.Common
{
    public interface ICommandFactory
    {
        ICommand Create(string executable, IEnumerable<string> args);
        ICommand Create(string executable, params string[] args);
        ICommand Create(string executable, string args);
    }
}
