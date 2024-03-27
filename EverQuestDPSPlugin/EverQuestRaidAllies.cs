using Advanced_Combat_Tracker;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Security;

[assembly: AssemblyTitle("EverQuest Raid Allies")]
[assembly: AssemblyVersion("1.0.3.*")]
[assembly: AssemblyTrademark("Freemania")]
[assembly: AssemblyCopyright("Copyright © 2023")]
#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif
namespace EverQuestRaidAllies
{
    public class EverQuestRaidAllies : UserControl, IActPluginV1
    {
        #region Class Members
        Label lblStatus;
        //readonly String raidAllyFormat = @"(?<groupId>[\d]+)\s(?<playerName>\S+)\s(?<playerLevel>[\d]+)\s(?<playerClass>\S+)\s(?<raidRole>.+\b)";
        FileSystemWatcher watcher;
        private TextBox directoryPathTxtBox;
        private Button SelectDirectoryBtn;
        SettingsSerializer xmlSettings;
        string settingsFile;
        readonly string settingsFileName = $"Config{Path.DirectorySeparatorChar}EverQuestRaidAllies.config.xml";
        //readonly String raidAllyFileName = @"RaidRoster_(?<serverName>.+)-(?<date>[\d]+)-(?<time>[\d]+).txt";
        private ListBox listBox1;
        private System.ComponentModel.IContainer components;
        private Label label1;
        private string pluginName = "EverQuest Raid Allies";
        #endregion
        /// <summary>
        /// Constructor for Raid Allies plugin
        /// </summary>
        public EverQuestRaidAllies()
        {
            InitializeComponent();
        }
        
