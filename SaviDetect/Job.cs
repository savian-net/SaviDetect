using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Timer = System.Timers.Timer;

namespace SaviDetect
{
    public class Job
    {
        public string ProgramToExecute { get; set; }
        public string DirectoryToMonitor { get; set; }
        public List<string> UserArguments { get; set; }
        //public bool UsePolling { get; set; }
        public int DelayAfterFileDetection { get; set; }
        public bool AllowMultipleNotifications { get; set; }
        public bool ReturnActionTags { get; set; }
        public bool Changed { get; set; }
        public bool Created { get; set; }
        public bool Renamed { get; set; }
        public bool Deleted { get; set; }
        public int MillisecondsToWait { get; set; }

        public FileSystemWatcher FileWatcher { get; set; }

        bool isEventHandled = false;
        //private DateTime lastPollTime;
        List<FileInfo> lastFileCollection = new List<FileInfo>();

        readonly Timer _timer;

        public void Start()
        {
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }

        public Job()
        {
            _timer = new Timer(1000) { AutoReset = true };
        }

        public void Process()
        {
            if (!Directory.Exists(DirectoryToMonitor))
            {
                Log.Error($"Directory does not exist: {DirectoryToMonitor}");
                return;
            }
            //lastPollTime = DateTime.Now;
            //if (UsePolling)
            //{
            //    var files = new DirectoryInfo(DirectoryToMonitor).GetFiles();
            //    lastFileCollection.AddRange(files);
            //    var t = Task.Factory.StartNew(CreatePollingThread);
            //}
            //else
            //{
                CreateFileSystemWatcher();
            //}
        }

        //private async void CreatePollingThread()
        //{
        //    while (true)
        //    {
        //        var files =
        //            new DirectoryInfo(DirectoryToMonitor).GetFiles().Where(p => !p.Name.StartsWith("TeraCopy"))
        //                .ToList();
        //        if (Deleted)
        //        {
        //            var deletes = lastFileCollection.Where(p => files.All(p2 => p2.FullName != p.FullName)).ToList();
        //            foreach (var delFile in deletes)
        //            {
        //                CreateCurrentParmObject();
        //                this.Deleted = true;
        //                fsw_HandleFileEvent(delFile.FullName, WatcherChangeTypes.Deleted);
        //            }
        //        }

        //        if (Created)
        //        {
        //            var creates = files.Where(p => lastFileCollection.All(p2 => p2.FullName != p.FullName)).ToList();
        //            //Common.Log.Info("**** LAST: " + lastFileCollection.Count + "; FILES: " + files.Count + "; CREATES: " + creates.Count);
        //            foreach (var createdFile in creates)
        //            {
        //                if (Renamed && FileWasRenamed(createdFile))
        //                {
        //                    CreateCurrentParmObject();
        //                    this.Renamed = true;
        //                    fsw_HandleFileEvent(createdFile.FullName, WatcherChangeTypes.Renamed);
        //                }
        //                else
        //                {
        //                    CreateCurrentParmObject();
        //                    this.Created = true;
        //                    fsw_HandleFileEvent(createdFile.FullName, WatcherChangeTypes.Created);
        //                }
        //            }
        //        }

        //        if (Changed)
        //        {
        //            var changes = files.Where(p => p.LastWriteTime > lastPollTime);
        //            foreach (var changedFile in changes)
        //            {
        //                CreateCurrentParmObject();
        //                this.Changed = true;
        //                fsw_HandleFileEvent(changedFile.FullName, WatcherChangeTypes.Changed);
        //            }
        //        }

        //        lastFileCollection = new List<FileInfo>();
        //        lastFileCollection.AddRange(files);
        //        //Common.Log.Info("LAST: " + lastFileCollection.Count + "; FILES: " + files.Count); // + "; CREATES: " + creates.Count);
        //        //foreach (var fi in lastFileCollection)
        //        //{
        //        //    Common.Log.Info(fi.FullName);
        //        //}
        //        lastPollTime = DateTime.Now;
        //        Thread.Sleep( PollingDelay);
        //    }
        //}

        private bool FileWasRenamed(FileInfo fi)
        {
            var matchFound = lastFileCollection.Any(p => p.Length == fi.Length && p.CreationTime == fi.CreationTime);
            if (matchFound)
                return true;
            return false;
        }

        private void CreateFileSystemWatcher()
        {
            FileWatcher = new FileSystemWatcher(DirectoryToMonitor);
            FileWatcher.Filter = String.Empty;
            FileWatcher.EnableRaisingEvents = true;
            FileWatcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName |
                                       NotifyFilters.DirectoryName;
            HookUpEvents();
        }

