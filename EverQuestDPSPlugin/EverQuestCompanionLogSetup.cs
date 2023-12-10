using Advanced_Combat_Tracker;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace EverQuestDPS
{
    public partial class EverQuestDPSPlugin
    {
        #region Class Members
        FileSystemWatcher watcherForDebugFile;
        StreamReader sr;
        String dbgFilePath;
        bool readingLine = false;
        readonly string fileNameExpected = $"Logs{Path.DirectorySeparatorChar}dbg.txt";
        Regex zoneEnterRgx;
        #endregion

        private void Watcher_CreatedForDebugFile(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType.HasFlag(WatcherChangeTypes.Created))
            {
                SetWatcherToDirectory();
            }
        }

        private void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType.HasFlag(WatcherChangeTypes.Changed) && !readingLine)
                ReadLineFromFileStreamAtPosition();
        }

        private void ReadLineFromFileStreamAtPosition()
        {
            string line;
            readingLine = true;
            while ((line = sr.ReadLine()) != null)
            {
                Match m = zoneEnterRgx.Match(line);
                if (m.Success && (ActGlobals.charName == m.Groups["characterEnteringZone"].Value))
                {
                    ActGlobals.oFormActMain.ChangeZone(m.Groups["ZoneName"].Value);
                }
            }
            readingLine = false;
        }

        #region UI Update Code
        private void ChangedbgLogFileTextValue(String status)
        {
            switch (directoryPathTB.InvokeRequired)
            {
                case true:
                    this.directoryPathTB.Invoke(new Action(() =>
                    {
                        this.directoryPathTB.Text = status;
                    }));
                    break;
                case false:
                    this.directoryPathTB.Text = status;
                    break;
                default:
                    break;
            }
        }

        private void ChangeEditabilityOfLogFileTextValue(bool enabled)
        {
            if (directoryPathTB.InvokeRequired)
            {
                this.directoryPathTB.Invoke(new Action(() =>
                {
                    this.directoryPathTB.Enabled = enabled;
                }));
            }
            else
                this.directoryPathTB.Enabled = enabled;
        }
        #endregion

        private void DirectoryPathTxtBox_TextChanged(object sender, EventArgs e)
        {
            if (Directory.Exists(directoryPathTB.Text))
            {
                SetWatcherToDirectory();
            }
            else
            {
                ChangeLblStatus("EverQuest install directory is missing from plugin settings.");
            }
            ChangeLblStatus($"dbg.txt log file changed to {dbgFilePath}.");
        }
    }
}
