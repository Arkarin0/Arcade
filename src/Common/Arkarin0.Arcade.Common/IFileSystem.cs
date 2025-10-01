// Created/modified by Arkarin0 under one more more license(s).

using System.IO;

#nullable enable
namespace Arkarin0.Arcade.Common
{
    public interface IFileSystem
    {
        void WriteToFile(string path, string content);

        bool FileExists(string path);

        bool DirectoryExists(string path);

        void CreateDirectory(string path);

        string? GetFileName(string? path);

        string? GetDirectoryName(string? path);

        string? GetFileNameWithoutExtension(string? path);

        string? GetExtension(string? path);

        string PathCombine(string path1, string path2);

        void DeleteFile(string path);

        void CopyFile(string sourceFileName, string destFileName, bool overwrite = false);

        Stream GetFileStream(string path, FileMode mode, FileAccess access);

        FileAttributes GetAttributes(string path);
    }
}