        private void HookUpEvents()
        {
            try
            {
                if (Changed)
                {
                    FileWatcher.Changed += fsw_Changed;
                }

                if (Created)
                {
                    FileWatcher.Created += fsw_Created;
                }

                if (Deleted)
                {
                    FileWatcher.Deleted += fsw_Deleted;
                }

                if (Renamed)
                {
                    FileWatcher.Renamed += fsw_Renamed;
                }

            }
            catch (Exception ex)
            {
                Log.Error($"SaviDetect encountered an error on startup:", ex);
            }

        }

        private void CreateCurrentParmObject()
        {
            //this.Created = false;
            this.Changed = false;
            this.Deleted = false;
            this.Renamed = false;
            this.ProgramToExecute = this.ProgramToExecute;
            this.UserArguments = this.UserArguments;
            this.DirectoryToMonitor = this.DirectoryToMonitor;
        }

        void fsw_Changed(object sender, FileSystemEventArgs e)
        {
            CreateCurrentParmObject();
            this.Changed = true;
            fsw_HandleFileEvent(e.FullPath, e.ChangeType);
        }

        internal void fsw_Created(object sender, FileSystemEventArgs e)
        {
            CreateCurrentParmObject();
            this.Created = true;
            fsw_HandleFileEvent(e.FullPath, e.ChangeType);
        }

        void fsw_Deleted(object sender, FileSystemEventArgs e)
        {
            CreateCurrentParmObject();
            this.Deleted = true;
            fsw_HandleFileEvent(e.FullPath, e.ChangeType);
        }

        void fsw_Renamed(object sender, RenamedEventArgs e)
        {
            CreateCurrentParmObject();
            this.Renamed = true;
            fsw_HandleFileEvent(e.FullPath, e.ChangeType);
        }

        void fsw_HandleFileEvent(string fullPath, WatcherChangeTypes changeType)
        {
            if (!isEventHandled || AllowMultipleNotifications)
            {
                Log.Info($"Action detected: {fullPath} ==> {changeType}");
                if (this.DelayAfterFileDetection != 0)
                {
                    Log.Info($"Delay after action detected: {this.DelayAfterFileDetection} ms");
                    Thread.Sleep(this.DelayAfterFileDetection);
                }
                ProcessStartInfo psi = new ProcessStartInfo(this.ProgramToExecute);
                psi.Arguments = BuildArgumentList(fullPath, changeType);
                Log.Info($"Inititiate process: {ProgramToExecute} {psi.Arguments}");
                psi.UseShellExecute = false;
                psi.RedirectStandardOutput = true;
                psi.WindowStyle = ProcessWindowStyle.Hidden;
                try
                {
                    System.Diagnostics.Process process = System.Diagnostics.Process.Start(psi);
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("Executing the following command: " + psi.FileName + " " + psi.Arguments);
                    sb.Append("Process executed.");
                    var output = process.StandardOutput.ReadToEnd().Trim();
                    if (output != string.Empty)
                    {
                        sb.AppendLine("Standard output is: " + output);
                    }
                    else
                        sb.AppendLine();

                    Log.Info(sb.ToString());
                    process.WaitForExit(this.MillisecondsToWait);
                    isEventHandled = true;
                    //process.Exited += new EventHandler(process_Exited);
                }
                catch (IOException ex)
                {
                    Log.Error($"File not found. Arguments passed were: {psi.Arguments}", ex);
                }
                catch (Exception ex)
                {
                    Log.Error($"Process failed to execute. Process info: {psi.FileName} {psi.Arguments}", ex);
                }
            }
        }

        //void process_Exited(object sender, EventArgs e)
        //{
        //    System.Diagnostics.Process p = (System.Diagnostics.Process)sender;
        //    p.Close();
        //}

        private string BuildArgumentList(string fullPath, WatcherChangeTypes changeType)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                var fi = new FileInfo(fullPath);
                if (this.UserArguments != null)
                {
                    foreach (var s in this.UserArguments)
                    {
                        if (s.ToLower() == "[filename]")
                            sb.Append('"').Append(fi.Name).Append('"').Append(" ");
                        else if (s.ToLower() == "[dir]")
                            sb.Append('"').Append(fi.Directory).Append('"').Append(" ");
                        else if (s.ToLower() == "[fullname]")
                            sb.Append('"').Append(fi.FullName).Append('"').Append(" ");
                        else
                            sb.Append(s).Append(" ");
                    }
                }

                if (ReturnActionTags)
                {
                    sb.Append(this.Changed ? "type='" + changeType + "'" : string.Empty).Append(" ");
                }

                return sb.ToString();

            }
            catch (Exception ex)
            {
                Log.Error($"BuildArguments failed to execute", ex);
                return String.Empty;
            }
        }
    }
}

