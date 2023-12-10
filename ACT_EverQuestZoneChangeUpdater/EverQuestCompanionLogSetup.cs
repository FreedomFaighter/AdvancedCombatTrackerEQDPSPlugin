using Advanced_Combat_Tracker;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;


[assembly: AssemblyTitle("Advanced Combat Tracker EverQuest Zone Change Updater")]
[assembly: AssemblyVersion("1.0.0.*")]
[assembly: AssemblyTrademark("Freemania")]
[assembly: AssemblyCopyright("Copyright © 2023")]
#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif
namespace ACT_CompanionLogSetup
{
    public class EverQuestCompanionLogSetup : UserControl, IActPluginV1
    {
        #region Class Members
        Label lblStatus;
        readonly String zoneEnterLine = @"\[(?:.+)\](?:[\d]+):Received MSG_EQ_ADDPLAYER, Player = (?<characterEnteringZone>.+), zone = (?<ZoneName>.+)";
        FileSystemWatcher watcher;
        private TextBox directoryPathTxtBox;
        private Button SelectDirectoryBtn;
        StreamReader sr;
        String dbgFilePath;
        SettingsSerializer xmlSettings;
        string settingsFile;
        readonly string settingsFileName = $"Config{Path.DirectorySeparatorChar}EverQuestdbgtxtFile.config.xml";
        Regex zoneEnterLineRegex;
        bool readingLine = false;
        private Label label1;
        private CheckBox checkBox1;
        readonly string fileNameExpected = $"Logs{Path.DirectorySeparatorChar}dbg.txt";
        bool checkLogLineWithGlobalCharacterName;
        #endregion

        public EverQuestCompanionLogSetup()
        {
            InitializeComponent();
        }

