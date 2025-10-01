// Created/modified by Arkarin0 under one more more license(s).

using System;
using System.IO;

namespace Arkarin0.Arcade.Common
{
    public interface ICommand
    {
        CommandResult Execute();

        ICommand CaptureStdErr();
        ICommand CaptureStdOut();
        ICommand EnvironmentVariable(string name, string value);
        ICommand ForwardStatus(TextWriter to = null);
        ICommand ForwardStdErr(TextWriter to = null);
        ICommand ForwardStdOut(TextWriter to = null);
        ICommand OnErrorLine(Action<string> handler);
        ICommand OnOutputLine(Action<string> handler);
        ICommand QuietBuildReporter();
        ICommand WorkingDirectory(string projectDirectory);
    }
}
