// Created/modified by Arkarin0 under one more more license(s).

using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.XPath;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
using Xunit;
using Xunit.Abstractions;

namespace Arkarin0.DotNet.Arcade.Sdk.Tests
{
    [Collection(TestProjectCollection.Name)]
    public class UnitestRepoTests
    {
        private readonly ITestOutputHelper _output;
        private readonly TestProjectFixture _fixture;

        public UnitestRepoTests(ITestOutputHelper output, TestProjectFixture fixture)
        {
            _output = output;
            _fixture = fixture;
        }

        private static void AssertTestInfoFile(TestApp app, string project,
            bool expectedIsTestProject,bool expectedIsUnitTestProject,bool expectedIsIntegrationTestProject, bool expectedIsPerformanceTestProject, string expectedTestRunnerName,
            string targetFrameWork= "net4.6.2")
        {
            var artifactsBin = Path.Join(app.WorkingDirectory, "artifacts", "bin", project, "Debug", targetFrameWork);
            var testinfoFilePath = Path.Combine(artifactsBin, "TestInfo.txt");

            Assert.True(File.Exists(testinfoFilePath), $"Result file '{testinfoFilePath}' does not extist.");

            var content = File.ReadAllText(testinfoFilePath)
                .Split("\r\n")
                .Where(e => !string.IsNullOrWhiteSpace(e));

            Assert.Equal($"IsTestProject: {expectedIsTestProject}", content.ElementAtOrDefault(0),ignoreCase:true);
            Assert.Equal($"IsPerformanceTestProject: {expectedIsPerformanceTestProject}", content.ElementAtOrDefault(1),ignoreCase: true);
            Assert.Equal($"IsIntegrationTestProject: {expectedIsIntegrationTestProject}", content.ElementAtOrDefault(2), ignoreCase: true);
            Assert.Equal($"IsUnitTestProject: {expectedIsUnitTestProject}", content.ElementAtOrDefault(3), ignoreCase: true);
            Assert.Equal($"TestRunnerName: {expectedTestRunnerName}", content.ElementAtOrDefault(4), ignoreCase: true);
        }

        private static void AssertTestResults(TestApp app, string project, string targetFrameWork = "net4.6.2", string testRunnerName="XUnit")
        {
            int expectedfailedProjects = 1, expectedPassedProjects = 1;
            var configuration = "Debug";
            var platform = "x64";

            var outputDir = Path.Join(app.WorkingDirectory, "artifacts", "TestResults", configuration);
            var basefileName = $"{project}_{targetFrameWork}_{platform}.xml";
            var basefile= Path.Combine(outputDir, basefileName);
            var trxResultFile = Path.ChangeExtension(basefile, "trx");
            var xmlResultFile = Path.ChangeExtension(basefile, "xml");
            var htmlResultFile = Path.ChangeExtension(basefile, "html");

            
            switch (testRunnerName)
            {
                case "XUnit":
                    Assert.True(File.Exists(xmlResultFile), $"Result file does not exist: '{xmlResultFile}'.");
                    Assert.True(File.Exists(htmlResultFile), $"Result file does not exist: '{htmlResultFile}'.");
                    AssertXunitXmlTestResultFile(xmlResultFile, expectedPassedProjects, expectedfailedProjects);
                    break;
                case "NUnit":
                case "MSTest":
                    Assert.True(File.Exists(trxResultFile), $"Result file does not exist: '{trxResultFile}'.");
                    Assert.True(File.Exists(htmlResultFile), $"Result file does not exist: '{htmlResultFile}'.");
                    AssertVSTestTrxTestResultFile(trxResultFile, expectedPassedProjects, expectedfailedProjects);
                    break;

                default:
                    Assert.True(false, $"Testrunnername '{testRunnerName}' not implemented");
                    break;
            }
            
        }


        private static void AssertXunitXmlTestResultFile(string filepath, int expectedPasses, int expectedFails)
        {
            Assert.True(File.Exists(filepath), $"Result file does not exist: '{filepath}'.");

            var stream = File.OpenRead(filepath);
            var reader = new XmlTextReader(stream)
            {
                Namespaces = false
            };
            var document = new System.Xml.XmlDocument();
            document.Load(reader);
            stream.Close();
            var root = document.DocumentElement;
            var counters = root.SelectSingleNode(@"/assemblies/assembly");

            Assert.NotNull(counters);
#pragma warning disable CA1305 // IFormatProvider angeben            
            int failedProjects = int.Parse(counters.Attributes["failed"].Value);
            int passedProjects = int.Parse(counters.Attributes["passed"].Value);
#pragma warning restore CA1305 // IFormatProvider angeben

            Assert.Equal(expectedPasses, failedProjects);
            Assert.Equal(passedProjects, expectedFails);
        }

