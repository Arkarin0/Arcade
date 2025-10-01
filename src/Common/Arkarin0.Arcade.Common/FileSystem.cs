// Created/modified by Arkarin0 under one more more license(s).

using System.IO;

#nullable enable
namespace Arkarin0.Arcade.Common
{
    public class FileSystem : IFileSystem
    {
        public void CreateDirectory(string path) => Directory.CreateDirectory(path);

        public bool DirectoryExists(string path) => Directory.Exists(path);

        public bool FileExists(string path) => File.Exists(path);

        public void DeleteFile(string path) => File.Delete(path);

        public string? GetDirectoryName(string? path) => Path.GetDirectoryName(path);

        public string? GetFileName(string? path) => Path.GetFileName(path);

        public string? GetFileNameWithoutExtension(string? path) => Path.GetFileNameWithoutExtension(path);

        public string? GetExtension(string? path) => Path.GetExtension(path);

        public string PathCombine(string path1, string path2) => Path.Combine(path1, path2);

        public void WriteToFile(string path, string content)
        {
            string dirPath = Path.GetDirectoryName(path);
            Directory.CreateDirectory(dirPath);
            File.WriteAllText(path, content);
        }

        public void CopyFile(string sourceFileName, string destFileName, bool overwrite = false) => File.Copy(sourceFileName, destFileName, overwrite);

        public Stream GetFileStream(string path, FileMode mode, FileAccess access) => new FileStream(path, mode, access);

        public FileAttributes GetAttributes(string path) => File.GetAttributes(path);
    }
}
