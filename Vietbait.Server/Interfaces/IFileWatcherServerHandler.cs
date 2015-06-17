using System.IO;

namespace Vietbait.Server.FileWatcherServer
{
    internal interface IFileWatcherServerHandler
    {
        void OnChanged(object source, FileSystemEventArgs e);
        void OnRenamed(object source, RenamedEventArgs e);
    }
}