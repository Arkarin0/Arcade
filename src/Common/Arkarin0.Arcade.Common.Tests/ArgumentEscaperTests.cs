// Created/modified by Arkarin0 under one more more license(s).

using FluentAssertions;
using Arkarin0.Arcade.Common;
using Xunit;

namespace Arkarin0.DotNet.Arcade.Sdk.Tests
{
    public class ArgumentEscaperTests
    {
        [Fact]
        public void EscapesOnlyArgsWithSpecialCharacters()
        {
            var args = new[]
            {
                "subcommand",
                "--not-escaped",
                "1.0.0-prerelease.21165.2",
                "--with-space",
                "/mnt/d/Program Files",
                "--already-escaped",
                "\"some value\"",
                "containing-\"-quote",
            };

            string escaped = ArgumentEscaper.EscapeAndConcatenateArgArrayForProcessStart(args);
            escaped.Should().Be(
                "subcommand " +
                "--not-escaped 1.0.0-prerelease.21165.2 " +
                "--with-space \"/mnt/d/Program Files\" " +
                "--already-escaped \"some value\" " +
                "\"containing-\\\"-quote\"");
        }
    }
}
