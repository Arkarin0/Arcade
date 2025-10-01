// Created/modified by Arkarin0 under one more more license(s).

using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace Arkarin0.DotNet.Arcade.Sdk.Tests
{
    [Collection(TestProjectCollection.Name)]
    public class MinimalRepoTests
    {
        private readonly ITestOutputHelper _output;
        private readonly TestProjectFixture _fixture;

        public MinimalRepoTests(ITestOutputHelper output, TestProjectFixture fixture)
        {
            _output = output;
            _fixture = fixture;
        }

        //[Fact(Skip = "https://github.com/dotnet/arcade/issues/7092")]
        [Fact]
        public void MinimalRepoBuildsWithoutErrors()
        {
            var app = _fixture.CreateTestApp("MinimalRepo");
            var exitCode = app.ExecuteBuild(_output,
                // these properties are required for projects that are not in a git repo
                "/p:EnableSourceLink=false",
                "/p:EnableSourceControlManagerQueries=false");
            Assert.Equal(0, exitCode);
        }

        [Fact(Skip = "https://github.com/dotnet/arcade/issues/7092")]
        public void MinimalRepoWithFinalVersions()
        {
            var app = _fixture.CreateTestApp("MinimalRepo");
            var exitCode = app.ExecuteBuild(_output,
                // these properties are required for projects that are not in a git repo
                "/p:EnableSourceLink=false",
                "/p:EnableSourceControlManagerQueries=false",
                "/p:DotNetFinalVersionKind=release");
            Assert.Equal(0, exitCode);
        }

#pragma warning disable CA1861 // Konstantenmatrizen als Argumente vermeiden
        [Theory]
        [InlineData("bin\\ClassLib1\\Debug\\netstandard2.0\\ClassLib1.dll", null)]
        [InlineData("obj\\ClassLib1\\Debug\\netstandard2.0\\ClassLib1.dll", null)]
        [InlineData("packages\\Debug\\Shipping\\ClassLib1.1.0.0.nupkg", new string[] { "-pack",  "/p:IsShippingPackage=true" })]
        [InlineData("packages\\Debug\\NonShipping\\ClassLib1.1.0.0.nupkg", new string[] { "-pack", "/p:IsShippingPackage=false" })]
        public void MinimalRepoBuildsUsesArtifactsOutputFolder(string expectedPath, string[] arguments=null)
        {
            var args= new List<string>(arguments?? Array.Empty<string>())
            {
                // these properties are required for projects that are not in a git repo
                "/p:EnableSourceLink=false",
                "/p:EnableSourceControlManagerQueries=false"
            };

            var app = _fixture.CreateTestApp("MinimalRepo");
            var exitCode = app.ExecuteBuild(_output,[.. args]);
            Assert.Equal(0, exitCode);

            var expectedArtifactsPath= Path.Join(app.WorkingDirectory, "artifacts", expectedPath);
            Assert.True(File.Exists(expectedArtifactsPath), $"Assert failed. Path not found.\nPath    :'{expectedArtifactsPath}'");
        }
#pragma warning restore CA1861 // Konstantenmatrizen als Argumente vermeiden
    }
}
