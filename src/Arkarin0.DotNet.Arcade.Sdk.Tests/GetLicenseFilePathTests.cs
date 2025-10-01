// Created/modified by Arkarin0 under one more more license(s).

//using System;
//using System.IO;
//using Xunit;

//namespace Arkarin0.DotNet.Arcade.Sdk.Tests
//{
//    public class GetLicenseFilePathTests
//    {
//        [Theory]
//        [InlineData("licenSe.TXT")]
//        [InlineData("license.md")]
//        [InlineData("LICENSE")]
//        public void GetLicenseFilePath(string licenseFileName)
//        {
//            var dir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
//            Directory.CreateDirectory(dir);
//            var licensePath = Path.Combine(dir, licenseFileName);

//            File.WriteAllText(licensePath, "");

//            var task = new GetLicenseFilePath()
//            {
//                Directory = dir
//            };

//            bool result = task.Execute();
//            Assert.Equal(licensePath, task.Path);
//            Assert.True(result);

//            Directory.Delete(dir, recursive: true);
//        }
//    }
//}