        private static void AssertVSTestTrxTestResultFile(string filepath, int expectedPasses, int expectedFails)
        {
            Assert.True(File.Exists(filepath), $"Result file does not exist: '{filepath}'.");

            var stream = File.OpenRead(filepath);
            var reader = new XmlTextReader(stream)
            {
                Namespaces = false
            };
            var document = new System.Xml.XmlDocument();
            document.Load(reader);
            stream.Close();
            var root = document.DocumentElement;
            var counters = root.SelectSingleNode(@"/TestRun/ResultSummary/Counters");

            Assert.NotNull(counters);
#pragma warning disable CA1305 // IFormatProvider angeben            
            int failedProjects = int.Parse(counters.Attributes["failed"].Value);
            int passedProjects = int.Parse(counters.Attributes["passed"].Value);
#pragma warning restore CA1305 // IFormatProvider angeben

            Assert.Equal(expectedPasses, failedProjects);
            Assert.Equal(passedProjects, expectedFails);
        }


        //[Fact(Skip = "https://github.com/dotnet/arcade/issues/7092")]
        [Fact]
        public void RepoBuildsWithoutErrors()
        {
            var app = _fixture.CreateTestApp("UnittestRepo");
            var exitCode = app.ExecuteBuild(_output,
                // these properties are required for projects that are not in a git repo
                "/p:EnableSourceLink=false",
                "/p:EnableSourceControlManagerQueries=false");
            Assert.Equal(0, exitCode);
        }

        [Fact]
        public void TestIsIdentifiedAsUnittest()
        {
            var app = _fixture.CreateTestApp("UnittestRepo");
            var exitCode = app.ExecuteBuild(_output,
                // these properties are required for projects that are not in a git repo
                "/p:EnableSourceLink=false",
                "/p:EnableSourceControlManagerQueries=false");
            Assert.Equal(0, exitCode);

            AssertTestInfoFile(app, "ClassLib1.Tests",
                expectedIsTestProject: true,
                expectedIsUnitTestProject: true,
                expectedIsIntegrationTestProject: false,
                expectedIsPerformanceTestProject: false,
                expectedTestRunnerName: "XUnit");
        }

        [Fact]
        public void TestIsIdentifiedAsIntegrationtest()
        {
            var app = _fixture.CreateTestApp("UnittestRepo");
            var exitCode = app.ExecuteBuild(_output,
                // these properties are required for projects that are not in a git repo
                "/p:EnableSourceLink=false",
                "/p:EnableSourceControlManagerQueries=false");
            Assert.Equal(0, exitCode);

            AssertTestInfoFile(app, "ClassLib1.IntegrationTests",
                expectedIsTestProject: true,
                expectedIsUnitTestProject: false,
                expectedIsIntegrationTestProject: true,
                expectedIsPerformanceTestProject: false,
                expectedTestRunnerName: "XUnit");
        }

        [Fact]
        public void TestIsIdentifiedAsPerformancetest()
        {
            var app = _fixture.CreateTestApp("UnittestRepo");
            var exitCode = app.ExecuteBuild(_output,
                // these properties are required for projects that are not in a git repo
                "/p:EnableSourceLink=false",
                "/p:EnableSourceControlManagerQueries=false");
            Assert.Equal(0, exitCode);

            AssertTestInfoFile(app, "ClassLib1.PerformanceTests",
                expectedIsTestProject: true,
                expectedIsUnitTestProject: false,
                expectedIsIntegrationTestProject: false,
                expectedIsPerformanceTestProject: true,
                expectedTestRunnerName: "XUnit");
        }

        [Theory]
        [InlineData("XUnit")]
        [InlineData("MSTest")]
        [InlineData("NUnit")]
        public void TestframworkIsAppliedAndExecuteable(string testFramework)
        {
            var project = "ClassLib1.Tests";

            var app = _fixture.CreateTestApp("UnittestRepo");
            var exitCode = app.ExecuteBuild(_output,
                // these properties are required for projects that are not in a git repo
                "-test",
                "-UseArcade",
                "/p:EnableSourceLink=false",
                "/p:EnableSourceControlManagerQueries=false",
                $"/p:TestRunnerName={testFramework}");
            Assert.Equal(1, exitCode);

            AssertTestInfoFile(app, project,
                expectedIsTestProject: true,
                expectedIsUnitTestProject: true,
                expectedIsIntegrationTestProject: false,
                expectedIsPerformanceTestProject: false,
                expectedTestRunnerName: testFramework);

            AssertTestResults(app, project, testRunnerName:testFramework);
        }
    }
}
