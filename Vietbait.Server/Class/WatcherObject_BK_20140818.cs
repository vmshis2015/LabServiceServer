using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Threading;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Vietbait.Server.FileWatcherServer
{
    public class WatcherObject
    {
        #region Attributes

        private readonly List<string> _changedFiles = new List<string>();
        private readonly Timer _myTimer = new Timer(30000);
        private readonly BackgroundWorker _bgProcessFile = new BackgroundWorker();
        public Dictionary<string, FileSystemEventArgs> ListEvent = new Dictionary<string, FileSystemEventArgs>();
        public Dictionary<string, object> ListObject = new Dictionary<string, object>();
        public string Password = string.Empty;
        public string UserName = string.Empty;
        private FileSystemWatcher _fileWatcher;
        private DateTime _lastModifitime = DateTime.Now;
        public string WatchedPath { get; set; }

        public event OnChangeHandler Change;
        public event OnRenameHandler Rename;

        #endregion

        #region Contructor

        public WatcherObject(string resultPath)
        {
            WatchedPath = resultPath;
            _myTimer.Elapsed += _myTimer_Elapsed;
            _bgProcessFile.DoWork += BgProcessFileDoWork;
        }

        public WatcherObject(string resultPath, string userName, string passWord)
        {
            WatchedPath = resultPath;
            UserName = userName;
            Password = passWord;
            _myTimer.Elapsed += _myTimer_Elapsed;
            _bgProcessFile.DoWork += BgProcessFileDoWork;
        }

        public WatcherObject()
        {
            _myTimer.Elapsed += _myTimer_Elapsed;
            _bgProcessFile.DoWork += BgProcessFileDoWork;
        }

        #endregion

        #region Public Method

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public void StartWatch()
        {
            label1:
            // Create a new FileSystemWatcher and set its properties.
            _fileWatcher = new FileSystemWatcher
            {
                Path = WatchedPath,
                //NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                //               | NotifyFilters.FileName | NotifyFilters.DirectoryName
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName
            };


            //FileWatcher.Filter = "*.txt";
            // Add event handlers.
            _fileWatcher.Created += OnChanged;
            _fileWatcher.Changed += OnChanged;
            _fileWatcher.Renamed += OnRenamed;

            try
            {
                //Kiểm tra sự tồn tại của thư mục chứa file cần them dõi

                bool writeNotExists = false;

                while (!Directory.Exists(WatchedPath))
                    //while (!CheckPath(WatchedPath))
                {
                    if (!writeNotExists)
                    {
                        writeNotExists = true;
                        WriteLog(string.Format("Directory {0}{1}{2} not exist", "\"", WatchedPath, "\""));
                    }
                    //Nếu chưa tìm thấy thư mục cần theo dõi thì lặp lại liên tục
                    Thread.Sleep(2000);
                }

                _lastModifitime = GetlastModifiedTime();

                // Begin watching.
                _fileWatcher.EnableRaisingEvents = true;

                if (!_myTimer.Enabled) _myTimer.Start();

                // Wait for the user to quit the program.
                WriteLog(string.Format("OK! Folder {0}{1}{2} is monitored", "\"", WatchedPath, "\""));
                //while (Directory.Exists(WatchedPath))
                while (true)
                    //while (CheckPath(WatchedPath))
                {
                    ProcessQueue();
                    Thread.Sleep(2000);
                }
                WriteLog(string.Format("Directory {0}{1}{2} is Disconnected", "\"", WatchedPath, "\""));
                
                //FileWatcher.Renamed -= OnRenamed;
                goto label1;
            }
            catch (Exception ex)
            {
                _fileWatcher.Created -= OnChanged;
                _fileWatcher.Changed -= OnChanged;
                _fileWatcher.Renamed -= OnRenamed;

                WriteLog(string.Format("Error while watching folder - Detail:\r\n{0}", ex));
                Thread.Sleep(500);
                goto label1;
            }
        }

        public static bool CheckPath(string path)
        {
            if (string.IsNullOrEmpty(path)) return false;
            string pathRoot = Path.GetPathRoot(path);
            if (string.IsNullOrEmpty(pathRoot)) return false;
            var pinfo = new ProcessStartInfo("net", "use")
            {
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                UseShellExecute = false
            };
            string output;
            using (Process p = Process.Start(pinfo))
            {
                output = p.StandardOutput.ReadToEnd();
            }
            foreach (string line in output.Split('\n'))
            {
                if (line.Contains(pathRoot) && line.Contains("OK"))
                {
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region Private Method

        private void BgProcessFileDoWork(object sender, DoWorkEventArgs e)
        {
            while (_changedFiles.Count > 0)
            {
                string filePath = _changedFiles[0];
                try
                {
                    File.AppendAllText(filePath, @" ");
                    _lastModifitime = DateTime.Now;
                    _changedFiles.RemoveAt(0);
                    Thread.Sleep(1000);
                }
                catch (Exception ex)
                {
                    WriteLog(string.Format("Error while modify file '{1}' - Detail:\r\n{0}", ex, filePath));
                }
            }
        }

        private void _myTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _myTimer.Stop();
            // Lấy ra tên file mới
            try
            {
                IEnumerable<FileInfo> fileList = new DirectoryInfo(WatchedPath).GetFiles("*.txt",
                    SearchOption.TopDirectoryOnly);
                List<string> fileQuery = (from file in fileList
                    where
                        file.LastWriteTime > _lastModifitime &&
                        file.LastWriteTime <= DateTime.Now
                    orderby file.LastWriteTime
                    select file.FullName).ToList();

                if (!fileQuery.Any()) return;
                // Log các file bị thay đổi

                // Thêm vào danh sách file thay đổi
                _changedFiles.AddRange(fileQuery);

                WriteLog(string.Format("tìm thấy {0} files",fileQuery.Count));

                // phát sinh sự kiện cần xử lý
                if (!_bgProcessFile.IsBusy)
                {
                    WriteLog(string.Format("Chạy luồng xử lý"));
                    _bgProcessFile.RunWorkerAsync();
                }
            }
            catch
            {
            }
            finally
            {
                _myTimer.Start();
            }
        }

        /// <summary>
        ///     Lấy về thời gian thay đổi file cuối cùng
        /// </summary>
        /// <returns></returns>
        private DateTime GetlastModifiedTime()
        {
            try
            {
                // Bắt đầu lọc file từ thư mục.
                IEnumerable<FileInfo> fileList = new DirectoryInfo(WatchedPath).GetFiles("*.txt",
                    SearchOption.TopDirectoryOnly);
                return (from file in fileList
                    orderby file.LastWriteTime descending
                    select file.LastWriteTime).FirstOrDefault();
            }
            catch (Exception)
            {
                return DateTime.Now;
            }
        }

        private void ProcessQueue()
        {
            try
            {
                // Kiểm tra nếu trong mảng còn phần tử thì xử lý
                while (ListEvent.Count > 0)
                {
                    string currentKey = new List<string>(ListEvent.Keys)[0];
                    Change.Invoke(ListObject[currentKey], ListEvent[currentKey]);
                    ListEvent.Remove(currentKey);
                    ListObject.Remove(currentKey);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        ///     Hàm xử lý sự kiện khi thư mục theo dõi có file bị sửa, tạo mới và bị xóa
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void OnChanged(object source, FileSystemEventArgs e)
        {
            try
            {
                var key = e.FullPath;
                if (!ListEvent.ContainsKey(key))
                {
                    ListEvent.Add(key, e);
                }

                if (!ListObject.ContainsKey(key))
                {
                    ListObject.Add(key, source);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void OnRenamed(object source, RenamedEventArgs e)
        {
            try
            {
                _fileWatcher.EnableRaisingEvents = false;
                WriteLog(string.Format(@"File: {0}-{1}", e.FullPath, e.ChangeType));
                Rename.Invoke(source, e);
            }
            catch (Exception)
            {
            }
            finally
            {
                _fileWatcher.EnableRaisingEvents = true;
            }
        }

        private static void WriteLog(string content)
        {
            try
            {
                string watcherLogFolder = "WatcherLog";
                if (!Directory.Exists(watcherLogFolder)) Directory.CreateDirectory(watcherLogFolder);
                string watcherLogFilePath = string.Format("{0}{1}{2}.txt", watcherLogFolder,
                    Path.DirectorySeparatorChar,
                    DateTime.Now.ToString("yyyy-MM-dd"));
                if (string.IsNullOrEmpty(content.Trim()))
                {
                    File.AppendAllText(watcherLogFilePath,
                        string.Format("{0}{1}", (char) 13, (char) 10));
                }
                else
                {
                    File.AppendAllText(watcherLogFilePath,
                        string.Format("{0} :::: {1}{2}{3}",
                            DateTime.Now.ToString("dd/MM/yyyy - H:mm:ss.fff"),
                            content, (char) 13, (char) 10));
                }
            }
            catch (Exception ex)
            {
                //throw ex;
            }
        }

        #endregion
    }
}