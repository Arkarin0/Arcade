// Created/modified by Arkarin0 under one more more license(s).

using Microsoft.Build.Framework;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Arkarin0.Arcade.Test.Common
{
    public class MockBuildEngine : IBuildEngine
    {
        public bool ContinueOnError => throw new NotImplementedException();

        public int LineNumberOfTaskNode => 0;

        public int ColumnNumberOfTaskNode => 0;

        public string ProjectFileOfTaskNode => "Fake File";

        public List<CustomBuildEventArgs> CustomBuildEvents = new List<CustomBuildEventArgs>();
        public List<BuildErrorEventArgs> BuildErrorEvents = new List<BuildErrorEventArgs>();
        public List<BuildMessageEventArgs> BuildMessageEvents = new List<BuildMessageEventArgs>();
        public List<BuildWarningEventArgs> BuildWarningEvents = new List<BuildWarningEventArgs>();

        public bool BuildProjectFile(string projectFileName, string[] targetNames, IDictionary globalProperties, IDictionary targetOutputs)
        {
            throw new NotImplementedException();
        }

        public void LogCustomEvent(CustomBuildEventArgs e)
        {
            CustomBuildEvents.Add(e);
        }

        public void LogErrorEvent(BuildErrorEventArgs e)
        {
            BuildErrorEvents.Add(e);
        }

        public void LogMessageEvent(BuildMessageEventArgs e)
        {
            BuildMessageEvents.Add(e);
        }

        public void LogWarningEvent(BuildWarningEventArgs e)
        {
            BuildWarningEvents.Add(e);
        }
    }
}