        void UpdateCheckClicked()
        {
            const int pluginId = 94;
            try
            {
                String assemblyVersionRegex = @"Version=(?<AssemblyVersion>\S+)";
                Regex assemblyVersionFromRegexMatch = new Regex(assemblyVersionRegex, RegexOptions.Compiled);
                SecureString secureString = new SecureString();
                foreach (char c in ActGlobals.oFormActMain.PluginGetRemoteVersion(pluginId))
                    secureString.AppendChar(c);
                String remoteVersionFromGithub = Marshal.PtrToStringAuto(SecureStringMarshal.SecureStringToCoTaskMemUnicode(secureString));
                Version remoteVersion = new Version(remoteVersionFromGithub);
                Version currentVersion = typeof(EverQuestCompanionLogSetup).Assembly.GetName().Version;
                if (remoteVersion > currentVersion)
                {
                    DialogResult result = MessageBox.Show($"There is an updated version of the {this.GetType().Name}.  Update it now?{Environment.NewLine}{Environment.NewLine}(If there is an update to ACT, you should click No and update ACT first.)", "New Version", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    switch (result)
                    {
                        case DialogResult.Yes:
                            FileInfo updatedFile = ActGlobals.oFormActMain.PluginDownload(pluginId);
                            ActPluginData pluginData = ActGlobals.oFormActMain.PluginGetSelfData(this);
                            pluginData.pluginFile.Delete();
                            updatedFile.MoveTo(pluginData.pluginFile.FullName);
                            ThreadInvokes.CheckboxSetChecked(ActGlobals.oFormActMain, pluginData.cbEnabled, false);
                            Application.DoEvents();
                            ThreadInvokes.CheckboxSetChecked(ActGlobals.oFormActMain, pluginData.cbEnabled, true);
                            break;
                        case DialogResult.No:
                            ChangeLblStatus($"Update for {this.GetType().Name} declined.");
                            break;
                        default:
                            break;

                    }
                }
            }
            catch (Exception ex)
            {
                ActGlobals.oFormActMain.WriteExceptionLog(ex, "Plugin Update Check");
            }
        }

        private void InitializeComponent()
        {
            this.directoryPathTxtBox = new System.Windows.Forms.TextBox();
            this.SelectDirectoryBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // directoryPathTxtBox
            // 
            this.directoryPathTxtBox.Location = new System.Drawing.Point(3, 87);
            this.directoryPathTxtBox.Name = "directoryPathTxtBox";
            this.directoryPathTxtBox.ReadOnly = true;
            this.directoryPathTxtBox.Size = new System.Drawing.Size(576, 20);
            this.directoryPathTxtBox.TabIndex = 0;
            this.directoryPathTxtBox.TextChanged += new System.EventHandler(this.DirectoryPathTxtBox_TextChanged);
            // 
            // SelectDirectoryBtn
            // 
            this.SelectDirectoryBtn.Location = new System.Drawing.Point(48, 142);
            this.SelectDirectoryBtn.Name = "SelectDirectoryBtn";
            this.SelectDirectoryBtn.Size = new System.Drawing.Size(105, 55);
            this.SelectDirectoryBtn.TabIndex = 2;
            this.SelectDirectoryBtn.Text = "Select Current EverQuest Intalltion Directory";
            this.SelectDirectoryBtn.UseVisualStyleBackColor = true;
            this.SelectDirectoryBtn.Click += new System.EventHandler(this.SelectDirectoryBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(45, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(187, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Current EverQuest Intallation Directory";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(189, 142);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(387, 17);
            this.checkBox1.TabIndex = 4;
            this.checkBox1.Text = "Check name in zone log line with ACT Character Global Name before update";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.CheckBox1_CheckedChanged);
            // 
            // CompanionLogSetup
            // 
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SelectDirectoryBtn);
            this.Controls.Add(this.directoryPathTxtBox);
            this.Name = "CompanionLogSetup";
            this.Size = new System.Drawing.Size(579, 236);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        public void InitPlugin(TabPage pluginScreenSpace, Label pluginStatusText)
        {
            settingsFile = Path.Combine(ActGlobals.oFormActMain.AppDataFolder.FullName, settingsFileName);
            lblStatus = pluginStatusText;   // Hand the status label's reference to our local var
            pluginScreenSpace.Controls.Add(this);
            this.Dock = DockStyle.Fill;
            xmlSettings = new SettingsSerializer(this);
            watcher = new FileSystemWatcher();

            LoadSettings();
            if (ActGlobals.oFormActMain.GetAutomaticUpdatesAllowed())
            {// If ACT is set to automatically check for updates, check for updates to the plugin
                Task updateCheckClicked = new Task(() =>
                {
                    UpdateCheckClicked();
                });
                updateCheckClicked.Start();   // If we don't put this on a separate thread, web latency will delay the plugin init phase
            }
            zoneEnterLineRegex = new Regex(zoneEnterLine, RegexOptions.Compiled);
            ActGlobals.oFormActMain.ZoneChangeRegex = new Regex(@"\[(?:.+)\] You have entered (?!.*an area where levitation effects do not function)(?!.*the Drunken Monkey stance adequately)(?<zoneName>.*).", RegexOptions.Compiled);
            ChangeLblStatus("Plugin Started");
        }

        public void DeInitPlugin()
        {
            ActGlobals.oFormActMain.ChangeZone(String.Empty);
            sr?.Close();
            if (watcher != null)
            {
                watcher.Changed -= Watcher_Changed;
                watcher.Created -= Watcher_Created;
                watcher.Dispose();
            }
            SaveSettings();
            lblStatus.Text = "Plugin Exited";
        }

        private void SetWatcherToDirectory()
        {
            if (Directory.Exists(directoryPathTxtBox.Text))
            {
                dbgFilePath = Path.Combine(directoryPathTxtBox.Text, fileNameExpected);
                watcher = new FileSystemWatcher(Path.GetDirectoryName(dbgFilePath), "dbg.txt");
                watcher.EnableRaisingEvents = true;
                watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Attributes | NotifyFilters.CreationTime;
                watcher.Changed += Watcher_Changed;
                watcher.Created += Watcher_Created;
                if (File.Exists(dbgFilePath))
                {
                    sr = new StreamReader(new FileStream(dbgFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
                }
            }
            else
            {
                watcher.Dispose();
            }
        }

        private void Watcher_Created(object sender, FileSystemEventArgs e)
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

        private void LoadSettings()
        {
            xmlSettings.AddControlSetting(directoryPathTxtBox.Name, directoryPathTxtBox);
            xmlSettings.AddControlSetting(checkBox1.Name, checkBox1);
            if (File.Exists(settingsFile))
            {
                using (FileStream fs = new FileStream(settingsFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (XmlTextReader xReader = new XmlTextReader(fs))
                {
                    try
                    {
                        while (xReader.Read())
                        {
                            if (xReader.NodeType == XmlNodeType.Element)
                            {
                                if (xReader.LocalName == "SettingsSerializer")
                                    xmlSettings.ImportFromXml(xReader);
                            }
                        }
                    }
                    catch (ArgumentNullException ex)
                    {
                        ChangeLblStatus($"Argument Null for {ex.ParamName} with message: {ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        ChangeLblStatus($"With message: {ex.Message}");
                    }
                }

            }
            else
            {
                ChangeLblStatus($"{settingsFile} does not exist yet.");
                SaveSettings();
            }
        }

        private void SaveSettings()
        {
            try
            {
                using (FileStream fs = new FileStream(settingsFile, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (XmlTextWriter xWriter = new XmlTextWriter(fs, Encoding.UTF8))
                    {
                        xWriter.Formatting = Formatting.Indented;
                        xWriter.Indentation = 1;
                        xWriter.IndentChar = '\t';
                        xWriter.WriteStartDocument(true);
                        xWriter.WriteStartElement("Config");    // <Config>
                        xWriter.WriteStartElement("SettingsSerializer");    // <Config><SettingsSerializer>
                        xmlSettings.ExportToXml(xWriter);   // Fill the SettingsSerializer XML
                        xWriter.WriteEndElement();  // </SettingsSerializer>
                                                    //SaveXmlApostropheNameFix(xWriter);  // Create and fill the ApostropheNameFix node
                        xWriter.WriteEndElement();  // </Config>
                        xWriter.WriteEndDocument(); // Tie up loose ends (shouldn't be any)
                    }
                }
            }
            catch (Exception ex)
            {
                ActGlobals.oFormActMain.WriteExceptionLog(ex, "Failed to save file in entirety");
            }
        }

        private void ReadLineFromFileStreamAtPosition()
        {
            string line;
            readingLine = true;
            while ((line = sr.ReadLine()) != null)
            {
                Match m = zoneEnterLineRegex.Match(line);
                if (m.Success && (!checkLogLineWithGlobalCharacterName || (ActGlobals.charName == m.Groups["characterEnteringZone"].Value)))
                {
                    ActGlobals.oFormActMain.ChangeZone(m.Groups["ZoneName"].Value);
                }
            }
            readingLine = false;
        }

        #region UI Update Code
        private void ChangeLblStatus(String status)
        {
            switch (lblStatus.InvokeRequired)
            {
                case true:
                    this.lblStatus.Invoke(new Action(() =>
                    {
                        this.lblStatus.Text = status;
                    }));
                    break;
                case false:
                    this.lblStatus.Text = status;
                    break;
                default:
                    break;
            }
        }

        private void ChangedbgLogFileTextValue(String status)
        {
            switch (directoryPathTxtBox.InvokeRequired)
            {
                case true:
                    this.directoryPathTxtBox.Invoke(new Action(() =>
                    {
                        this.directoryPathTxtBox.Text = status;
                    }));
                    break;
                case false:
                    this.directoryPathTxtBox.Text = status;
                    break;
                default:
                    break;
            }
        }

        private void ChangeEditabilityOfLogFileTextValue(bool enabled)
        {
            if (directoryPathTxtBox.InvokeRequired)
            {
                this.directoryPathTxtBox.Invoke(new Action(() =>
                {
                    this.directoryPathTxtBox.Enabled = enabled;
                }));
            }
            else
                this.directoryPathTxtBox.Enabled = enabled;
        }
        #endregion

        #region UI Interaction
        private void SelectDirectoryBtn_Click(object sender, EventArgs e)
        {
            using (var dbgEverQuestInstallPath = new FolderBrowserDialog())
            {
                DialogResult dr = dbgEverQuestInstallPath.ShowDialog();

                if (dr == DialogResult.OK && Directory.Exists(dbgEverQuestInstallPath.SelectedPath))
                {
                    dbgFilePath = Path.Combine(dbgEverQuestInstallPath.SelectedPath, fileNameExpected);
                    ChangedbgLogFileTextValue(dbgEverQuestInstallPath.SelectedPath);
                }
                else
                {
                    ChangeLblStatus($"{dbgFilePath} does not exist after dialog OK.");
                    MessageBox.Show($"{fileNameExpected} need to be selected from the Logs directory of the EverQuest directory.", "File Name not as expected", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }
        }

        private void LockTxtDirectoryPathchkbx_CheckedChanged(object sender, EventArgs e)
        {
            var checkBx = sender as CheckBox;

            ChangeEditabilityOfLogFileTextValue(!checkBx.Checked);
        }
        #endregion

        private void DirectoryPathTxtBox_TextChanged(object sender, EventArgs e)
        {
            if (Directory.Exists(directoryPathTxtBox.Text))
            {
                SetWatcherToDirectory();
            }
            else
            {
                ChangeLblStatus("EverQuest install directory is missing from plugin settings.");
            }
            ChangeLblStatus($"dbg.txt log file changed to {dbgFilePath}.");
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.InvokeRequired)
            {
                checkBox1.Invoke(new Action(() =>
                {
                    this.checkLogLineWithGlobalCharacterName = checkBox1.Checked;
                }));
            }
            else
            {
                this.checkLogLineWithGlobalCharacterName = checkBox1.Checked;
            }
        }
    }
}