        void UpdateCheckClicked()
        {
            const int pluginId = 96;
            try
            {
                SecureString secureString = new SecureString();
                foreach (char c in ActGlobals.oFormActMain.PluginGetRemoteVersion(pluginId))
                    secureString.AppendChar(c);
                String remoteVersionFromGithub = Marshal.PtrToStringAuto(SecureStringMarshal.SecureStringToCoTaskMemUnicode(secureString));
                Version remoteVersion = new Version(remoteVersionFromGithub);
                Version currentVersion = typeof(EverQuestRaidAllies).Assembly.GetName().Version;
                if (remoteVersion > currentVersion)
                {
                    DialogResult result = MessageBox.Show($"There is an updated version of the {pluginName}.  Update it now?{Environment.NewLine}{Environment.NewLine}(If there is an update to ACT, you should click No and update ACT first.)", "New Version", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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
                            ChangeLblStatus($"Update for {pluginName} declined.");
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
        
        /// <summary>
        /// Initialize the User Interface component
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.directoryPathTxtBox = new System.Windows.Forms.TextBox();
            this.SelectDirectoryBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // directoryPathTxtBox
            // 
            this.directoryPathTxtBox.Location = new System.Drawing.Point(18, 84);
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
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(613, 9);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(275, 212);
            this.listBox1.TabIndex = 5;
            // 
            // EverQuestRaidAllies
            // 
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SelectDirectoryBtn);
            this.Controls.Add(this.directoryPathTxtBox);
            this.Name = "EverQuestRaidAllies";
            this.Size = new System.Drawing.Size(901, 236);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        /// <summary>
        /// Initialize the plugin based on the IActPluginV1 interface
        /// </summary>
        /// <param name="pluginScreenSpace"></param>
        /// <param name="pluginStatusText"></param>
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
            ChangeLblStatus("Plugin Started");
        }
        /// <summary>
        /// Actions needed to deinitialize the plugin from the IActPluginV1 interface
        /// </summary>
        public void DeInitPlugin()
        {
            SaveSettings();
            if (watcher != null)
            {
                watcher.Created -= Watcher_Created;
                watcher.Dispose();
            }
            lblStatus.Text = "Plugin Exited";
        }

        /// <summary>
        /// Set the directory to read the raid roster file from
        /// </summary>
        private void SetWatcherToDirectory()
        {
            if (Directory.Exists(directoryPathTxtBox.Text))
            {
                watcher = new FileSystemWatcher(Path.GetFullPath(directoryPathTxtBox.Text), "RaidRoster_*-*-*.txt")
                {
                    EnableRaisingEvents = true,
                    IncludeSubdirectories = false,
                    NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.Attributes | NotifyFilters.CreationTime
                };
                watcher.Created += Watcher_Created;
            }
            else
            {
                watcher.Dispose();
            }
        }
        /// <summary>
        /// Once a file is created read the lines from the roster and include them on the Raid Allies list for the encounter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Watcher_Created(object sender, FileSystemEventArgs e)
        {
            if (ActGlobals.oFormActMain.ActiveZone.ActiveEncounter == null)
            {
                ChangeLblStatus("No active encounter and no raid allies will be tracked.");
                return;
            }
            ChangeLblStatus($"Reading {e.FullPath}");
            Regex raidAllyLineRegex = new Regex(Properties.EverQuestRaidAllies.raidAllyFormat);
            Regex filename = new Regex(Properties.EverQuestRaidAllies.raidAllyFileName);
            Match m = filename.Match(e.Name);
            List<String> playerNames = new List<string>();
            if (e.ChangeType.HasFlag(WatcherChangeTypes.Created) && m.Success)
            {
                if (this.listBox1.InvokeRequired)
                {
                    this.listBox1.Invoke(new Action(() =>
                    {
                        this.listBox1.Items.Clear();
                    }));
                }
                else
                {
                    this.listBox1.Items.Clear();
                }
                List<CombatantData> data = new List<CombatantData>();

                using (StreamReader sr = new StreamReader(new FileStream(e.FullPath, FileMode.Open)))
                {
                    String line;

                    while ((line = sr.ReadLine()) != null)
                    {
                        Match raidLineMatch = raidAllyLineRegex.Match(line);
                        CombatantData combatantData = new CombatantData(raidLineMatch.Groups["playerName"].Value, ActGlobals.oFormActMain.ActiveZone.ActiveEncounter);

                        if (combatantData != null && combatantData.Name != ActGlobals.charName)
                        {
                            data.Add(combatantData);
                            if (this.listBox1.InvokeRequired)
                            {
                                this.listBox1.Invoke(new Action(() =>
                                {
                                    this.listBox1.Items.Add(combatantData.Name);
                                }));
                            }
                            else
                            {
                                this.listBox1.Items.Add(combatantData.Name);
                            }
                        }
                    }
                }
                
                ActGlobals.oFormActMain.ActiveZone.ActiveEncounter.SetAllies(data);
            }
        }
        /// <summary>
        /// Load the settings for the plugin
        /// </summary>
        private void LoadSettings()
        {
            xmlSettings.AddControlSetting(directoryPathTxtBox.Name, directoryPathTxtBox);
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
        /// <summary>
        /// Save the settings of the plugin
        /// </summary>
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

        #region UI Update Code
        /// <summary>
        /// Change the text of the label status in a thread safe approach
        /// </summary>
        /// <param name="status"></param>
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
        /// <summary>
        /// Change the EverQuest installation path directory in a thread safe approach
        /// </summary>
        /// <param name="status"></param>
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
        #endregion

        #region UI Interaction
        /// <summary>
        /// Open the directory choice dialog for reading the roster log file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectDirectoryBtn_Click(object sender, EventArgs e)
        {
            using (var EverQuestInstallPath = new FolderBrowserDialog())
            {
                DialogResult dr = EverQuestInstallPath.ShowDialog();

                if (dr == DialogResult.OK && Directory.Exists(EverQuestInstallPath.SelectedPath))
                {
                    ChangedbgLogFileTextValue(EverQuestInstallPath.SelectedPath);
                    SetWatcherToDirectory();
                    SaveSettings();
                }
                else
                {
                    MessageBox.Show($"{EverQuestInstallPath.SelectedPath} is not a valid path.", "File Name not as expected", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }
        }
        #endregion
        /// <summary>
        /// Update the directory path if it exists after a change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            ChangeLblStatus($"Raid roster text file directory changed to {directoryPathTxtBox.Text}.");
        }
    }
}
