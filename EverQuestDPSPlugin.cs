using Advanced_Combat_Tracker;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

/*
 * Project: EverQuest DPS Plugin
 * Original: EverQuest 2 English DPS Localization plugin developed by EQAditu
 * Description: Missing from the arsenal of the plugin based Advanced Combat Tracker to track EverQuest's current combat messages.  Ignores chat as that is displayed in game.
 */

[assembly: AssemblyTitle("ACT EverQuest Damage Per Second Parsing")]
[assembly: AssemblyDescription("Worked based on EQAditu's EQ2 English DPS Parser, Plugin for ACT EverQuest Damage Per Second Parsing")]
[assembly: AssemblyCompany("Egot")]
[assembly: AssemblyVersion("0.0.*")]
[assembly: AssemblyCopyright("2023")]
#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif
namespace ACT_EverQuest_DPS_Plugin
{
    enum SpellDamageSave
    {
        Fire = 1,
        Cold = 2,
        Poison = 4,
        Disease = 8,
        Magic = 16,
        Corruption = 32
    }

    enum SpecialAttacks
    {
        Crippling_Blow = 1,
        Critical = 2,
        Flurry = 4,
        Locked = 8,
        Lucky = 16,
        Riposte = 32,
        Strikethrough = 64,
        Double_Bow_Shot = 128,
        Twincast = 256
    }

    public class EverQuestDPSPlugin : UserControl, IActPluginV1
    {
        #region Designer generated code (Avoid editing)
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cbMultiDamageIsOne = new System.Windows.Forms.CheckBox();
            this.cbRecalcWardedHits = new System.Windows.Forms.CheckBox();
            this.tbFixAncestralSentry = new System.Windows.Forms.TextBox();
            this.lblAncestralSentry = new System.Windows.Forms.Label();
            this.btnAposNameRemove = new System.Windows.Forms.Button();
            this.btnAposNameAdd = new System.Windows.Forms.Button();
            this.tbAposNameR = new System.Windows.Forms.TextBox();
            this.tbAposNameL = new System.Windows.Forms.TextBox();
            this.tbAposName = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.clbAposName = new System.Windows.Forms.CheckedListBox();
            this.cbSParseConsider = new System.Windows.Forms.CheckBox();
            this.cbIncludeInterceptFocus = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // cbMultiDamageIsOne
            // 
            this.cbMultiDamageIsOne.AutoSize = true;
            this.cbMultiDamageIsOne.Checked = true;
            this.cbMultiDamageIsOne.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbMultiDamageIsOne.Location = new System.Drawing.Point(3, 3);
            this.cbMultiDamageIsOne.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.cbMultiDamageIsOne.Name = "cbMultiDamageIsOne";
            this.cbMultiDamageIsOne.Size = new System.Drawing.Size(362, 17);
            this.cbMultiDamageIsOne.TabIndex = 5;
            this.cbMultiDamageIsOne.Text = "Record a hit with multiple damage types as a single hit. (Not retroactive)";
            this.cbMultiDamageIsOne.MouseHover += new System.EventHandler(this.cbMultiDamageIsOne_MouseHover);
            // 
            // cbRecalcWardedHits
            // 
            this.cbRecalcWardedHits.AutoSize = true;
            this.cbRecalcWardedHits.Checked = true;
            this.cbRecalcWardedHits.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbRecalcWardedHits.Location = new System.Drawing.Point(3, 22);
            this.cbRecalcWardedHits.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.cbRecalcWardedHits.Name = "cbRecalcWardedHits";
            this.cbRecalcWardedHits.Size = new System.Drawing.Size(368, 17);
            this.cbRecalcWardedHits.TabIndex = 7;
            this.cbRecalcWardedHits.Text = "Recalculate warded/intercepted hits to their true value.  (Not retroactive)";
            this.cbRecalcWardedHits.MouseHover += new System.EventHandler(this.cbRecalcWardedHits_MouseHover);
            // 
            // tbFixAncestralSentry
            // 
            this.tbFixAncestralSentry.Location = new System.Drawing.Point(488, 38);
            this.tbFixAncestralSentry.Name = "tbFixAncestralSentry";
            this.tbFixAncestralSentry.Size = new System.Drawing.Size(161, 20);
            this.tbFixAncestralSentry.TabIndex = 10;
            this.tbFixAncestralSentry.Text = "Ancestral Sentry";
            this.tbFixAncestralSentry.MouseHover += new System.EventHandler(this.lblAncestralSentry_MouseHover);
            // 
            // lblAncestralSentry
            // 
            this.lblAncestralSentry.AutoSize = true;
            this.lblAncestralSentry.Location = new System.Drawing.Point(3, 41);
            this.lblAncestralSentry.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.lblAncestralSentry.Name = "lblAncestralSentry";
            this.lblAncestralSentry.Size = new System.Drawing.Size(388, 13);
            this.lblAncestralSentry.TabIndex = 9;
            this.lblAncestralSentry.Text = "\"Ancestral Sentry\" interceding players will appear as a heal under the combatant:" +
    "";
            this.lblAncestralSentry.MouseHover += new System.EventHandler(this.lblAncestralSentry_MouseHover);
            // 
            // btnAposNameRemove
            // 
            this.btnAposNameRemove.Location = new System.Drawing.Point(559, 215);
            this.btnAposNameRemove.Name = "btnAposNameRemove";
            this.btnAposNameRemove.Size = new System.Drawing.Size(126, 23);
            this.btnAposNameRemove.TabIndex = 17;
            this.btnAposNameRemove.Text = "Remove Correction";
            this.btnAposNameRemove.UseVisualStyleBackColor = true;
            this.btnAposNameRemove.Click += new System.EventHandler(this.btnAposNameRemove_Click);
            this.btnAposNameRemove.MouseHover += new System.EventHandler(this.AposName_MouseHover);
            // 
            // btnAposNameAdd
            // 
            this.btnAposNameAdd.Location = new System.Drawing.Point(559, 190);
            this.btnAposNameAdd.Name = "btnAposNameAdd";
            this.btnAposNameAdd.Size = new System.Drawing.Size(126, 23);
            this.btnAposNameAdd.TabIndex = 16;
            this.btnAposNameAdd.Text = "Add Correction";
            this.btnAposNameAdd.UseVisualStyleBackColor = true;
            this.btnAposNameAdd.Click += new System.EventHandler(this.btnAposNameAdd_Click);
            this.btnAposNameAdd.MouseHover += new System.EventHandler(this.AposName_MouseHover);
            // 
            // tbAposNameR
            // 
            this.tbAposNameR.Enabled = false;
            this.tbAposNameR.Location = new System.Drawing.Point(499, 138);
            this.tbAposNameR.Name = "tbAposNameR";
            this.tbAposNameR.Size = new System.Drawing.Size(186, 20);
            this.tbAposNameR.TabIndex = 14;
            this.tbAposNameR.MouseHover += new System.EventHandler(this.AposName_MouseHover);
            // 
            // tbAposNameL
            // 
            this.tbAposNameL.Enabled = false;
            this.tbAposNameL.Location = new System.Drawing.Point(290, 138);
            this.tbAposNameL.Name = "tbAposNameL";
            this.tbAposNameL.Size = new System.Drawing.Size(186, 20);
            this.tbAposNameL.TabIndex = 12;
            this.tbAposNameL.MouseHover += new System.EventHandler(this.AposName_MouseHover);
            // 
            // tbAposName
            // 
            this.tbAposName.Location = new System.Drawing.Point(290, 164);
            this.tbAposName.Name = "tbAposName";
            this.tbAposName.Size = new System.Drawing.Size(395, 20);
            this.tbAposName.TabIndex = 15;
            this.tbAposName.TextChanged += new System.EventHandler(this.tbAposName_TextChanged);
            this.tbAposName.MouseHover += new System.EventHandler(this.AposName_MouseHover);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(287, 122);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(142, 13);
            this.label16.TabIndex = 13;
            this.label16.Text = "Apostrophe name to be fixed";
            // 
            // clbAposName
            // 
            this.clbAposName.FormattingEnabled = true;
            this.clbAposName.IntegralHeight = false;
            this.clbAposName.Location = new System.Drawing.Point(3, 122);
            this.clbAposName.Name = "clbAposName";
            this.clbAposName.Size = new System.Drawing.Size(278, 116);
            this.clbAposName.Sorted = true;
            this.clbAposName.TabIndex = 11;
            this.clbAposName.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.clbAposName_ItemCheck);
            this.clbAposName.SelectedIndexChanged += new System.EventHandler(this.clbAposName_SelectedIndexChanged);
            this.clbAposName.MouseHover += new System.EventHandler(this.AposName_MouseHover);
            // 
            // cbSParseConsider
            // 
            this.cbSParseConsider.AutoSize = true;
            this.cbSParseConsider.Checked = true;
            this.cbSParseConsider.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbSParseConsider.Location = new System.Drawing.Point(3, 56);
            this.cbSParseConsider.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.cbSParseConsider.Name = "cbSParseConsider";
            this.cbSParseConsider.Size = new System.Drawing.Size(479, 17);
            this.cbSParseConsider.TabIndex = 7;
            this.cbSParseConsider.Text = "Add characters marked by the /con, /whogroup, /whoraid command to the Selective P" +
    "arsing list";
            this.cbSParseConsider.MouseHover += new System.EventHandler(this.cbSParseConsider_MouseHover);
            // 
            // cbIncludeInterceptFocus
            // 
            this.cbIncludeInterceptFocus.AutoSize = true;
            this.cbIncludeInterceptFocus.Location = new System.Drawing.Point(3, 75);
            this.cbIncludeInterceptFocus.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.cbIncludeInterceptFocus.Name = "cbIncludeInterceptFocus";
            this.cbIncludeInterceptFocus.Size = new System.Drawing.Size(466, 17);
            this.cbIncludeInterceptFocus.TabIndex = 18;
            this.cbIncludeInterceptFocus.Text = "Parse focus damage done to channeler pets. (Skews attacker DPS/ToHit%, Not Retroa" +
    "ctive)";
            this.cbIncludeInterceptFocus.MouseHover += new System.EventHandler(this.cbIncludeInterceptFocus_MouseHover);
            // 
            // ACT_English_Parser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.cbIncludeInterceptFocus);
            this.Controls.Add(this.btnAposNameRemove);
            this.Controls.Add(this.btnAposNameAdd);
            this.Controls.Add(this.tbAposNameR);
            this.Controls.Add(this.tbAposNameL);
            this.Controls.Add(this.tbAposName);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.clbAposName);
            this.Controls.Add(this.tbFixAncestralSentry);
            this.Controls.Add(this.lblAncestralSentry);
            this.Controls.Add(this.cbMultiDamageIsOne);
            this.Controls.Add(this.cbRecalcWardedHits);
            this.Controls.Add(this.cbSParseConsider);
            this.Name = "ACT_English_Parser";
            this.Size = new System.Drawing.Size(688, 241);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private CheckBox cbMultiDamageIsOne;
        private CheckBox cbRecalcWardedHits;
        private TextBox tbFixAncestralSentry;
        private Label lblAncestralSentry;
        private Button btnAposNameRemove;
        private Button btnAposNameAdd;
        private TextBox tbAposNameR;
        private TextBox tbAposNameL;
        private TextBox tbAposName;
        private Label label16;
        private CheckBox cbSParseConsider;
        private CheckBox cbIncludeInterceptFocus;
        private CheckedListBox clbAposName;
        #endregion
        #region class members
        //readonly int pluginId = -1;
        readonly char[] chrApos = new char[] { '\'', '’' };
        readonly char[] chrSpaceApos = new char[] { ' ', '\'', '’' };
        List<Tuple<Color, Regex>> regexTupleList = new List<Tuple<Color, Regex>>();
        readonly String AlcoholConsumption = @"Glug, glug, glug...  (?<drinker>.+) take a swig of (?<typeOfAlcohol>.*)\.";
        readonly static String attackTypes = @"pierce|gore|crush|slash|hit|kick|slam|bash|shoot|strike|bite|grab|punch|scratch|rake";
        readonly static String evasionTypes = @"block(|s)|dodge(|s)|parr(ies|y)|riposte(|s)";
        readonly String DamageShield = @"(?<victim>.+) is (?<damageShieldDamageType>\S+) by (?<attacker>.+) (?<damageShieldType>\S+) for (?<damagePoints>[\d]+) points of non-melee damage.";
        readonly String DrinkConsumption = @"Glug, glug, glug...  (?<drinker>.+) take(|s) a drink from (?<possessivePersona>(your|their).+) (?<typeOfDrink>.*\.)";
        readonly String eqDateTimeStampFormat = @"ddd MMM dd HH:mm:ss yyyy";
        readonly String HealingOverTime = @"(?<healer>.+) healed (?<healingTarget>.+) over time for (?<healingPoints>[\d]+)(?:[\s\(](?<overHealPoints>[\d]+)[\)]){0,1} hit points by (?<healingSpell>.*\.)(?:[\s][\(](?<healingSpecial>.+)[\)]){0,1}";
        readonly String HitpointsHealingOverTime = @"Hit Points Healing Over Time";
        readonly String InstantHeal = @"(?<healer>.+) healed (?<healingTarget>.+) for (?<healingPoints>[\d]+)(?:[\s\(](?<overHealPoints>[\d]+)[\)]){0,1} hit points by (?<healingSpell>.*(\.))(?:[\s][\(](?<healingSpecial>.+)[\)]){0,1}";
        readonly String LootedCorpse = @"--(?<looter>.+) have looted a(?<loot>.+) from (?<looted>.+)'s corpse.--";
        readonly String MeleeAttack = @"(?<attacker>.+) " + $@"{attackTypes}" + @" (?<victim>.+) for (?<damageAmount>[\d]+) (?:point[|s]) of damage.(?:\s\((?<damageSpecial>.+)\)){0,1}";
        readonly String MissedMeleeAttack = $@"(?<attacker>.+) (?:(tr(ies|y))) to (?<attackType>\S+) (?<victim>.+), but (?:miss(|es))!";
        readonly String PetMelee = @"(?:(?<attacker>\S +)(`s pet))";
        readonly String PluginName = @"EverQuest Damage Per Second Parser";
        readonly static String PluginSettingsFileName = @"Config\ACT_EverQuest_English_Parser.config.xml";
        readonly String PluginSettingsSectionName = @"Data Correction\EQ English Settings";
        readonly String SlainMessage = @"(?<attacker>.+) ha(ve|s) slain (?<victim>.+)!";
        readonly String SpecialCripplingBlow = Enum.GetName(typeof(SpecialAttacks), (SpecialAttacks)SpecialAttacks.Crippling_Blow).Replace("_", " ");
        readonly String SpecialCritical = Enum.GetName(typeof(SpecialAttacks), (SpecialAttacks)SpecialAttacks.Critical).Replace("_", " ");
        readonly String SpecialDoubleBowShot = Enum.GetName(typeof(SpecialAttacks), (SpecialAttacks)SpecialAttacks.Double_Bow_Shot).Replace("_", " ");
        readonly String SpecialFlurry = Enum.GetName(typeof(SpecialAttacks), (SpecialAttacks)SpecialAttacks.Flurry).Replace("_", " ");
        readonly String SpecialLocked = Enum.GetName(typeof(SpecialAttacks), (SpecialAttacks)SpecialAttacks.Locked).Replace("_", " ");
        readonly String SpecialLucky = Enum.GetName(typeof(SpecialAttacks), (SpecialAttacks)SpecialAttacks.Lucky).Replace("_", " ");
        readonly String SpecialRiposte = Enum.GetName(typeof(SpecialAttacks), (SpecialAttacks)SpecialAttacks.Riposte).Replace("_", " ");
        readonly String SpecialStrikethrough = Enum.GetName(typeof(SpecialAttacks), (SpecialAttacks)SpecialAttacks.Strikethrough).Replace("_", " ");
        readonly String SpecialTwincast = Enum.GetName(typeof(SpecialAttacks), (SpecialAttacks)SpecialAttacks.Twincast).Replace("_", " ");
        readonly String SpellDamage = @"(?<attacker>.+) hit (?<victim>.+) for (?<damagePoints>[\d]+) (?:point[|s]) of (?<typeOfDamage>.+) damage by (?:(?<damageEffect>.*)\.)(?:\s\((?<spellSpeicals>.+)\))";
        static readonly String TimeStamp = @"\[(?<dateTimeOfLogLine>.+)\]";
        readonly String ZoneChange = @"You have entered (?!.*the Drunken Monkey stance adequately)(?<zoneName>.*)\.";
        readonly String LoadingPleaseWait = @"LOADING, PLEASE WAIT...";
        readonly String Unknown = @"(?<Unknown>(u|U)nknown)";
        readonly String logTimestamp = "logTimestamp";
        readonly String targetTooFarAway = @"Your target is too far away, get closer!";
        //readonly String tells = @"(?<teller>.+) tells (the|) (?<listener>.+), \'(<message>.+)\'";
        readonly String Evasion = @"(?<attacker>.*) tries to (?<attackType>\S+) (?:(?<victim>(.+)), but \1) (?:(?<evasionType>" + $@"{evasionTypes}" + @"))!(?:[\s][\(](?<evasionSpecial>.+)[\)]){0,1}";
        readonly String Banestrike = @"You hit (?<victim>.+) for (?<baneDamage>[\d]+) points of physical damage by Banestrike (?<baneAbilityRank>.+\.)";
        readonly Regex dateTimeRegex = new Regex(TimeStamp, RegexOptions.Compiled);
        Regex selfCheck = new Regex(@"((y|Y)ou|(YOU(?:(\b|R))(?:(\b|SELF))))", RegexOptions.Compiled);
        SortedList<string, AposNameFix> aposNameList = new SortedList<string, AposNameFix>();
        TreeNode optionsNode = null;
        Label lblStatus;    // The status label that appears in ACT's Plugin tab
        string settingsFile;
        SettingsSerializer xmlSettings;
        #endregion
        
        public EverQuestDPSPlugin()
        {
            InitializeComponent();
        }
        
        public void InitPlugin(TabPage pluginScreenSpace, Label pluginStatusText)
        {
            settingsFile = Path.Combine(ActGlobals.oFormActMain.AppDataFolder.FullName, PluginSettingsFileName);
            lblStatus = pluginStatusText;   // Hand the status label's reference to our local var
            pluginScreenSpace.Controls.Add(this);
            this.Dock = DockStyle.Fill;

            int dcIndex = -1;   // Find the Data Correction node in the Options tab
            for (int i = 0; i < ActGlobals.oFormActMain.OptionsTreeView.Nodes.Count; i++)
            {
                if (ActGlobals.oFormActMain.OptionsTreeView.Nodes[i].Text == "Data Correction")
                    dcIndex = i;
            }
            if (dcIndex != -1)
            {
                // Add our own node to the Data Correction node
                optionsNode = ActGlobals.oFormActMain.OptionsTreeView.Nodes[dcIndex].Nodes.Add($"{PluginName} Settings");
                // Register our user control(this) to our newly create node path.  All controls added to the list will be laid out left to right, top to bottom
                ActGlobals.oFormActMain.OptionsControlSets.Add($@"{PluginSettingsSectionName}", new List<Control> { this });
                Label lblConfig = new Label
                {
                    AutoSize = true,
                    Text = "Find the applicable options in the Options tab, Data Correction section."
                };
                pluginScreenSpace.Controls.Add(lblConfig);
            }

            xmlSettings = new SettingsSerializer(this); // Create a new settings serializer and pass it this instance
            LoadSettings();

            PopulateRegexArray();
            SetupEverQuestEnvironment();   // Not really needed since ACT has this code internalized as well.
            ActGlobals.oFormActMain.BeforeLogLineRead += new LogLineEventDelegate(oFormActMain_BeforeLogLineRead);
            //ActGlobals.oFormActMain.UpdateCheckClicked += new FormActMain.NullDelegate(oFormActMain_UpdateCheckClicked);
            ActGlobals.oFormActMain.GetDateTimeFromLog += new FormActMain.DateTimeLogParser(parseDateTime);
            //if (ActGlobals.oFormActMain.GetAutomaticUpdatesAllowed())   // If ACT is set to automatically check for updates, check for updates to the plugin
            //    new Thread(new ThreadStart(oFormActMain_UpdateCheckClicked)).Start();   // If we don't put this on a separate thread, web latency will delay the plugin init phase
            ActGlobals.oFormActMain.CharacterFileNameRegex = new Regex(@"(?:.+)[\\]eqlog_(?<characterName>\S+)_(?<server>.+).txt", RegexOptions.Compiled);
            ActGlobals.oFormActMain.ZoneChangeRegex = new Regex($@"{ZoneChange}", RegexOptions.Compiled);
            lblStatus.Text = $"{PluginName} Plugin Started";
        }
        
        public void DeInitPlugin()
        {
            ActGlobals.oFormActMain.BeforeLogLineRead -= oFormActMain_BeforeLogLineRead;
            //ActGlobals.oFormActMain.UpdateCheckClicked -= oFormActMain_UpdateCheckClicked;
            ActGlobals.oFormActMain.GetDateTimeFromLog -= parseDateTime;

            if (optionsNode != null)    // If we added our user control to the Options tab, remove it
            {
                optionsNode.Remove();
                ActGlobals.oFormActMain.OptionsControlSets.Remove($@"{PluginSettingsSectionName}");
            }

            SaveSettings();
            lblStatus.Text = $@"{PluginName} Plugin Exited";
        }

        private DateTime parseDateTime(String logLine)
        {
            DateTime currentEQTimeStamp;
            DateTime.TryParseExact(dateTimeRegex.Match(logLine).Groups["dateTimeOfLogLine"].Value, eqDateTimeStampFormat, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.AssumeLocal, out currentEQTimeStamp);
            return currentEQTimeStamp;
        }

        private void PopulateRegexArray()
        {
            ActGlobals.oFormEncounterLogs.LogTypeToColorMapping.Clear();
            regexTupleList.Add(new Tuple<Color, Regex>(Color.Red, new Regex($@"{TimeStamp} {MeleeAttack}", RegexOptions.Compiled)));
            ActGlobals.oFormEncounterLogs.LogTypeToColorMapping.Add(regexTupleList.Count - 1, regexTupleList[regexTupleList.Count - 1].Item1);
            regexTupleList.Add(new Tuple<Color, Regex>(Color.Red, new Regex($@"{TimeStamp} {DamageShield}", RegexOptions.Compiled)));
            ActGlobals.oFormEncounterLogs.LogTypeToColorMapping.Add(regexTupleList.Count - 1, regexTupleList[regexTupleList.Count - 1].Item1);
            regexTupleList.Add(new Tuple<Color, Regex>(Color.Gray, new Regex($@"{TimeStamp} {MissedMeleeAttack}", RegexOptions.Compiled)));
            ActGlobals.oFormEncounterLogs.LogTypeToColorMapping.Add(regexTupleList.Count - 1, regexTupleList[regexTupleList.Count - 1].Item1);
            regexTupleList.Add(new Tuple<Color, Regex>(Color.Goldenrod, new Regex($@"{TimeStamp} {SlainMessage}", RegexOptions.Compiled)));
            ActGlobals.oFormEncounterLogs.LogTypeToColorMapping.Add(regexTupleList.Count - 1, regexTupleList[regexTupleList.Count - 1].Item1);
            regexTupleList.Add(new Tuple<Color, Regex>(Color.Red, new Regex($@"{TimeStamp} {SpellDamage}", RegexOptions.Compiled)));
            ActGlobals.oFormEncounterLogs.LogTypeToColorMapping.Add(regexTupleList.Count - 1, regexTupleList[regexTupleList.Count - 1].Item1);
            regexTupleList.Add(new Tuple<Color, Regex>(Color.Green, new Regex($@"{TimeStamp} {HealingOverTime}", RegexOptions.Compiled)));
            ActGlobals.oFormEncounterLogs.LogTypeToColorMapping.Add(regexTupleList.Count - 1, regexTupleList[regexTupleList.Count - 1].Item1);
            regexTupleList.Add(new Tuple<Color, Regex>(Color.Maroon, new Regex($@"{TimeStamp} {PetMelee}", RegexOptions.Compiled)));
            ActGlobals.oFormEncounterLogs.LogTypeToColorMapping.Add(regexTupleList.Count - 1, regexTupleList[regexTupleList.Count - 1].Item1);
            regexTupleList.Add(new Tuple<Color, Regex>(Color.DarkBlue, new Regex($@"{TimeStamp} {ZoneChange}", RegexOptions.Compiled)));
            ActGlobals.oFormEncounterLogs.LogTypeToColorMapping.Add(regexTupleList.Count - 1, regexTupleList[regexTupleList.Count - 1].Item1);
            regexTupleList.Add(new Tuple<Color, Regex>(Color.BlueViolet, new Regex($@"{TimeStamp} {InstantHeal}", RegexOptions.Compiled)));
            ActGlobals.oFormEncounterLogs.LogTypeToColorMapping.Add(regexTupleList.Count - 1, regexTupleList[regexTupleList.Count - 1].Item1);
            regexTupleList.Add(new Tuple<Color, Regex>(Color.AliceBlue, new Regex($@"{TimeStamp} {LootedCorpse}", RegexOptions.Compiled)));
            ActGlobals.oFormEncounterLogs.LogTypeToColorMapping.Add(regexTupleList.Count - 1, regexTupleList[regexTupleList.Count - 1].Item1);
            regexTupleList.Add(new Tuple<Color, Regex>(Color.AliceBlue, new Regex($@"{TimeStamp} {AlcoholConsumption}", RegexOptions.Compiled)));
            ActGlobals.oFormEncounterLogs.LogTypeToColorMapping.Add(regexTupleList.Count - 1, regexTupleList[regexTupleList.Count - 1].Item1);
            regexTupleList.Add(new Tuple<Color, Regex>(Color.AliceBlue, new Regex($@"{TimeStamp} {DrinkConsumption}", RegexOptions.Compiled)));
            ActGlobals.oFormEncounterLogs.LogTypeToColorMapping.Add(regexTupleList.Count - 1, regexTupleList[regexTupleList.Count - 1].Item1);
            regexTupleList.Add(new Tuple<Color, Regex>(Color.Azure, new Regex($@"{TimeStamp} {LoadingPleaseWait}", RegexOptions.Compiled)));
            ActGlobals.oFormEncounterLogs.LogTypeToColorMapping.Add(regexTupleList.Count - 1, regexTupleList[regexTupleList.Count - 1].Item1);
            regexTupleList.Add(new Tuple<Color, Regex>(Color.Silver, new Regex($@"{TimeStamp} {Unknown}", RegexOptions.Compiled)));
            ActGlobals.oFormEncounterLogs.LogTypeToColorMapping.Add(regexTupleList.Count - 1, regexTupleList[regexTupleList.Count - 1].Item1);
            regexTupleList.Add(new Tuple<Color, Regex>(Color.Snow, new Regex($@"{TimeStamp} {targetTooFarAway}", RegexOptions.Compiled)));
            ActGlobals.oFormEncounterLogs.LogTypeToColorMapping.Add(regexTupleList.Count - 1, regexTupleList[regexTupleList.Count - 1].Item1);
            regexTupleList.Add(new Tuple<Color, Regex>(Color.DeepSkyBlue, new Regex($@"{TimeStamp} {Evasion}", RegexOptions.Compiled)));
            ActGlobals.oFormEncounterLogs.LogTypeToColorMapping.Add(regexTupleList.Count - 1, regexTupleList[regexTupleList.Count - 1].Item1);

        }
        
        void oFormActMain_BeforeLogLineRead(bool isImport, LogLineEventArgs logInfo)
        {
            for (int i = 0; i < regexTupleList.Count; i++)
            {
                Match reMatch = regexTupleList[i].Item2.Match(logInfo.logLine);
                if (reMatch.Success)
                {
                    logInfo.detectedType = i + 1;
                    ParseEverQuestLogLine(reMatch, i);
                    break;
                }
            }
        }
        
        private DateTime GetDateTimeFromGroupMatch(String dt)
        {
            DateTime currentEQTimeStamp;
            DateTime.TryParseExact(dt, eqDateTimeStampFormat, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.AssumeLocal, out currentEQTimeStamp);
            return currentEQTimeStamp;
        }
        
        enum EverQuestSwingType : int
        {
            None = 0,
            Melee = 1,
            NonMelee = 2,
            InstantHealing = 4,
            HealOverTime = 8,
            Bane = 16,
            PetMelee = 32,
            PetNonMelee = 64,
        }

        private void ParseEverQuestLogLine(Match reMatch, int logMatched)
        {
            switch (logMatched)
            {
                //Melee
                case 0:
                    if (ActGlobals.oFormActMain.SetEncounter(ActGlobals.oFormActMain.LastKnownTime, CharacterNamePersonaReplace(reMatch.Groups["attacker"].Value), CharacterNamePersonaReplace(reMatch.Groups["victim"].Value)))
                    {
                        MasterSwing masterSwingMelee = new MasterSwing((int)SwingTypeEnum.Melee
                            , reMatch.Groups["damageSpecial"].Success ? (reMatch.Groups["damageSpecial"].Value.Contains(SpecialCritical) ? reMatch.Groups["damageSpecial"].Value.Contains(SpecialCritical) : false) : false, reMatch.Groups["damageSpecial"].Success ? reMatch.Groups["damageSpecial"].Value : String.Empty, new Dnum(Int64.Parse(reMatch.Groups["damageAmount"].Value))
                            , ActGlobals.oFormActMain.LastEstimatedTime, ActGlobals.oFormActMain.GlobalTimeSorter, reMatch.Groups["attackType"].Value, CharacterNamePersonaReplace(reMatch.Groups["attacker"].Value), "Hitpoints", CharacterNamePersonaReplace(reMatch.Groups["victim"].Value));
                        masterSwingMelee.Tags[logTimestamp] = ActGlobals.oFormActMain.LastKnownTime;
                        ActGlobals.oFormActMain.AddCombatAction(masterSwingMelee);
                    }
                    break;
                //Non-melee damage shield
                case 1:
                    if (ActGlobals.oFormActMain.SetEncounter(ActGlobals.oFormActMain.LastKnownTime, CharacterNamePersonaReplace(reMatch.Groups["attacker"].Value), CharacterNamePersonaReplace(reMatch.Groups["victim"].Value)))
                    {
                        MasterSwing masterSwingDamageShield = new MasterSwing((int)EverQuestSwingType.NonMelee
                            , reMatch.Groups["damageSpecial"].Success ? (reMatch.Groups["damageSpecial"].Value.Contains(SpecialCritical) ? reMatch.Groups["damageSpecial"].Value.Contains(SpecialCritical) : false) : false, reMatch.Groups["damageSpecial"].Success ? reMatch.Groups["damageSpecial"].Value : String.Empty
                            , new Dnum(Int64.Parse(reMatch.Groups["damagePoints"].Value), reMatch.Groups["damageShieldType"].Value)
                            , ActGlobals.oFormActMain.LastEstimatedTime, ActGlobals.oFormActMain.GlobalTimeSorter, reMatch.Groups["damageShieldDamageType"].Value, CharacterNamePersonaReplace(reMatch.Groups["attacker"].Value), "Hitpoints", CharacterNamePersonaReplace(reMatch.Groups["victim"].Value));
                        masterSwingDamageShield.Tags[logTimestamp] = ActGlobals.oFormActMain.LastKnownTime;
                        ActGlobals.oFormActMain.AddCombatAction(masterSwingDamageShield);
                    }
                    break;
                //Missed melee
                case 2:
                    if (ActGlobals.oFormActMain.SetEncounter(ActGlobals.oFormActMain.LastKnownTime, CharacterNamePersonaReplace(reMatch.Groups["attacker"].Value), CharacterNamePersonaReplace(reMatch.Groups["victim"].Value)))
                    {
                        MasterSwing masterSwingMissedMelee = new MasterSwing((int)SwingTypeEnum.Melee
                            , reMatch.Groups["damageSpecial"].Success ? (reMatch.Groups["damageSpecial"].Value.Contains(SpecialCritical) ? reMatch.Groups["damageSpecial"].Value.Contains(SpecialCritical) : false) : false,
                            reMatch.Groups["damageSpecial"].Success ? reMatch.Groups["damageSpecial"].Value : String.Empty
                        , Dnum.Miss
                            , ActGlobals.oFormActMain.LastEstimatedTime, ActGlobals.oFormActMain.GlobalTimeSorter, reMatch.Groups["attackType"].Value, CharacterNamePersonaReplace(reMatch.Groups["attacker"].Value), "Miss", CharacterNamePersonaReplace(reMatch.Groups["victim"].Value));
                        masterSwingMissedMelee.Tags[logTimestamp] = ActGlobals.oFormActMain.LastKnownTime;
                        ActGlobals.oFormActMain.AddCombatAction(masterSwingMissedMelee);
                    }
                    break;
                //Spell Cast
                case 4:
                    if (ActGlobals.oFormActMain.SetEncounter(ActGlobals.oFormActMain.LastKnownTime, CharacterNamePersonaReplace(reMatch.Groups["attacker"].Value), CharacterNamePersonaReplace(reMatch.Groups["victim"].Value)))
                    {
                        Dnum damage = new Dnum(Int64.Parse(reMatch.Groups["damagePoints"].Value), reMatch.Groups["typeOfDamage"].Value);
                        MasterSwing masterSwingSpellcast = new MasterSwing((int)SwingTypeEnum.NonMelee
                            , reMatch.Groups["spellSpecials"].Success ? (reMatch.Groups["spellSpecials"].Value.Contains(SpecialCritical) ? reMatch.Groups["spellSpecials"].Value.Contains(SpecialCritical) : false) : false
                            , reMatch.Groups["spellSpeicals"].Success ? reMatch.Groups["spellSpeicals"].Value : String.Empty
                            , damage, ActGlobals.oFormActMain.LastEstimatedTime
                            , ActGlobals.oFormActMain.GlobalTimeSorter, reMatch.Groups["damageEffect"].Value
                            , CharacterNamePersonaReplace(reMatch.Groups["attacker"].Value)
                            , "Hitpoints"
                            , CharacterNamePersonaReplace(reMatch.Groups["victim"].Value)
                        );
                        masterSwingSpellcast.Tags[logTimestamp] = ActGlobals.oFormActMain.LastKnownTime;
                        ActGlobals.oFormActMain.AddCombatAction(masterSwingSpellcast);
                    }
                    break;
                //Heal Over Time heal
                case 5:
                    if (ActGlobals.oFormActMain.InCombat)
                    {
                        MasterSwing masterSwingHealOverTime = new MasterSwing((int)EverQuestSwingType.HealOverTime, reMatch.Groups["healingSpecial"].Success ? (reMatch.Groups["healingSpecial"].Value.Contains(SpecialCritical) ? reMatch.Groups["healingSpecial"].Value.Contains(SpecialCritical) : false) : false
                            , reMatch.Groups["healingSpecial"].Success ? reMatch.Groups["healingSpecial"].Value : String.Empty
                            , new Dnum(Int64.Parse(reMatch.Groups["healingPoints"].Value))
                            , ActGlobals.oFormActMain.LastEstimatedTime
                            , ActGlobals.oFormActMain.GlobalTimeSorter
                            , reMatch.Groups["healingSpell"].Value
                            , CharacterNamePersonaReplace(reMatch.Groups["healer"].Value)
                            , "Hitpoints", reMatch.Groups["healingTarget"].Value.Contains("self") ? CharacterNamePersonaReplace(reMatch.Groups["healer"].Value) : CharacterNamePersonaReplace(reMatch.Groups["healingTarget"].Value));
                        masterSwingHealOverTime.Tags["overheal"] = reMatch.Groups["overHealPoints"].Success ? Int64.Parse(reMatch.Groups["overHealPoints"].Value) : 0;
                        masterSwingHealOverTime.Tags[logTimestamp] = ActGlobals.oFormActMain.LastKnownTime;
                        ActGlobals.oFormActMain.AddCombatAction(masterSwingHealOverTime);
                    }
                    break;
                //slain message
                case 6:
                    MasterSwing masterSwingSlain = new MasterSwing(0, false, Dnum.Death, ActGlobals.oFormActMain.LastKnownTime, ActGlobals.oFormActMain.GlobalTimeSorter, String.Empty, CharacterNamePersonaReplace(reMatch.Groups["attacker"].Value), String.Empty, CharacterNamePersonaReplace(reMatch.Groups["victim"].Value));
                    ActGlobals.oFormActMain.AddCombatAction(masterSwingSlain);
                    break;
                //Zone change
                case 7:
                    //when checking the HistoryRecord the EndTime should be compared against default(DateTime) to determine if it an exact value among other methods such does the default(DateTime) take place before the StartTime for the HistoryRecord
                    ActGlobals.oFormActMain.ZoneDatabaseAdd(new HistoryRecord(0, ActGlobals.oFormActMain.LastKnownTime, new DateTime(), reMatch.Groups["zoneName"].Value != String.Empty ? reMatch.Groups["zoneName"].Value : throw new Exception("Zone regex triggered but zone name not found."), ActGlobals.charName));
                    ActGlobals.oFormActMain.ChangeZone(ActGlobals.oFormActMain.ZoneDatabase[ActGlobals.oFormActMain.LastKnownTime].Label);
                    break;
                //Instant heals
                case 8:
                    if (ActGlobals.oFormActMain.InCombat)
                    {
                        String healingSpecial = reMatch.Groups["healingSpecial"].Value;
                        MasterSwing masterSwingInstantHeal = new MasterSwing((int)EverQuestSwingType.InstantHealing, healingSpecial.Contains(SpecialCritical)
                            , healingSpecial
                            , new Dnum(Int64.Parse(reMatch.Groups["healingPoints"].Value))
                            , ActGlobals.oFormActMain.LastEstimatedTime
                            , ActGlobals.oFormActMain.GlobalTimeSorter
                            , reMatch.Groups["healingSpell"].Value
                            , CharacterNamePersonaReplace(reMatch.Groups["healer"].Value)
                            , "Hitpoints", reMatch.Groups["healingTarget"].Value.Contains("self") ? CharacterNamePersonaReplace(reMatch.Groups["healer"].Value) : CharacterNamePersonaReplace(reMatch.Groups["healingTarget"].Value));
                        masterSwingInstantHeal.Tags["overheal"] = reMatch.Groups["overHealPoints"].Success ? Int64.Parse(reMatch.Groups["overHealPoints"].Value) : 0;
                        masterSwingInstantHeal.Tags[logTimestamp] = ActGlobals.oFormActMain.LastKnownTime;
                        ActGlobals.oFormActMain.AddCombatAction(masterSwingInstantHeal);
                    }
                    break;
                //Loot line for future use
                case 9:
                    String looter = CharacterNamePersonaReplace(reMatch.Groups["looter"].Value);
                    String loot = reMatch.Groups["loot"].Value;
                    String looted = CharacterNamePersonaReplace(reMatch.Groups["looted"].Value);
                    break;
                //Alcohol drink
                case 10:
                    String alcoholDrinker = CharacterNamePersonaReplace(reMatch.Groups["drinker"].Value);
                    String typeOfAlcohol = reMatch.Groups["typeOfAlcohol"].Value;
                    break;
                //Character drink consumption "You are thirsty."
                case 11:
                    String drinkDrinker = CharacterNamePersonaReplace(reMatch.Groups["drinker"].Value);
                    String typeOfDrink = reMatch.Groups["typeOfDrink"].Value;
                    bool possesiveMatchWithCharacterNamePersona = ((CharacterNamePersonaReplace(reMatch.Groups["drinker"].Value) == ActGlobals.charName && reMatch.Groups["possessivePersona"].Value.Equals("your")) || (CharacterNamePersonaReplace(reMatch.Groups["drinker"].Value) != ActGlobals.charName && reMatch.Groups["possessivePersona"].Value.Equals("their"))) ? true : false;
                    if (possesiveMatchWithCharacterNamePersona)
                        break;
                    else
                        throw new Exception($"Possesive persona of action doesn't match drinker.  'They made {CharacterNamePersonaReplace(reMatch.Groups["drinker"].Value)} do it' {reMatch.Groups["drinker"].Value} != {reMatch.Groups["possesivePersona"].Value}.");
                case 12:
                    _ = ActGlobals.oFormActMain.ZoneDatabase[ActGlobals.oFormActMain.ZoneDatabase[ActGlobals.oFormActMain.ZoneDatabase.Max().Key].Label.Equals(ActGlobals.oFormActMain.CurrentZone) ? ActGlobals.oFormActMain.ZoneDatabase[ActGlobals.oFormActMain.ZoneDatabase.Max().Key].EndTime = ActGlobals.oFormActMain.LastKnownTime : throw new Exception("unable to determine last zone and time from log file")];
                    break;
                //Lines with unknown in the logline
                case 13:
                    MasterSwing masterSwingUnknown = new MasterSwing((int)SwingTypeEnum.NonMelee, false, Dnum.Unknown, ActGlobals.oFormActMain.LastEstimatedTime, ActGlobals.oFormActMain.GlobalTimeSorter, "Unknown", "Unknown", "Unknown", "Unknown");
                    break;
                case 15:
                    if (ActGlobals.oFormActMain.SetEncounter(ActGlobals.oFormActMain.LastKnownTime, CharacterNamePersonaReplace(reMatch.Groups["attacker"].Value), CharacterNamePersonaReplace(reMatch.Groups["victim"].Value)))
                    {
                        MasterSwing masterSwingEvasion = new MasterSwing((int)EverQuestSwingType.Melee, false, reMatch.Groups["evasionSpecial"].Value, Dnum.NoDamage, ActGlobals.oFormActMain.LastEstimatedTime, ActGlobals.oFormActMain.GlobalTimeSorter, reMatch.Groups["attackType"].Value, CharacterNamePersonaReplace(reMatch.Groups["attacker"].Value), reMatch.Groups["evasionType"].Value, CharacterNamePersonaReplace(reMatch.Groups["victim"].Value));
                        masterSwingEvasion.Tags[logTimestamp] = ActGlobals.oFormActMain.LastKnownTime;
                        ActGlobals.oFormActMain.AddCombatAction(masterSwingEvasion);
                    }
                    break;
                default:
                    break;
            }
        }
        
        private void SplitAttackerSkill(ref string attacker, ref string skillType, string[] engNameSkillSplit)
        {
            attacker = string.Empty;    // It wasn't a pet
            for (int i = 0; i < engNameSkillSplit.Length; i++)
            {
                string str = engNameSkillSplit[i];
                attacker += "'" + str;      // Join the attacker name pieces together
                string strNext = string.Empty;
                if (i + 1 != engNameSkillSplit.Length)
                {
                    strNext = engNameSkillSplit[i + 1];
                }
                bool valid = false;
                if (strNext != string.Empty)
                {
                    if (strNext.StartsWith("s "))
                        valid = true;
                    if (strNext[0] == ' ')
                    {
                        if (str[str.Length - 1] == 's' || str[str.Length - 1] == 'z')
                            valid = true;
                    }
                }
                if (valid)  // Until you find a grammatically correct usage of appostrophes
                {
                    skillType = string.Empty;
                    for (int j = i + 1; j < engNameSkillSplit.Length; j++)
                    {
                        skillType += "'" + engNameSkillSplit[j];    // Anything left of the split string is the skillType
                    }
                    skillType = skillType.TrimStart(chrSpaceApos);      // Remove the appostrophe and spaces at the begining of the string
                    if (skillType.StartsWith("s "))
                        skillType = skillType.Substring(2);
                    break;
                }
            }
        }
        
        private string CharacterNamePersonaReplace(string PersonaString)
        {
            return selfCheck.Match(PersonaString).Success ? ActGlobals.charName : PersonaString;
        }
        
        void LoadSettings()
        {
            // Add items to the xmlSettings object here...
            xmlSettings.AddControlSetting(cbMultiDamageIsOne.Name, cbMultiDamageIsOne);
            xmlSettings.AddControlSetting(cbRecalcWardedHits.Name, cbRecalcWardedHits);
            xmlSettings.AddControlSetting(cbIncludeInterceptFocus.Name, cbIncludeInterceptFocus);
            xmlSettings.AddControlSetting(tbFixAncestralSentry.Name, tbFixAncestralSentry);
            xmlSettings.AddControlSetting(cbSParseConsider.Name, cbSParseConsider);

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
                                if (xReader.LocalName == "ApostropheNameFix")
                                    LoadXmlApostropheNameFix(xReader);

                            }
                        }
                    }
                    catch (ArgumentNullException ex)
                    {
                        lblStatus.Text = "Argument Null for " + ex.ParamName + " with message: " + ex.Message;
                    }
                    catch (Exception ex)
                    {
                        lblStatus.Text = "With message: " + ex.Message;
                    }
                }
            }
        }
        
        void SaveSettings()
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
                    SaveXmlApostropheNameFix(xWriter);  // Create and fill the ApostropheNameFix node
                    xWriter.WriteEndElement();  // </Config>
                    xWriter.WriteEndDocument(); // Tie up loose ends (shouldn't be any)
                }
            }
        }
        
        internal class AposNameFix : IEquatable<AposNameFix>
        {
            string left, right, fullName;
            public string FullName
            {
                get
                {
                    return fullName;
                }
                set
                {
                    fullName = value;
                }
            }
            public string Right
            {
                get
                {
                    return right;
                }
                set
                {
                    right = value;
                }
            }
            public string Left
            {
                get
                {
                    return left;
                }
                set
                {
                    left = value;
                }
            }
            bool active = true;
            public bool Active
            {
                get
                {
                    return active;
                }
                set
                {
                    active = value;
                }
            }

            public AposNameFix(string FullName, string Left, string Right)
            {
                this.left = Left;
                this.right = Right;
                this.fullName = FullName;
            }
            public bool IsMatch(CombatActionEventArgs e)
            {
                if (e.attacker == left && e.theAttackType.StartsWith(right))
                    return true;
                return false;
            }
            public bool Fix(CombatActionEventArgs e)
            {
                if (!active)
                    return false;
                if (e.theAttackType == right)
                {
                    e.swingType = (int)SwingTypeEnum.Melee;
                    if (e.attacker[e.attacker.Length - 1] == 's' || e.attacker[e.attacker.Length - 1] == 'z')
                        e.attacker += "' ";
                    else
                        e.attacker += "'s ";
                    e.attacker += right;
                    e.theAttackType = e.theDamageType.ToString();
                    return true;
                }
                if (e.theAttackType.StartsWith(right + "'"))
                {
                    int trimLen = right.Length;
                    if (e.attacker[e.attacker.Length - 1] == 's' || e.attacker[e.attacker.Length - 1] == 'z')
                    {
                        e.attacker += "' ";
                        trimLen += 2;
                    }
                    else
                    {
                        e.attacker += "'s ";
                        trimLen += 3;
                    }
                    e.attacker += right;
                    e.theAttackType = e.theAttackType.Substring(trimLen);
                    return true;
                }
                return false;
            }
            public bool Equals(AposNameFix other)
            {
                return this.fullName.Equals(other.fullName);
            }
            public override string ToString()
            {
                return fullName;
            }
        }
        
        private int LoadXmlApostropheNameFix(XmlTextReader xReader)
        {
            int errorCount = 0;
            if (xReader.IsEmptyElement)
                return errorCount;
            while (xReader.Read())
            {
                if (xReader.NodeType == XmlNodeType.EndElement)
                    return errorCount;
                if (xReader.NodeType == XmlNodeType.Element)
                {
                    if (xReader.LocalName == "AposFix")
                    {
                        try
                        {
                            AposNameFix newItem = new AposNameFix(xReader.GetAttribute("FullName"), xReader.GetAttribute("Left"), xReader.GetAttribute("Right"));
                            newItem.Active = Boolean.Parse(xReader.GetAttribute("Active"));
                            AposAddNameFix(newItem);
                        }
                        catch (Exception ex)
                        {
                            errorCount++;
                            ActGlobals.oFormActMain.WriteExceptionLog(ex, "AposFix" + xReader.ReadOuterXml());
                        }
                    }
                    else
                        break;
                }
            }
            return errorCount;
        }
        
        private void SaveXmlApostropheNameFix(XmlTextWriter xWriter)
        {
            xWriter.WriteStartElement("ApostropheNameFix");
            foreach (KeyValuePair<string, AposNameFix> pair in aposNameList)
            {
                xWriter.WriteStartElement("AposFix");
                xWriter.WriteAttributeString("Active", pair.Value.Active.ToString());
                xWriter.WriteAttributeString("FullName", pair.Value.FullName);
                xWriter.WriteAttributeString("Left", pair.Value.Left);
                xWriter.WriteAttributeString("Right", pair.Value.Right);
                xWriter.WriteEndElement();
            }
            xWriter.WriteEndElement();
        }
        
        //void oFormActMain_UpdateCheckClicked()
        //{
        //    try
        //    {
        //        DateTime localDate = ActGlobals.oFormActMain.PluginGetSelfDateUtc(this);
        //        DateTime remoteDate = ActGlobals.oFormActMain.PluginGetRemoteDateUtc(pluginId);
        //        ActGlobals.oFormActMain.PluginGetGithubApi(pluginId);
        //        if (localDate.AddHours(2) < remoteDate)
        //        {
        //            DialogResult result = MessageBox.Show($"There is an updated version of the {PluginName} .  Update it now?\n\n(If there is an update to ACT, you should click No and update ACT first.)", "New Version", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        //            if (result == DialogResult.Yes)
        //            {
        //                FileInfo updatedFile = ActGlobals.oFormActMain.PluginDownload(pluginId);
        //                ActPluginData pluginData = ActGlobals.oFormActMain.PluginGetSelfData(this);
        //                pluginData.pluginFile.Delete();
        //                updatedFile.MoveTo(pluginData.pluginFile.FullName);
        //                ThreadInvokes.CheckboxSetChecked(ActGlobals.oFormActMain, pluginData.cbEnabled, false);
        //                Application.DoEvents();
        //                ThreadInvokes.CheckboxSetChecked(ActGlobals.oFormActMain, pluginData.cbEnabled, true);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ActGlobals.oFormActMain.WriteExceptionLog(ex, "Plugin Update Check");
        //    }
        //}
        
        private void cbRecalcWardedHits_MouseHover(object sender, EventArgs e)
        {
            ActGlobals.oFormActMain.SetOptionsHelpText("If enabled, no-damage hits or reduced damage hits immediately following a ward absorbtion will be increased by the absorption amount.  Stoneskin's no-damage hits cannot be recalculated.");
        }
        
        private void cbMultiDamageIsOne_MouseHover(object sender, EventArgs e)
        {
            ActGlobals.oFormActMain.SetOptionsHelpText("When enabled, an attack that has multiple damage types, such as \"300 crushing, 5 poison and 5 disease damage\" will show up as one total attack: 300/5/5 crushing/poison/disease, internally seen as 310.  If disabled, each damage type will show up as an individual swing, IE three attacks: 300 crushing; 5 poison; 5 disease.  Having a single attack show up as multiple will have consequences when calculating ToHit%.");
        }
        
        private void lblAncestralSentry_MouseHover(object sender, EventArgs e)
        {
            ActGlobals.oFormActMain.SetOptionsHelpText("The Mystic ability, Ancestral Sentry, will attempt to intercede players near it in a static manner.");
        }
        
        private void cbSParseConsider_MouseHover(object sender, EventArgs e)
        {
            ActGlobals.oFormActMain.SetOptionsHelpText("The /con command simply adds some text to the log about your target's con-level.  The /whogroup and /whoraid commands will list the members of your group/raid respectively.  Using this option will allow you to quickly add players to the Selective Parsing list.");
        }
        
        private void AposName_MouseHover(object sender, EventArgs e)
        {
            ActGlobals.oFormActMain.SetOptionsHelpText("Certain mob names with apostrophes in their name will cause the English parser to incorrectly split a combatant name from an ability name.  Use this section to add a full combatant name that should not be split apart.");
        }
        
        private void cbIncludeInterceptFocus_MouseHover(object sender, EventArgs e)
        {
            ActGlobals.oFormActMain.SetOptionsHelpText("When a channeler per intercepts damage, it receives an attack as focus damage.  Normally this focus damage is ignored and instead parsed as a heal for the channeler.  Enabling this option will also parse the damage done to the pet.");
        }
        
        private void clbAposName_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            string label = (string)clbAposName.Items[e.Index];
            AposNameFix selectedItem = aposNameList[label];
            selectedItem.Active = e.NewValue == CheckState.Checked;
        }
        
        private void clbAposName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (clbAposName.SelectedIndex != -1)
            {
                string label = (string)clbAposName.Items[clbAposName.SelectedIndex];
                AposNameFix selectedItem = aposNameList[label];
                tbAposName.Text = selectedItem.FullName;
                tbAposNameL.Text = selectedItem.Left;
                tbAposNameR.Text = selectedItem.Right;
            }
        }
        
        private void tbAposName_TextChanged(object sender, EventArgs e)
        {
            string[] engNameSkillSplit = tbAposName.Text.Split(new char[] { '\'' });
            string attacker = string.Empty;
            string skillType = string.Empty;

            SplitAttackerSkill(ref attacker, ref skillType, engNameSkillSplit);

            tbAposNameL.Text = attacker;
            tbAposNameR.Text = skillType;
        }
        
        private void btnAposNameAdd_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(tbAposNameL.Text) || String.IsNullOrEmpty(tbAposNameR.Text))
            {
                MessageBox.Show("This name does not appear to be split by the parsing engine.", "Invalid Name", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            AposNameFix newItem = new AposNameFix(tbAposName.Text, tbAposNameL.Text, tbAposNameR.Text);
            AposAddNameFix(newItem);
        }
        
        private void btnAposNameRemove_Click(object sender, EventArgs e)
        {
            if (aposNameList.ContainsKey(tbAposName.Text))
            {
                aposNameList.Remove(tbAposName.Text);
                clbAposName.Items.Remove(tbAposName.Text);
            }
        }
        
        private void AposAddNameFix(AposNameFix newItem)
        {
            if (!aposNameList.ContainsKey(newItem.FullName))
            {
                aposNameList.Add(newItem.FullName, newItem);
                clbAposName.Items.Add(newItem.FullName, newItem.Active);
            }
        }
        
        private string GetIntCommas()
        {
            return ActGlobals.mainTableShowCommas ? "#,0" : "0";
        }
        
        private string GetFloatCommas()
        {
            return ActGlobals.mainTableShowCommas ? "#,0.00" : "0.00";
        }
        
        private void SetupEverQuestEnvironment()
        {
            CultureInfo usCulture = new CultureInfo("en-US");   // This is for SQL syntax; do not change

            #region EncounterData Column Defs
            EncounterData.ColumnDefs.Clear();
            //Do not change the SqlDataName while doing localization
            EncounterData.ColumnDefs.Add("EncId", new EncounterData.ColumnDef("EncounterId", false, "INT", "EncounterId", (Data) => { return Data.EncId; }, (Data) => { return Data.EncId; }));
            EncounterData.ColumnDefs.Add("Title", new EncounterData.ColumnDef("Title", true, "VARCHAR(64)", "Title", (Data) => { return Data.Title; }, (Data) => { return Data.Title; }));
            EncounterData.ColumnDefs.Add("StartTime", new EncounterData.ColumnDef("StartTime", true, "TIMESTAMP", "StartTime", (Data) => { return Data.StartTime == DateTime.MaxValue ? "--:--:--" : String.Format("{0} {1}", Data.StartTime.ToShortDateString(), Data.StartTime.ToLongTimeString()); }, (Data) => { return Data.StartTime == DateTime.MaxValue ? "0000-00-00 00:00:00" : Data.StartTime.ToString("u").TrimEnd(new char[] { 'Z' }); }));
            EncounterData.ColumnDefs.Add("EndTime", new EncounterData.ColumnDef("EndTime", true, "TIMESTAMP", "EndTime", (Data) => { return Data.EndTime == DateTime.MinValue ? "--:--:--" : Data.EndTime.ToString("T"); }, (Data) => { return Data.EndTime == DateTime.MinValue ? "0000-00-00 00:00:00" : Data.EndTime.ToString("u").TrimEnd(new char[] { 'Z' }); }));
            EncounterData.ColumnDefs.Add("Duration", new EncounterData.ColumnDef("Duration", true, "INT", "Duration", (Data) => { return Data.DurationS; }, (Data) => { return Data.Duration.TotalSeconds.ToString("0"); }));
            EncounterData.ColumnDefs.Add("Damage", new EncounterData.ColumnDef("Damage", true, "BIGINT", "Damage", (Data) => { return Data.Damage.ToString(); }, (Data) => { return Data.Damage.ToString(); }));
            EncounterData.ColumnDefs.Add("EncDPS", new EncounterData.ColumnDef("EncDPS", true, "DOUBLE", "EncDPS", (Data) => { return Data.DPS.ToString(); }, (Data) => { return Data.DPS.ToString(usCulture); }));
            EncounterData.ColumnDefs.Add("Zone", new EncounterData.ColumnDef("Zone", false, "VARCHAR(64)", "Zone", (Data) => { return Data.ZoneName; }, (Data) => { return Data.ZoneName; }));
            EncounterData.ColumnDefs.Add("Kills", new EncounterData.ColumnDef("Kills", true, "INT", "Kills", (Data) => { return Data.AlliedKills.ToString(); }, (Data) => { return Data.AlliedKills.ToString(); }));
            EncounterData.ColumnDefs.Add("Deaths", new EncounterData.ColumnDef("Deaths", true, "INT", "Deaths", (Data) => { return Data.AlliedDeaths.ToString(); }, (Data) => { return Data.AlliedDeaths.ToString(); }));
            #endregion EncounterData

            #region EncounterData ExportVariables
            EncounterData.ExportVariables.Clear();
            EncounterData.ExportVariables.Add("n", new EncounterData.TextExportFormatter("n", "New Line", "Formatting after this element will appear on a new line.", (Data, SelectiveAllies, Extra) => { return "\n"; }));
            EncounterData.ExportVariables.Add("t", new EncounterData.TextExportFormatter("t", "Tab Character", "Formatting after this element will appear in a relative column arrangement.  (The formatting example cannot display this properly)", (Data, SelectiveAllies, Extra) => { return "\t"; }));
            EncounterData.ExportVariables.Add("title", new EncounterData.TextExportFormatter("title", "Encounter Title", "The title of the completed encounter.  This may only be used in Allies formatting.", (Data, SelectiveAllies, Extra) => { return EncounterFormatSwitch(Data, SelectiveAllies, "title", Extra); }));
            EncounterData.ExportVariables.Add("duration", new EncounterData.TextExportFormatter("duration", "Duration", "The duration of the combatant or the duration of the encounter, displayed as mm:ss", (Data, SelectiveAllies, Extra) => { return EncounterFormatSwitch(Data, SelectiveAllies, "duration", Extra); }));
            EncounterData.ExportVariables.Add("DURATION", new EncounterData.TextExportFormatter("DURATION", "Short Duration", "The duration of the combatant or encounter displayed in whole seconds.", (Data, SelectiveAllies, Extra) => { return EncounterFormatSwitch(Data, SelectiveAllies, "DURATION", Extra); }));
            EncounterData.ExportVariables.Add("damage", new EncounterData.TextExportFormatter("damage", "Damage", "The amount of damage from auto-attack, spells, CAs, etc done to other combatants.", (Data, SelectiveAllies, Extra) => { return EncounterFormatSwitch(Data, SelectiveAllies, "damage", Extra); }));
            EncounterData.ExportVariables.Add("damage-m", new EncounterData.TextExportFormatter("damage-m", "Damage M", "Damage divided by 1,000,000 (with two decimal places)", (Data, SelectiveAllies, Extra) => { return EncounterFormatSwitch(Data, SelectiveAllies, "damage-m", Extra); }));
            EncounterData.ExportVariables.Add("damage-*", new EncounterData.TextExportFormatter("damage-*", "Damage w/suffix", "Damage divided 1/K/M/B/T/Q (with two decimal places)", (Data, SelectiveAllies, Extra) => { return EncounterFormatSwitch(Data, SelectiveAllies, "damage-*", Extra); }));
            EncounterData.ExportVariables.Add("DAMAGE-k", new EncounterData.TextExportFormatter("DAMAGE-k", "Short Damage K", "Damage divided by 1,000 (with no decimal places)", (Data, SelectiveAllies, Extra) => { return EncounterFormatSwitch(Data, SelectiveAllies, "DAMAGE-k", Extra); }));
            EncounterData.ExportVariables.Add("DAMAGE-m", new EncounterData.TextExportFormatter("DAMAGE-m", "Short Damage M", "Damage divided by 1,000,000 (with no decimal places)", (Data, SelectiveAllies, Extra) => { return EncounterFormatSwitch(Data, SelectiveAllies, "DAMAGE-m", Extra); }));
            EncounterData.ExportVariables.Add("DAMAGE-b", new EncounterData.TextExportFormatter("DAMAGE-b", "Short Damage B", "Damage divided by 1,000,000,000 (with no decimal places)", (Data, SelectiveAllies, Extra) => { return EncounterFormatSwitch(Data, SelectiveAllies, "DAMAGE-b", Extra); }));
            EncounterData.ExportVariables.Add("DAMAGE-*", new EncounterData.TextExportFormatter("DAMAGE-*", "Short Damage w/suffix", "Damage divided by 1/K/M/B/T/Q (with no decimal places)", (Data, SelectiveAllies, Extra) => { return EncounterFormatSwitch(Data, SelectiveAllies, "DAMAGE-*", Extra); }));
            EncounterData.ExportVariables.Add("dps", new EncounterData.TextExportFormatter("dps", "DPS", "The damage total of the combatant divided by their personal duration, formatted as 12.34", (Data, SelectiveAllies, Extra) => { return EncounterFormatSwitch(Data, SelectiveAllies, "dps", Extra); }));
            EncounterData.ExportVariables.Add("dps-*", new EncounterData.TextExportFormatter("dps-*", "DPS w/suffix", "The damage total of the combatant divided by their personal duration, formatted as 12.34K", (Data, SelectiveAllies, Extra) => { return EncounterFormatSwitch(Data, SelectiveAllies, "dps-*", Extra); }));
            EncounterData.ExportVariables.Add("DPS", new EncounterData.TextExportFormatter("DPS", "Short DPS", "The damage total of the combatatant divided by their personal duration, formatted as 12", (Data, SelectiveAllies, Extra) => { return EncounterFormatSwitch(Data, SelectiveAllies, "DPS", Extra); }));
            EncounterData.ExportVariables.Add("DPS-k", new EncounterData.TextExportFormatter("DPS-k", "DPS K", "DPS divided by 1,000 (with no decimal places)", (Data, SelectiveAllies, Extra) => { return EncounterFormatSwitch(Data, SelectiveAllies, "DPS-k", Extra); }));
            EncounterData.ExportVariables.Add("DPS-m", new EncounterData.TextExportFormatter("DPS-m", "DPS M", "DPS divided by 1,000,000 (with no decimal places)", (Data, SelectiveAllies, Extra) => { return EncounterFormatSwitch(Data, SelectiveAllies, "DPS-m", Extra); }));
            EncounterData.ExportVariables.Add("DPS-*", new EncounterData.TextExportFormatter("DPS-*", "DPS w/suffix", "DPS divided by 1/K/M/B/T/Q (with no decimal places)", (Data, SelectiveAllies, Extra) => { return EncounterFormatSwitch(Data, SelectiveAllies, "DPS-*", Extra); }));
            EncounterData.ExportVariables.Add("encdps", new EncounterData.TextExportFormatter("encdps", "Encounter DPS", "The damage total of the combatant divided by the duration of the encounter, formatted as 12.34 -- This is more commonly used than DPS", (Data, SelectiveAllies, Extra) => { return EncounterFormatSwitch(Data, SelectiveAllies, "encdps", Extra); }));
            EncounterData.ExportVariables.Add("encdps-*", new EncounterData.TextExportFormatter("encdps-*", "Encounter DPS w/suffix", "The damage total of the combatant divided by the duration of the encounter, formatted as 12.34 -- This is more commonly used than DPS", (Data, SelectiveAllies, Extra) => { return EncounterFormatSwitch(Data, SelectiveAllies, "encdps-*", Extra); }));
            EncounterData.ExportVariables.Add("ENCDPS", new EncounterData.TextExportFormatter("ENCDPS", "Short Encounter DPS", "The damage total of the combatant divided by the duration of the encounter, formatted as 12 -- This is more commonly used than DPS", (Data, SelectiveAllies, Extra) => { return EncounterFormatSwitch(Data, SelectiveAllies, "ENCDPS", Extra); }));
            EncounterData.ExportVariables.Add("ENCDPS-k", new EncounterData.TextExportFormatter("ENCDPS-k", "Short Encounter DPS K", "ENCDPS divided by 1,000 (with no decimal places)", (Data, SelectiveAllies, Extra) => { return EncounterFormatSwitch(Data, SelectiveAllies, "ENCDPS-k", Extra); }));
            EncounterData.ExportVariables.Add("ENCDPS-m", new EncounterData.TextExportFormatter("ENCDPS-m", "Short Encounter DPS M", "ENCDPS divided by 1,000,000 (with no decimal places)", (Data, SelectiveAllies, Extra) => { return EncounterFormatSwitch(Data, SelectiveAllies, "ENCDPS-m", Extra); }));
            EncounterData.ExportVariables.Add("ENCDPS-*", new EncounterData.TextExportFormatter("ENCDPS-*", "Short Encounter DPS w/suffix", "ENCDPS divided by 1/K/M/B/T/Q (with no decimal places)", (Data, SelectiveAllies, Extra) => { return EncounterFormatSwitch(Data, SelectiveAllies, "ENCDPS-*", Extra); }));
            EncounterData.ExportVariables.Add("hits", new EncounterData.TextExportFormatter("hits", "Hits", "The number of attack attempts that produced damage.  IE a spell successfully doing damage.", (Data, SelectiveAllies, Extra) => { return EncounterFormatSwitch(Data, SelectiveAllies, "hits", Extra); }));
            EncounterData.ExportVariables.Add("crithits", new EncounterData.TextExportFormatter("crithits", "Critical Hit Count", "The number of damaging attacks that were critical.", (Data, SelectiveAllies, Extra) => { return EncounterFormatSwitch(Data, SelectiveAllies, "crithits", Extra); }));
            EncounterData.ExportVariables.Add("crithit%", new EncounterData.TextExportFormatter("crithit%", "Critical Hit Percentage", "The percentage of damaging attacks that were critical.", (Data, SelectiveAllies, Extra) => { return EncounterFormatSwitch(Data, SelectiveAllies, "crithit%", Extra); }));
            EncounterData.ExportVariables.Add("misses", new EncounterData.TextExportFormatter("misses", "Misses", "The number of auto-attacks or CAs that produced a miss message.", (Data, SelectiveAllies, Extra) => { return EncounterFormatSwitch(Data, SelectiveAllies, "misses", Extra); }));
            EncounterData.ExportVariables.Add("hitfailed", new EncounterData.TextExportFormatter("hitfailed", "Other Avoid", "Any type of failed attack that was not a miss.  This includes resists, reflects, blocks, dodging, etc.", (Data, SelectiveAllies, Extra) => { return EncounterFormatSwitch(Data, SelectiveAllies, "hitfailed", Extra); }));
            EncounterData.ExportVariables.Add("swings", new EncounterData.TextExportFormatter("swings", "Swings (Attacks)", "The number of attack attempts.  This includes any auto-attacks or abilities, also including resisted abilities that do no damage.", (Data, SelectiveAllies, Extra) => { return EncounterFormatSwitch(Data, SelectiveAllies, "swings", Extra); }));
            EncounterData.ExportVariables.Add("tohit", new EncounterData.TextExportFormatter("tohit", "To Hit %", "The percentage of hits to swings as 12.34", (Data, SelectiveAllies, Extra) => { return EncounterFormatSwitch(Data, SelectiveAllies, "tohit", Extra); }));
            EncounterData.ExportVariables.Add("TOHIT", new EncounterData.TextExportFormatter("TOHIT", "Short To Hit %", "The percentage of hits to swings as 12", (Data, SelectiveAllies, Extra) => { return EncounterFormatSwitch(Data, SelectiveAllies, "TOHIT", Extra); }));
            EncounterData.ExportVariables.Add("maxhit", new EncounterData.TextExportFormatter("maxhit", "Highest Hit", "The highest single damaging hit formatted as [Combatant-]SkillName-Damage#", (Data, SelectiveAllies, Extra) => { return EncounterFormatSwitch(Data, SelectiveAllies, "maxhit", Extra); }));
            EncounterData.ExportVariables.Add("MAXHIT", new EncounterData.TextExportFormatter("MAXHIT", "Short Highest Hit", "The highest single damaging hit formatted as [Combatant-]Damage#", (Data, SelectiveAllies, Extra) => { return EncounterFormatSwitch(Data, SelectiveAllies, "MAXHIT", Extra); }));
            EncounterData.ExportVariables.Add("maxhit-*", new EncounterData.TextExportFormatter("maxhit-*", "Highest Hit w/ suffix", "MaxHit divided by 1/K/M/B/T/Q", (Data, SelectiveAllies, Extra) => { return EncounterFormatSwitch(Data, SelectiveAllies, "maxhit-*", Extra); }));
            EncounterData.ExportVariables.Add("MAXHIT-*", new EncounterData.TextExportFormatter("MAXHIT-*", "Short Highest Hit w/ suffix", "Short MaxHit divided by 1/K/M/B/T/Q", (Data, SelectiveAllies, Extra) => { return EncounterFormatSwitch(Data, SelectiveAllies, "MAXHIT-*", Extra); }));
            EncounterData.ExportVariables.Add("healed", new EncounterData.TextExportFormatter("healed", "Healed", "The numerical total of all heals, wards or similar sourced from this combatant.", (Data, SelectiveAllies, Extra) => { return EncounterFormatSwitch(Data, SelectiveAllies, "healed", Extra); }));
            EncounterData.ExportVariables.Add("enchps", new EncounterData.TextExportFormatter("enchps", "Encounter HPS", "The healing total of the combatant divided by the duration of the encounter, formatted as 12.34", (Data, SelectiveAllies, Extra) => { return EncounterFormatSwitch(Data, SelectiveAllies, "enchps", Extra); }));
            EncounterData.ExportVariables.Add("enchps-*", new EncounterData.TextExportFormatter("enchps-*", "Encounter HPS w/suffix", "Encounter HPS divided by 1/K/M/B/T/Q", (Data, SelectiveAllies, Extra) => { return EncounterFormatSwitch(Data, SelectiveAllies, "enchps-*", Extra); }));
            EncounterData.ExportVariables.Add("ENCHPS", new EncounterData.TextExportFormatter("ENCHPS", "Short Encounter HPS", "The healing total of the combatant divided by the duration of the encounter, formatted as 12", (Data, SelectiveAllies, Extra) => { return EncounterFormatSwitch(Data, SelectiveAllies, "ENCHPS", Extra); }));
            EncounterData.ExportVariables.Add("ENCHPS-k", new EncounterData.TextExportFormatter("ENCHPS-k", "Short ENCHPS K", "ENCHPS divided by 1,000 (with no decimal places)", (Data, SelectiveAllies, Extra) => { return EncounterFormatSwitch(Data, SelectiveAllies, "ENCHPS-k", Extra); }));
            EncounterData.ExportVariables.Add("ENCHPS-m", new EncounterData.TextExportFormatter("ENCHPS-m", "Short ENCHPS M", "ENCHPS divided by 1,000,000 (with no decimal places)", (Data, SelectiveAllies, Extra) => { return EncounterFormatSwitch(Data, SelectiveAllies, "ENCHPS-m", Extra); }));
            EncounterData.ExportVariables.Add("ENCHPS-*", new EncounterData.TextExportFormatter("ENCHPS-*", "Short ENCHPS w/suffix", "ENCHPS divided by 1/K/M/B/T/Q (with no decimal places)", (Data, SelectiveAllies, Extra) => { return EncounterFormatSwitch(Data, SelectiveAllies, "ENCHPS-*", Extra); }));
            EncounterData.ExportVariables.Add("heals", new EncounterData.TextExportFormatter("heals", "Heal Count", "The total number of heals from this combatant.", (Data, SelectiveAllies, Extra) => { return EncounterFormatSwitch(Data, SelectiveAllies, "heals", Extra); }));
            EncounterData.ExportVariables.Add("critheals", new EncounterData.TextExportFormatter("critheals", "Critical Heal Count", "The number of heals that were critical.", (Data, SelectiveAllies, Extra) => { return EncounterFormatSwitch(Data, SelectiveAllies, "critheals", Extra); }));
            EncounterData.ExportVariables.Add("critheal%", new EncounterData.TextExportFormatter("critheal%", "Critical Heal Percentage", "The percentage of heals that were critical.", (Data, SelectiveAllies, Extra) => { return EncounterFormatSwitch(Data, SelectiveAllies, "critheal%", Extra); }));
            EncounterData.ExportVariables.Add("cures", new EncounterData.TextExportFormatter("cures", "Cure or Dispel Count", "The total number of times the combatant cured or dispelled", (Data, SelectiveAllies, Extra) => { return EncounterFormatSwitch(Data, SelectiveAllies, "cures", Extra); }));
            EncounterData.ExportVariables.Add("maxheal", new EncounterData.TextExportFormatter("maxheal", "Highest Heal", "The highest single healing amount formatted as [Combatant-]SkillName-Healing#", (Data, SelectiveAllies, Extra) => { return EncounterFormatSwitch(Data, SelectiveAllies, "maxheal", Extra); }));
            EncounterData.ExportVariables.Add("MAXHEAL", new EncounterData.TextExportFormatter("MAXHEAL", "Short Highest Heal", "The highest single healing amount formatted as [Combatant-]Healing#", (Data, SelectiveAllies, Extra) => { return EncounterFormatSwitch(Data, SelectiveAllies, "MAXHEAL", Extra); }));
            EncounterData.ExportVariables.Add("maxhealward", new EncounterData.TextExportFormatter("maxhealward", "Highest Heal/Ward", "The highest single healing/warding amount formatted as [Combatant-]SkillName-Healing#", (Data, SelectiveAllies, Extra) => { return EncounterFormatSwitch(Data, SelectiveAllies, "maxhealward", Extra); }));
            EncounterData.ExportVariables.Add("MAXHEALWARD", new EncounterData.TextExportFormatter("MAXHEALWARD", "Short Highest Heal/Ward", "The highest single healing/warding amount formatted as [Combatant-]Healing#", (Data, SelectiveAllies, Extra) => { return EncounterFormatSwitch(Data, SelectiveAllies, "MAXHEALWARD", Extra); }));
            EncounterData.ExportVariables.Add("maxheal-*", new EncounterData.TextExportFormatter("maxheal-*", "Highest Heal w/ suffix", "Highest Heal divided by 1/K/M/B/T/Q", (Data, SelectiveAllies, Extra) => { return EncounterFormatSwitch(Data, SelectiveAllies, "maxheal-*", Extra); }));
            EncounterData.ExportVariables.Add("MAXHEAL-*", new EncounterData.TextExportFormatter("MAXHEAL-*", "Short Highest Heal w/ suffix", "Short Highest Heal divided by 1/K/M/B/T/Q", (Data, SelectiveAllies, Extra) => { return EncounterFormatSwitch(Data, SelectiveAllies, "MAXHEAL-*", Extra); }));
            EncounterData.ExportVariables.Add("maxhealward-*", new EncounterData.TextExportFormatter("maxhealward-*", "Highest Heal/Ward w/ suffix", "Highest Heal/Ward divided by 1/K/M/B/T/Q", (Data, SelectiveAllies, Extra) => { return EncounterFormatSwitch(Data, SelectiveAllies, "maxhealward-*", Extra); }));
            EncounterData.ExportVariables.Add("MAXHEALWARD-*", new EncounterData.TextExportFormatter("MAXHEALWARD-*", "Short Highest Heal/Ward w/ suffix", "Short Highest Heal/Ward divided by 1/K/M/B/T/Q", (Data, SelectiveAllies, Extra) => { return EncounterFormatSwitch(Data, SelectiveAllies, "MAXHEALWARD-*", Extra); }));
            EncounterData.ExportVariables.Add("damagetaken", new EncounterData.TextExportFormatter("damagetaken", "Damage Received", "The total amount of damage this combatant received.", (Data, SelectiveAllies, Extra) => { return EncounterFormatSwitch(Data, SelectiveAllies, "damagetaken", Extra); }));
            EncounterData.ExportVariables.Add("damagetaken-*", new EncounterData.TextExportFormatter("damagetaken-*", "Damage Received w/suffix", "Damage Received divided by 1/K/M/B/T/Q", (Data, SelectiveAllies, Extra) => { return EncounterFormatSwitch(Data, SelectiveAllies, "damagetaken-*", Extra); }));
            EncounterData.ExportVariables.Add("healstaken", new EncounterData.TextExportFormatter("healstaken", "Healing Received", "The total amount of healing this combatant received.", (Data, SelectiveAllies, Extra) => { return EncounterFormatSwitch(Data, SelectiveAllies, "healstaken", Extra); }));
            EncounterData.ExportVariables.Add("healstaken-*", new EncounterData.TextExportFormatter("healstaken-*", "Healing Received w/suffix", "Healing Received divided by 1/K/M/B/T/Q", (Data, SelectiveAllies, Extra) => { return EncounterFormatSwitch(Data, SelectiveAllies, "healstaken-*", Extra); }));
            EncounterData.ExportVariables.Add("kills", new EncounterData.TextExportFormatter("kills", "Killing Blows", "The total number of times this character landed a killing blow.", (Data, SelectiveAllies, Extra) => { return EncounterFormatSwitch(Data, SelectiveAllies, "kills", Extra); }));
            EncounterData.ExportVariables.Add("deaths", new EncounterData.TextExportFormatter("deaths", "Deaths", "The total number of times this character was killed by another.", (Data, SelectiveAllies, Extra) => { return EncounterFormatSwitch(Data, SelectiveAllies, "deaths", Extra); }));
            #endregion

            #region CombatantData ColumnDefs
            CombatantData.ColumnDefs.Clear();
            CombatantData.ColumnDefs.Add("EncounterId", new CombatantData.ColumnDef("EncounterId", false, "INT", "EncounterId", (Data) => { return Data.Parent.EncId; }, (Data) => { return Data.Parent.EncId; }, (Left, Right) => { return Left.Parent.EncId.CompareTo(Right.Parent.EncId); }));
            CombatantData.ColumnDefs.Add("Ally", new CombatantData.ColumnDef("Ally", false, "CHAR(1)", "Ally", (Data) => { return Data.Parent.GetAllies().Contains(Data).ToString(); }, (Data) => { return Data.Parent.GetAllies().Contains(Data) ? "T" : "F"; }, (Left, Right) => { return Left.Parent.GetAllies().Contains(Left).CompareTo(Right.Parent.GetAllies().Contains(Right)); }));
            CombatantData.ColumnDefs.Add("Name", new CombatantData.ColumnDef("Name", true, "VARCHAR(64)", "Name", (Data) => { return Data.Name; }, (Data) => { return Data.Name; }, (Left, Right) => { return Left.Name.CompareTo(Right.Name); }));
            CombatantData.ColumnDefs.Add("StartTime", new CombatantData.ColumnDef("StartTime", true, "TIMESTAMP", "StartTime", (Data) => { return Data.StartTime == DateTime.MaxValue ? "--:--:--" : Data.StartTime.ToString("T"); }, (Data) => { return Data.StartTime == DateTime.MaxValue ? "0000-00-00 00:00:00" : Data.StartTime.ToString("u").TrimEnd(new char[] { 'Z' }); }, (Left, Right) => { return Left.StartTime.CompareTo(Right.StartTime); }));
            CombatantData.ColumnDefs.Add("EndTime", new CombatantData.ColumnDef("EndTime", false, "TIMESTAMP", "EndTime", (Data) => { return Data.EndTime == DateTime.MinValue ? "--:--:--" : Data.EndTime.ToString("T"); }, (Data) => { return Data.EndTime == DateTime.MinValue ? "0000-00-00 00:00:00" : Data.EndTime.ToString("u").TrimEnd(new char[] { 'Z' }); }, (Left, Right) => { return Left.EndTime.CompareTo(Right.EndTime); }));
            CombatantData.ColumnDefs.Add("Duration", new CombatantData.ColumnDef("Duration", true, "INT", "Duration", (Data) => { return Data.DurationS; }, (Data) => { return Data.Duration.TotalSeconds.ToString("0"); }, (Left, Right) => { return Left.Duration.CompareTo(Right.Duration); }));
            CombatantData.ColumnDefs.Add("Damage", new CombatantData.ColumnDef("Damage", true, "BIGINT", "Damage", (Data) => { return Data.Damage.ToString(); }, (Data) => { return Data.Damage.ToString(); }, (Left, Right) => { return Left.Damage.CompareTo(Right.Damage); }));
            CombatantData.ColumnDefs.Add("Damage%", new CombatantData.ColumnDef("Damage%", true, "VARCHAR(4)", "DamagePerc", (Data) => { return Data.DamagePercent; }, (Data) => { return Data.DamagePercent; }, (Left, Right) => { return Left.Damage.CompareTo(Right.Damage); }));
            CombatantData.ColumnDefs.Add("Kills", new CombatantData.ColumnDef("Kills", false, "INT", "Kills", (Data) => { return Data.Kills.ToString(); }, (Data) => { return Data.Kills.ToString(); }, (Left, Right) => { return Left.Kills.CompareTo(Right.Kills); }));
            CombatantData.ColumnDefs.Add("Healed", new CombatantData.ColumnDef("Healed", false, "BIGINT", "Healed", (Data) => { return Data.Healed.ToString(); }, (Data) => { return Data.Healed.ToString(); }, (Left, Right) => { return Left.Healed.CompareTo(Right.Healed); }));
            CombatantData.ColumnDefs.Add("Healed%", new CombatantData.ColumnDef("Healed%", false, "VARCHAR(4)", "HealedPerc", (Data) => { return Data.HealedPercent; }, (Data) => { return Data.HealedPercent; }, (Left, Right) => { return Left.Healed.CompareTo(Right.Healed); }));
            CombatantData.ColumnDefs.Add("CritHeals", new CombatantData.ColumnDef("CritHeals", false, "INT", "CritHeals", (Data) => { return Data.CritHeals.ToString(); }, (Data) => { return Data.CritHeals.ToString(); }, (Left, Right) => { return Left.CritHeals.CompareTo(Right.CritHeals); }));
            CombatantData.ColumnDefs.Add("Heals", new CombatantData.ColumnDef("Heals", false, "INT", "Heals", (Data) => { return Data.Heals.ToString(); }, (Data) => { return Data.Heals.ToString(); }, (Left, Right) => { return Left.Heals.CompareTo(Right.Heals); }));
            CombatantData.ColumnDefs.Add("OverHeal", new CombatantData.ColumnDef("OverHeal", true, "INT", "OverHeal", (Data) => { return Data.Tags.ContainsKey("overheal") ? ((int)Data.Tags["overheal"]).ToString() : string.Empty; }, (Data) => { return Data.Tags.ContainsKey("overheal") ? ((int)Data.Tags["overheal"]).ToString() : string.Empty; }, (Left, Right) => { return (Left.Tags.ContainsKey("overheal") && Right.Tags.ContainsKey("overheal")) ? ((int)Left.Tags["overheal"]).CompareTo(((int)Right.Tags["overheal"])) : 0; }));
            CombatantData.ColumnDefs.Add("DPS", new CombatantData.ColumnDef("DPS", false, "DOUBLE", "DPS", (Data) => { return Data.DPS.ToString(); }, (Data) => { return Data.DPS.ToString(usCulture); }, (Left, Right) => { return Left.DPS.CompareTo(Right.DPS); }));
            CombatantData.ColumnDefs.Add("EncDPS", new CombatantData.ColumnDef("EncDPS", true, "DOUBLE", "EncDPS", (Data) => { return Data.EncDPS.ToString(); }, (Data) => { return Data.EncDPS.ToString(usCulture); }, (Left, Right) => { return Left.Damage.CompareTo(Right.Damage); }));
            CombatantData.ColumnDefs.Add("EncHPS", new CombatantData.ColumnDef("EncHPS", true, "DOUBLE", "EncHPS", (Data) => { return Data.EncHPS.ToString(); }, (Data) => { return Data.EncHPS.ToString(usCulture); }, (Left, Right) => { return Left.Healed.CompareTo(Right.Healed); }));
            CombatantData.ColumnDefs.Add("Hits", new CombatantData.ColumnDef("Hits", false, "INT", "Hits", (Data) => { return Data.Hits.ToString(); }, (Data) => { return Data.Hits.ToString(); }, (Left, Right) => { return Left.Hits.CompareTo(Right.Hits); }));
            CombatantData.ColumnDefs.Add("CritHits", new CombatantData.ColumnDef("CritHits", false, "INT", "CritHits", (Data) => { return Data.CritHits.ToString(); }, (Data) => { return Data.CritHits.ToString(); }, (Left, Right) => { return Left.CritHits.CompareTo(Right.CritHits); }));
            CombatantData.ColumnDefs.Add("Avoids", new CombatantData.ColumnDef("Avoids", false, "INT", "Blocked", (Data) => { return Data.Blocked.ToString(); }, (Data) => { return Data.Blocked.ToString(); }, (Left, Right) => { return Left.Blocked.CompareTo(Right.Blocked); }));
            CombatantData.ColumnDefs.Add("Misses", new CombatantData.ColumnDef("Misses", false, "INT", "Misses", (Data) => { return Data.Misses.ToString(); }, (Data) => { return Data.Misses.ToString(); }, (Left, Right) => { return Left.Misses.CompareTo(Right.Misses); }));
            CombatantData.ColumnDefs.Add("Swings", new CombatantData.ColumnDef("Swings", false, "INT", "Swings", (Data) => { return Data.Swings.ToString(); }, (Data) => { return Data.Swings.ToString(); }, (Left, Right) => { return Left.Swings.CompareTo(Right.Swings); }));
            CombatantData.ColumnDefs.Add("HealingTaken", new CombatantData.ColumnDef("HealingTaken", false, "BIGINT", "HealsTaken", (Data) => { return Data.HealsTaken.ToString(); }, (Data) => { return Data.HealsTaken.ToString(); }, (Left, Right) => { return Left.HealsTaken.CompareTo(Right.HealsTaken); }));
            CombatantData.ColumnDefs.Add("DamageTaken", new CombatantData.ColumnDef("DamageTaken", true, "BIGINT", "DamageTaken", (Data) => { return Data.DamageTaken.ToString(); }, (Data) => { return Data.DamageTaken.ToString(); }, (Left, Right) => { return Left.DamageTaken.CompareTo(Right.DamageTaken); }));
            CombatantData.ColumnDefs.Add("Deaths", new CombatantData.ColumnDef("Deaths", true, "INT", "Deaths", (Data) => { return Data.Deaths.ToString(); }, (Data) => { return Data.Deaths.ToString(); }, (Left, Right) => { return Left.Deaths.CompareTo(Right.Deaths); }));
            CombatantData.ColumnDefs.Add("ToHit%", new CombatantData.ColumnDef("ToHit%", false, "FLOAT", "ToHit", (Data) => { return Data.ToHit.ToString(); }, (Data) => { return Data.ToHit.ToString(usCulture); }, (Left, Right) => { return Left.ToHit.CompareTo(Right.ToHit); }));
            CombatantData.ColumnDefs.Add("CritDam%", new CombatantData.ColumnDef("CritDam%", false, "VARCHAR(8)", "CritDamPerc", (Data) => { return Data.CritDamPerc.ToString("0'%"); }, (Data) => { return Data.CritDamPerc.ToString("0'%"); }, (Left, Right) => { return Left.CritDamPerc.CompareTo(Right.CritDamPerc); }));
            CombatantData.ColumnDefs.Add("CritHeal%", new CombatantData.ColumnDef("CritHeal%", false, "VARCHAR(8)", "CritHealPerc", (Data) => { return Data.CritHealPerc.ToString("0'%"); }, (Data) => { return Data.CritHealPerc.ToString("0'%"); }, (Left, Right) => { return Left.CritHealPerc.CompareTo(Right.CritHealPerc); }));
            CombatantData.ColumnDefs.Add("Specials", new CombatantData.ColumnDef("Specials", true, "VARCHAR(32)", "Specials", CombatantDataGetCritTypes, CombatantDataGetCritTypes, (Left, Right) => { return CombatantDataGetCritTypes(Left).CompareTo(CombatantDataGetCritTypes(Right)); }));

            CombatantData.ColumnDefs["Damage"].GetCellForeColor = (Data) => { return Color.DarkRed; };
            CombatantData.ColumnDefs["Damage%"].GetCellForeColor = (Data) => { return Color.DarkRed; };
            CombatantData.ColumnDefs["Healed"].GetCellForeColor = (Data) => { return Color.DarkBlue; };
            CombatantData.ColumnDefs["Healed%"].GetCellForeColor = (Data) => { return Color.DarkBlue; };
            CombatantData.ColumnDefs["DPS"].GetCellForeColor = (Data) => { return Color.DarkRed; };
            CombatantData.ColumnDefs["EncDPS"].GetCellForeColor = (Data) => { return Color.DarkRed; };
            CombatantData.ColumnDefs["EncHPS"].GetCellForeColor = (Data) => { return Color.DarkBlue; };
            CombatantData.ColumnDefs["DamageTaken"].GetCellForeColor = (Data) => { return Color.DarkOrange; };

            CombatantData.OutgoingDamageTypeDataObjects = new Dictionary<string, CombatantData.DamageTypeDef>
        {
            {"Auto-Attack (Out)", new CombatantData.DamageTypeDef("Auto-Attack (Out)", -1, Color.DarkGoldenrod)},
            {"Skill/Ability (Out)", new CombatantData.DamageTypeDef("Skill/Ability (Out)", -1, Color.DarkOrange)},
            {"Outgoing Damage", new CombatantData.DamageTypeDef("Outgoing Damage", 0, Color.Orange)},
            {"Healed (Out)", new CombatantData.DamageTypeDef("Healed (Out)", 1, Color.Blue)},
            {"All Outgoing (Ref)", new CombatantData.DamageTypeDef("All Outgoing (Ref)", 0, Color.Black)}


        };
            CombatantData.IncomingDamageTypeDataObjects = new Dictionary<string, CombatantData.DamageTypeDef>
        {
            {"Incoming Damage", new CombatantData.DamageTypeDef("Incoming Damage", -1, Color.Red)},
            {"Healed (Inc)",new CombatantData.DamageTypeDef("Healed (Inc)", 1, Color.LimeGreen)},
            {"All Incoming (Ref)",new CombatantData.DamageTypeDef("All Incoming (Ref)", 0, Color.Black)}
        };
            CombatantData.SwingTypeToDamageTypeDataLinksOutgoing = new SortedDictionary<int, List<string>>
        {
            {(int)EverQuestSwingType.Melee, new List<string> { "Auto-Attack (Out)", "Outgoing Damage" } },
            {(int)EverQuestSwingType.NonMelee, new List<string> { "Skill/Ability (Out)", "Outgoing Damage" } },
            {(int)EverQuestSwingType.InstantHealing, new List<string> { "Instant Healed (Out)", "Outgoing Instant Healing" } },
                {(int)EverQuestSwingType.HealOverTime, new List<string> { "Heal Over Time (Out)", "Outgoing Heal Over Time" } },
                {(int)EverQuestSwingType.Bane, new List<string> { "Bane Damage (Out)", "Outgoing Bane Damage"} },
                {(int)EverQuestSwingType.PetMelee, new List<string> { "Pet Melee (Out)", "Outgoing Pet Melee Damage" } },
                {(int)EverQuestSwingType.PetNonMelee, new List<string> { "Pet Non Melee (Out)", "Outgoing Pet Non Melee"} }
        };
            CombatantData.SwingTypeToDamageTypeDataLinksIncoming = new SortedDictionary<int, List<string>>
        {
            {(int)EverQuestSwingType.Melee, new List<string> { "Melee Damage (Inc)" } },
            {(int)EverQuestSwingType.NonMelee, new List<string> { "Non Melee Damage (Inc)" } },
            {(int)EverQuestSwingType.InstantHealing, new List<string> { "Instant Healed (Inc)" } },
                {(int)EverQuestSwingType.HealOverTime, new List<string> {"Heal Over Time (Inc)"} },
                {(int)EverQuestSwingType.Bane, new List<string> {"Bane Damage (Inc)"} },
                {(int)EverQuestSwingType.PetMelee, new List<string> {"Pet Melee (Inc)"} },
                {(int)EverQuestSwingType.PetNonMelee, new List<string> {"Pet Non Melee (Inc)"} }
        };

            CombatantData.DamageSwingTypes = new List<int> { (int)EverQuestSwingType.Melee, (int)EverQuestSwingType.NonMelee, (int)EverQuestSwingType.Bane, (int)EverQuestSwingType.PetMelee, (int)EverQuestSwingType.PetNonMelee };
            CombatantData.HealingSwingTypes = new List<int> { (int)EverQuestSwingType.InstantHealing, (int)EverQuestSwingType.HealOverTime };

            CombatantData.DamageTypeDataNonSkillDamage = "Auto-Attack (Out)";
            CombatantData.DamageTypeDataOutgoingDamage = "Outgoing Damage";
            CombatantData.DamageTypeDataOutgoingHealing = "Healed (Out)";
            CombatantData.DamageTypeDataIncomingDamage = "Incoming Damage";
            CombatantData.DamageTypeDataIncomingHealing = "Healed (Inc)";
            #endregion

            #region CombatantData ExportVariables
            CombatantData.ExportVariables.Clear();
            CombatantData.ExportVariables.Add("n", new CombatantData.TextExportFormatter("n", "New Line", "Formatting after this element will appear on a new line.", (Data, Extra) => { return "\n"; }));
            CombatantData.ExportVariables.Add("t", new CombatantData.TextExportFormatter("t", "Tab Character", "Formatting after this element will appear in a relative column arrangement.  (The formatting example cannot display this properly)", (Data, Extra) => { return "\t"; }));
            CombatantData.ExportVariables.Add("name", new CombatantData.TextExportFormatter("name", "Name", "The combatant's name.", (Data, Extra) => { return CombatantFormatSwitch(Data, "name", Extra); }));
            CombatantData.ExportVariables.Add("NAME", new CombatantData.TextExportFormatter("NAME", "Short Name", "The combatant's name shortened to a number of characters after a colon, like: \"NAME:5\"", (Data, Extra) => { return CombatantFormatSwitch(Data, "NAME", Extra); }));
            CombatantData.ExportVariables.Add("duration", new CombatantData.TextExportFormatter("duration", "Duration", "The duration of the combatant or the duration of the encounter, displayed as mm:ss", (Data, Extra) => { return CombatantFormatSwitch(Data, "duration", Extra); }));
            CombatantData.ExportVariables.Add("DURATION", new CombatantData.TextExportFormatter("DURATION", "Short Duration", "The duration of the combatant or encounter displayed in whole seconds.", (Data, Extra) => { return CombatantFormatSwitch(Data, "DURATION", Extra); }));
            CombatantData.ExportVariables.Add("damage", new CombatantData.TextExportFormatter("damage", "Damage", "The amount of damage from auto-attack, spells, CAs, etc done to other combatants.", (Data, Extra) => { return CombatantFormatSwitch(Data, "damage", Extra); }));
            CombatantData.ExportVariables.Add("damage-m", new CombatantData.TextExportFormatter("damage-m", "Damage M", "Damage divided by 1,000,000 (with two decimal places)", (Data, Extra) => { return CombatantFormatSwitch(Data, "damage-m", Extra); }));
            CombatantData.ExportVariables.Add("damage-b", new CombatantData.TextExportFormatter("damage-b", "Damage B", "Damage divided by 1,000,000,000 (with two decimal places)", (Data, Extra) => { return CombatantFormatSwitch(Data, "damage-b", Extra); }));
            CombatantData.ExportVariables.Add("damage-*", new CombatantData.TextExportFormatter("damage-*", "Damage w/suffix", "Damage divided by 1/K/M/B/T/Q (with one decimal places)", (Data, Extra) => { return CombatantFormatSwitch(Data, "damage-*", Extra); }));
            CombatantData.ExportVariables.Add("DAMAGE-k", new CombatantData.TextExportFormatter("DAMAGE-k", "Short Damage K", "Damage divided by 1,000 (with no decimal places)", (Data, Extra) => { return CombatantFormatSwitch(Data, "DAMAGE-k", Extra); }));
            CombatantData.ExportVariables.Add("DAMAGE-m", new CombatantData.TextExportFormatter("DAMAGE-m", "Short Damage M", "Damage divided by 1,000,000 (with no decimal places)", (Data, Extra) => { return CombatantFormatSwitch(Data, "DAMAGE-m", Extra); }));
            CombatantData.ExportVariables.Add("DAMAGE-b", new CombatantData.TextExportFormatter("DAMAGE-b", "Short Damage B", "Damage divided by 1,000,000,000 (with no decimal places)", (Data, Extra) => { return CombatantFormatSwitch(Data, "DAMAGE-b", Extra); }));
            CombatantData.ExportVariables.Add("DAMAGE-*", new CombatantData.TextExportFormatter("DAMAGE-*", "Short Damage w/suffix", "Damage divided by 1/K/M/B/T/Q (with no decimal places)", (Data, Extra) => { return CombatantFormatSwitch(Data, "DAMAGE-*", Extra); }));
            CombatantData.ExportVariables.Add("damage%", new CombatantData.TextExportFormatter("damage%", "Damage %", "This value represents the percent share of all damage done by allies in this encounter.", (Data, Extra) => { return CombatantFormatSwitch(Data, "damage%", Extra); }));
            CombatantData.ExportVariables.Add("dps", new CombatantData.TextExportFormatter("dps", "DPS", "The damage total of the combatant divided by their personal duration, formatted as 12.34", (Data, Extra) => { return CombatantFormatSwitch(Data, "dps", Extra); }));
            CombatantData.ExportVariables.Add("dps-*", new CombatantData.TextExportFormatter("dps-*", "DPS w/suffix", "The damage total of the combatant divided by their personal duration, formatted as 12.34K", (Data, Extra) => { return CombatantFormatSwitch(Data, "dps-*", Extra); }));
            CombatantData.ExportVariables.Add("DPS", new CombatantData.TextExportFormatter("DPS", "Short DPS", "The damage total of the combatatant divided by their personal duration, formatted as 12K", (Data, Extra) => { return CombatantFormatSwitch(Data, "DPS", Extra); }));
            CombatantData.ExportVariables.Add("DPS-k", new CombatantData.TextExportFormatter("DPS-k", "Short DPS K", "Short DPS divided by 1,000 (with no decimal places)", (Data, Extra) => { return CombatantFormatSwitch(Data, "DPS-k", Extra); }));
            CombatantData.ExportVariables.Add("DPS-m", new CombatantData.TextExportFormatter("DPS-m", "Short DPS M", "Short DPS divided by 1,000,000 (with no decimal places)", (Data, Extra) => { return CombatantFormatSwitch(Data, "DPS-m", Extra); }));
            CombatantData.ExportVariables.Add("DPS-*", new CombatantData.TextExportFormatter("DPS-*", "Short DPS w/suffix", "Short DPS divided by 1/K/M/B/T/Q (with no decimal places)", (Data, Extra) => { return CombatantFormatSwitch(Data, "DPS-*", Extra); }));
            CombatantData.ExportVariables.Add("encdps", new CombatantData.TextExportFormatter("encdps", "Encounter DPS", "The damage total of the combatant divided by the duration of the encounter, formatted as 12.34 -- This is more commonly used than DPS", (Data, Extra) => { return CombatantFormatSwitch(Data, "encdps", Extra); }));
            CombatantData.ExportVariables.Add("encdps-*", new CombatantData.TextExportFormatter("encdps-*", "Encounter DPS w/suffix", "The damage total of the combatant divided by the duration of the encounter, formatted as 12.34 -- This is more commonly used than DPS", (Data, Extra) => { return CombatantFormatSwitch(Data, "encdps-*", Extra); }));
            CombatantData.ExportVariables.Add("ENCDPS", new CombatantData.TextExportFormatter("ENCDPS", "Short Encounter DPS", "The damage total of the combatant divided by the duration of the encounter, formatted as 12 -- This is more commonly used than DPS", (Data, Extra) => { return CombatantFormatSwitch(Data, "ENCDPS", Extra); }));
            CombatantData.ExportVariables.Add("ENCDPS-k", new CombatantData.TextExportFormatter("ENCDPS-k", "Short Encounter DPS K", "Short Encounter DPS divided by 1,000 (with no decimal places)", (Data, Extra) => { return CombatantFormatSwitch(Data, "ENCDPS-k", Extra); }));
            CombatantData.ExportVariables.Add("ENCDPS-m", new CombatantData.TextExportFormatter("ENCDPS-m", "Short Encounter DPS M", "Short Encounter DPS divided by 1,000,000 (with no decimal places)", (Data, Extra) => { return CombatantFormatSwitch(Data, "ENCDPS-m", Extra); }));
            CombatantData.ExportVariables.Add("ENCDPS-*", new CombatantData.TextExportFormatter("ENCDPS-*", "Short Encounter DPS w/suffix", "Short Encounter DPS divided by 1/K/M/B/T/Q (with no decimal places)", (Data, Extra) => { return CombatantFormatSwitch(Data, "ENCDPS-*", Extra); }));
            CombatantData.ExportVariables.Add("hits", new CombatantData.TextExportFormatter("hits", "Hits", "The number of attack attempts that produced damage.  IE a spell successfully doing damage.", (Data, Extra) => { return CombatantFormatSwitch(Data, "hits", Extra); }));
            CombatantData.ExportVariables.Add("crithits", new CombatantData.TextExportFormatter("crithits", "Critical Hit Count", "The number of damaging attacks that were critical.", (Data, Extra) => { return CombatantFormatSwitch(Data, "crithits", Extra); }));
            CombatantData.ExportVariables.Add("crithit%", new CombatantData.TextExportFormatter("crithit%", "Critical Hit Percentage", "The percentage of damaging attacks that were critical.", (Data, Extra) => { return CombatantFormatSwitch(Data, "crithit%", Extra); }));
            CombatantData.ExportVariables.Add("crittypes", new CombatantData.TextExportFormatter("crittypes", "Critical Types", "Distribution of Critical Types  (Normal|Legendary|Fabled|Mythical)", (Data, Extra) => { return CombatantFormatSwitch(Data, "crittypes", Extra); }));
            CombatantData.ExportVariables.Add("misses", new CombatantData.TextExportFormatter("misses", "Misses", "The number of auto-attacks or CAs that produced a miss message.", (Data, Extra) => { return CombatantFormatSwitch(Data, "misses", Extra); }));
            CombatantData.ExportVariables.Add("hitfailed", new CombatantData.TextExportFormatter("hitfailed", "Other Avoid", "Any type of failed attack that was not a miss.  This includes resists, reflects, blocks, dodging, etc.", (Data, Extra) => { return CombatantFormatSwitch(Data, "hitfailed", Extra); }));
            CombatantData.ExportVariables.Add("swings", new CombatantData.TextExportFormatter("swings", "Swings (Attacks)", "The number of attack attempts.  This includes any auto-attacks or abilities, also including resisted abilities that do no damage.", (Data, Extra) => { return CombatantFormatSwitch(Data, "swings", Extra); }));
            CombatantData.ExportVariables.Add("tohit", new CombatantData.TextExportFormatter("tohit", "To Hit %", "The percentage of hits to swings as 12.34", (Data, Extra) => { return CombatantFormatSwitch(Data, "tohit", Extra); }));
            CombatantData.ExportVariables.Add("TOHIT", new CombatantData.TextExportFormatter("TOHIT", "Short To Hit %", "The percentage of hits to swings as 12", (Data, Extra) => { return CombatantFormatSwitch(Data, "TOHIT", Extra); }));
            CombatantData.ExportVariables.Add("maxhit", new CombatantData.TextExportFormatter("maxhit", "Highest Hit", "The highest single damaging hit formatted as [Combatant-]SkillName-Damage#", (Data, Extra) => { return CombatantFormatSwitch(Data, "maxhit", Extra); }));
            CombatantData.ExportVariables.Add("MAXHIT", new CombatantData.TextExportFormatter("MAXHIT", "Short Highest Hit", "The highest single damaging hit formatted as [Combatant-]Damage#", (Data, Extra) => { return CombatantFormatSwitch(Data, "MAXHIT", Extra); }));
            CombatantData.ExportVariables.Add("maxhit-*", new CombatantData.TextExportFormatter("maxhit-*", "Highest Hit w/ suffix", "MaxHit divided by 1/K/M/B/T/Q", (Data, Extra) => { return CombatantFormatSwitch(Data, "maxhit-*", Extra); }));
            CombatantData.ExportVariables.Add("MAXHIT-*", new CombatantData.TextExportFormatter("MAXHIT-*", "Short Highest Hit w/ suffix", "Short MaxHit divided by 1/K/M/B/T/Q", (Data, Extra) => { return CombatantFormatSwitch(Data, "MAXHIT-*", Extra); }));
            CombatantData.ExportVariables.Add("healed", new CombatantData.TextExportFormatter("healed", "Healed", "The numerical total of all heals, wards or similar sourced from this combatant.", (Data, Extra) => { return CombatantFormatSwitch(Data, "healed", Extra); }));
            CombatantData.ExportVariables.Add("healed%", new CombatantData.TextExportFormatter("healed%", "Healed %", "This value represents the percent share of all healing done by allies in this encounter.", (Data, Extra) => { return CombatantFormatSwitch(Data, "healed%", Extra); }));
            CombatantData.ExportVariables.Add("enchps", new CombatantData.TextExportFormatter("enchps", "Encounter HPS", "The healing total of the combatant divided by the duration of the encounter, formatted as 12.34", (Data, Extra) => { return CombatantFormatSwitch(Data, "enchps", Extra); }));
            CombatantData.ExportVariables.Add("enchps-*", new CombatantData.TextExportFormatter("enchps-*", "Encounter HPS w/suffix", "Encounter HPS divided by 1/K/M/B/T/Q", (Data, Extra) => { return CombatantFormatSwitch(Data, "enchps-*", Extra); }));
            CombatantData.ExportVariables.Add("ENCHPS", new CombatantData.TextExportFormatter("ENCHPS", "Short Encounter HPS", "The healing total of the combatant divided by the duration of the encounter, formatted as 12", (Data, Extra) => { return CombatantFormatSwitch(Data, "ENCHPS", Extra); }));
            CombatantData.ExportVariables.Add("ENCHPS-k", new CombatantData.TextExportFormatter("ENCHPS-k", "Short Encounter HPS K", "Short Encounter HPS divided by 1,000 (with no decimal places)", (Data, Extra) => { return CombatantFormatSwitch(Data, "ENCHPS-k", Extra); }));
            CombatantData.ExportVariables.Add("ENCHPS-m", new CombatantData.TextExportFormatter("ENCHPS-m", "Short Encounter HPS M", "Short Encounter HPS divided by 1,000,000 (with no decimal places)", (Data, Extra) => { return CombatantFormatSwitch(Data, "ENCHPS-m", Extra); }));
            CombatantData.ExportVariables.Add("ENCHPS-*", new CombatantData.TextExportFormatter("ENCHPS-*", "Short Encounter HPS w/suffix", "Short Encounter HPS divided by 1/K/M/B/T/Q (with no decimal places)", (Data, Extra) => { return CombatantFormatSwitch(Data, "ENCHPS-*", Extra); }));
            CombatantData.ExportVariables.Add("critheals", new CombatantData.TextExportFormatter("critheals", "Critical Heal Count", "The number of heals that were critical.", (Data, Extra) => { return CombatantFormatSwitch(Data, "critheals", Extra); }));
            CombatantData.ExportVariables.Add("critheal%", new CombatantData.TextExportFormatter("critheal%", "Critical Heal Percentage", "The percentage of heals that were critical.", (Data, Extra) => { return CombatantFormatSwitch(Data, "critheal%", Extra); }));
            CombatantData.ExportVariables.Add("heals", new CombatantData.TextExportFormatter("heals", "Heal Count", "The total number of heals from this combatant.", (Data, Extra) => { return CombatantFormatSwitch(Data, "heals", Extra); }));
            CombatantData.ExportVariables.Add("cures", new CombatantData.TextExportFormatter("cures", "Cure or Dispel Count", "The total number of times the combatant cured or dispelled", (Data, Extra) => { return CombatantFormatSwitch(Data, "cures", Extra); }));
            CombatantData.ExportVariables.Add("maxheal", new CombatantData.TextExportFormatter("maxheal", "Highest Heal", "The highest single healing amount formatted as [Combatant-]SkillName-Healing#", (Data, Extra) => { return CombatantFormatSwitch(Data, "maxheal", Extra); }));
            CombatantData.ExportVariables.Add("MAXHEAL", new CombatantData.TextExportFormatter("MAXHEAL", "Short Highest Heal", "The highest single healing amount formatted as [Combatant-]Healing#", (Data, Extra) => { return CombatantFormatSwitch(Data, "MAXHEAL", Extra); }));
            CombatantData.ExportVariables.Add("maxhealward", new CombatantData.TextExportFormatter("maxhealward", "Highest Heal/Ward", "The highest single healing/warding amount formatted as [Combatant-]SkillName-Healing#", (Data, Extra) => { return CombatantFormatSwitch(Data, "maxhealward", Extra); }));
            CombatantData.ExportVariables.Add("MAXHEALWARD", new CombatantData.TextExportFormatter("MAXHEALWARD", "Short Highest Heal/Ward", "The highest single healing/warding amount formatted as [Combatant-]Healing#", (Data, Extra) => { return CombatantFormatSwitch(Data, "MAXHEALWARD", Extra); }));
            CombatantData.ExportVariables.Add("maxheal-*", new CombatantData.TextExportFormatter("maxheal-*", "Highest Heal w/ suffix", "Highest Heal divided by 1/K/M/B/T/Q", (Data, Extra) => { return CombatantFormatSwitch(Data, "maxheal-*", Extra); }));
            CombatantData.ExportVariables.Add("MAXHEAL-*", new CombatantData.TextExportFormatter("MAXHEAL-*", "Short Highest Heal w/ suffix", "Short Highest Heal divided by 1/K/M/B/T/Q", (Data, Extra) => { return CombatantFormatSwitch(Data, "MAXHEAL-*", Extra); }));
            CombatantData.ExportVariables.Add("maxhealward-*", new CombatantData.TextExportFormatter("maxhealward-*", "Highest Heal/Ward w/ suffix", "Highest Heal/Ward divided by 1/K/M/B/T/Q", (Data, Extra) => { return CombatantFormatSwitch(Data, "maxhealward-*", Extra); }));
            CombatantData.ExportVariables.Add("MAXHEALWARD-*", new CombatantData.TextExportFormatter("MAXHEALWARD-*", "Short Highest Heal/Ward w/ suffix", "Short Highest Heal/Ward divided by 1/K/M/B/T/Q", (Data, Extra) => { return CombatantFormatSwitch(Data, "MAXHEALWARD-*", Extra); }));
            CombatantData.ExportVariables.Add("damagetaken", new CombatantData.TextExportFormatter("damagetaken", "Damage Received", "The total amount of damage this combatant received.", (Data, Extra) => { return CombatantFormatSwitch(Data, "damagetaken", Extra); }));
            CombatantData.ExportVariables.Add("damagetaken-*", new CombatantData.TextExportFormatter("damagetaken-*", "Damage Received w/suffix", "Damage Received divided by 1/K/M/B/T/Q", (Data, Extra) => { return CombatantFormatSwitch(Data, "damagetaken-*", Extra); }));
            CombatantData.ExportVariables.Add("healstaken", new CombatantData.TextExportFormatter("healstaken", "Healing Received", "The total amount of healing this combatant received.", (Data, Extra) => { return CombatantFormatSwitch(Data, "healstaken", Extra); }));
            CombatantData.ExportVariables.Add("healstaken-*", new CombatantData.TextExportFormatter("healstaken-*", "Healing Received w/suffix", "Healing Received divided by 1/K/M/B/T/Q", (Data, Extra) => { return CombatantFormatSwitch(Data, "healstaken-*", Extra); }));
            CombatantData.ExportVariables.Add("kills", new CombatantData.TextExportFormatter("kills", "Killing Blows", "The total number of times this character landed a killing blow.", (Data, Extra) => { return CombatantFormatSwitch(Data, "kills", Extra); }));
            CombatantData.ExportVariables.Add("deaths", new CombatantData.TextExportFormatter("deaths", "Deaths", "The total number of times this character was killed by another.", (Data, Extra) => { return CombatantFormatSwitch(Data, "deaths", Extra); }));
            CombatantData.ExportVariables.Add("NAME3", new CombatantData.TextExportFormatter("NAME3", "Name (3 chars)", "The combatant's name, up to 3 characters will be displayed.", (Data, Extra) => { return CombatantFormatSwitch(Data, "NAME3", Extra); }));
            CombatantData.ExportVariables.Add("NAME4", new CombatantData.TextExportFormatter("NAME4", "Name (4 chars)", "The combatant's name, up to 4 characters will be displayed.", (Data, Extra) => { return CombatantFormatSwitch(Data, "NAME4", Extra); }));
            CombatantData.ExportVariables.Add("NAME5", new CombatantData.TextExportFormatter("NAME5", "Name (5 chars)", "The combatant's name, up to 5 characters will be displayed.", (Data, Extra) => { return CombatantFormatSwitch(Data, "NAME5", Extra); }));
            CombatantData.ExportVariables.Add("NAME6", new CombatantData.TextExportFormatter("NAME6", "Name (6 chars)", "The combatant's name, up to 6 characters will be displayed.", (Data, Extra) => { return CombatantFormatSwitch(Data, "NAME6", Extra); }));
            CombatantData.ExportVariables.Add("NAME7", new CombatantData.TextExportFormatter("NAME7", "Name (7 chars)", "The combatant's name, up to 7 characters will be displayed.", (Data, Extra) => { return CombatantFormatSwitch(Data, "NAME7", Extra); }));
            CombatantData.ExportVariables.Add("NAME8", new CombatantData.TextExportFormatter("NAME8", "Name (8 chars)", "The combatant's name, up to 8 characters will be displayed.", (Data, Extra) => { return CombatantFormatSwitch(Data, "NAME8", Extra); }));
            CombatantData.ExportVariables.Add("NAME9", new CombatantData.TextExportFormatter("NAME9", "Name (9 chars)", "The combatant's name, up to 9 characters will be displayed.", (Data, Extra) => { return CombatantFormatSwitch(Data, "NAME9", Extra); }));
            CombatantData.ExportVariables.Add("NAME10", new CombatantData.TextExportFormatter("NAME10", "Name (10 chars)", "The combatant's name, up to 10 characters will be displayed.", (Data, Extra) => { return CombatantFormatSwitch(Data, "NAME10", Extra); }));
            CombatantData.ExportVariables.Add("NAME11", new CombatantData.TextExportFormatter("NAME11", "Name (11 chars)", "The combatant's name, up to 11 characters will be displayed.", (Data, Extra) => { return CombatantFormatSwitch(Data, "NAME11", Extra); }));
            CombatantData.ExportVariables.Add("NAME12", new CombatantData.TextExportFormatter("NAME12", "Name (12 chars)", "The combatant's name, up to 12 characters will be displayed.", (Data, Extra) => { return CombatantFormatSwitch(Data, "NAME12", Extra); }));
            CombatantData.ExportVariables.Add("NAME13", new CombatantData.TextExportFormatter("NAME13", "Name (13 chars)", "The combatant's name, up to 13 characters will be displayed.", (Data, Extra) => { return CombatantFormatSwitch(Data, "NAME13", Extra); }));
            CombatantData.ExportVariables.Add("NAME14", new CombatantData.TextExportFormatter("NAME14", "Name (14 chars)", "The combatant's name, up to 14 characters will be displayed.", (Data, Extra) => { return CombatantFormatSwitch(Data, "NAME14", Extra); }));
            CombatantData.ExportVariables.Add("NAME15", new CombatantData.TextExportFormatter("NAME15", "Name (15 chars)", "The combatant's name, up to 15 characters will be displayed.", (Data, Extra) => { return CombatantFormatSwitch(Data, "NAME15", Extra); }));

            #endregion

            #region DamageTypeData
            DamageTypeData.ColumnDefs.Clear();
            DamageTypeData.ColumnDefs.Add("EncounterId", new DamageTypeData.ColumnDef("EncounterId", false, "INT", "EncounterId", (Data) => { return Data.Parent.Parent.EncId; }, (Data) => { return Data.Parent.Parent.EncId; }));
            DamageTypeData.ColumnDefs.Add("Combatant", new DamageTypeData.ColumnDef("Combatant", false, "VARCHAR(64)", "Combatant", (Data) => { return Data.Parent.Name; }, (Data) => { return Data.Parent.Name; }));
            DamageTypeData.ColumnDefs.Add("Grouping", new DamageTypeData.ColumnDef("Grouping", false, "VARCHAR(92)", "Grouping", (Data) => { return string.Empty; }, GetDamageTypeGrouping));
            DamageTypeData.ColumnDefs.Add("Type", new DamageTypeData.ColumnDef("Type", true, "VARCHAR(64)", "Type", (Data) => { return Data.Type; }, (Data) => { return Data.Type; }));
            DamageTypeData.ColumnDefs.Add("StartTime", new DamageTypeData.ColumnDef("StartTime", false, "TIMESTAMP", "StartTime", (Data) => { return Data.StartTime == DateTime.MaxValue ? "--:--:--" : Data.StartTime.ToString("T"); }, (Data) => { return Data.StartTime == DateTime.MaxValue ? "0000-00-00 00:00:00" : Data.StartTime.ToString("u").TrimEnd(new char[] { 'Z' }); }));
            DamageTypeData.ColumnDefs.Add("EndTime", new DamageTypeData.ColumnDef("EndTime", false, "TIMESTAMP", "EndTime", (Data) => { return Data.EndTime == DateTime.MinValue ? "--:--:--" : Data.EndTime.ToString("T"); }, (Data) => { return Data.EndTime == DateTime.MinValue ? "0000-00-00 00:00:00" : Data.EndTime.ToString("u").TrimEnd(new char[] { 'Z' }); }));
            DamageTypeData.ColumnDefs.Add("Duration", new DamageTypeData.ColumnDef("Duration", false, "INT", "Duration", (Data) => { return Data.DurationS; }, (Data) => { return Data.Duration.TotalSeconds.ToString("0"); }));
            DamageTypeData.ColumnDefs.Add("Damage", new DamageTypeData.ColumnDef("Damage", true, "BIGINT", "Damage", (Data) => { return Data.Damage.ToString(); }, (Data) => { return Data.Damage.ToString(); }));
            DamageTypeData.ColumnDefs.Add("EncDPS", new DamageTypeData.ColumnDef("EncDPS", true, "DOUBLE", "EncDPS", (Data) => { return Data.EncDPS.ToString(); }, (Data) => { return Data.EncDPS.ToString(usCulture); }));
            DamageTypeData.ColumnDefs.Add("CharDPS", new DamageTypeData.ColumnDef("CharDPS", false, "DOUBLE", "CharDPS", (Data) => { return Data.CharDPS.ToString(); }, (Data) => { return Data.CharDPS.ToString(usCulture); }));
            DamageTypeData.ColumnDefs.Add("DPS", new DamageTypeData.ColumnDef("DPS", false, "DOUBLE", "DPS", (Data) => { return Data.DPS.ToString(); }, (Data) => { return Data.DPS.ToString(usCulture); }));
            DamageTypeData.ColumnDefs.Add("Average", new DamageTypeData.ColumnDef("Average", true, "DOUBLE", "Average", (Data) => { return Data.Average.ToString(); }, (Data) => { return Data.Average.ToString(usCulture); }));
            DamageTypeData.ColumnDefs.Add("Median", new DamageTypeData.ColumnDef("Median", false, "BIGINT", "Median", (Data) => { return Data.Median.ToString(); }, (Data) => { return Data.Median.ToString(); }));
            DamageTypeData.ColumnDefs.Add("MinHit", new DamageTypeData.ColumnDef("MinHit", true, "BIGINT", "MinHit", (Data) => { return Data.MinHit.ToString(); }, (Data) => { return Data.MinHit.ToString(); }));
            DamageTypeData.ColumnDefs.Add("MaxHit", new DamageTypeData.ColumnDef("MaxHit", true, "BIGINT", "MaxHit", (Data) => { return Data.MaxHit.ToString(); }, (Data) => { return Data.MaxHit.ToString(); }));
            DamageTypeData.ColumnDefs.Add("Hits", new DamageTypeData.ColumnDef("Hits", true, "INT", "Hits", (Data) => { return Data.Hits.ToString(); }, (Data) => { return Data.Hits.ToString(); }));
            DamageTypeData.ColumnDefs.Add("CritHits", new DamageTypeData.ColumnDef("CritHits", false, "INT", "CritHits", (Data) => { return Data.CritHits.ToString(); }, (Data) => { return Data.CritHits.ToString(); }));
            DamageTypeData.ColumnDefs.Add("Avoids", new DamageTypeData.ColumnDef("Avoids", false, "INT", "Blocked", (Data) => { return Data.Blocked.ToString(); }, (Data) => { return Data.Blocked.ToString(); }));
            DamageTypeData.ColumnDefs.Add("Misses", new DamageTypeData.ColumnDef("Misses", false, "INT", "Misses", (Data) => { return Data.Misses.ToString(); }, (Data) => { return Data.Misses.ToString(); }));
            DamageTypeData.ColumnDefs.Add("Swings", new DamageTypeData.ColumnDef("Swings", true, "INT", "Swings", (Data) => { return Data.Swings.ToString(); }, (Data) => { return Data.Swings.ToString(); }));
            DamageTypeData.ColumnDefs.Add("ToHit", new DamageTypeData.ColumnDef("ToHit", false, "FLOAT", "ToHit", (Data) => { return Data.ToHit.ToString(); }, (Data) => { return Data.ToHit.ToString(); }));
            DamageTypeData.ColumnDefs.Add("AvgDelay", new DamageTypeData.ColumnDef("AvgDelay", false, "FLOAT", "AverageDelay", (Data) => { return Data.AverageDelay.ToString(); }, (Data) => { return Data.AverageDelay.ToString(usCulture); }));
            DamageTypeData.ColumnDefs.Add("Crit%", new DamageTypeData.ColumnDef("Crit%", false, "VARCHAR(8)", "CritPerc", (Data) => { return Data.CritPerc.ToString("0'%"); }, (Data) => { return Data.CritPerc.ToString("0'%"); }));
            DamageTypeData.ColumnDefs.Add("Specials", new DamageTypeData.ColumnDef("Specials", true, "VARCHAR(32)", "Specials", DamageTypeDataGetCritTypes, DamageTypeDataGetCritTypes));
            #endregion

            #region AttackType
            AttackType.ColumnDefs.Clear();
            AttackType.ColumnDefs.Add("EncounterId", new AttackType.ColumnDef("EncounterId", false, "INT", "EncounterId", (Data) => { return Data.Parent.Parent.Parent.EncId; }, (Data) => { return Data.Parent.Parent.Parent.EncId; }, (Left, Right) => { return Left.Parent.Parent.Parent.EncId.CompareTo(Right.Parent.Parent.Parent.EncId); }));
            AttackType.ColumnDefs.Add("Attacker", new AttackType.ColumnDef("Attacker", false, "VARCHAR(64)", "Attacker", (Data) => { return Data.Parent.Outgoing ? Data.Parent.Parent.Name : string.Empty; }, (Data) => { return Data.Parent.Outgoing ? Data.Parent.Parent.Name : string.Empty; }, (Left, Right) => { return 0; }));
            AttackType.ColumnDefs.Add("Victim", new AttackType.ColumnDef("Victim", false, "VARCHAR(64)", "Victim", (Data) => { return Data.Parent.Outgoing ? string.Empty : Data.Parent.Parent.Name; }, (Data) => { return Data.Parent.Outgoing ? string.Empty : Data.Parent.Parent.Name; }, (Left, Right) => { return 0; }));
            AttackType.ColumnDefs.Add("SwingType", new AttackType.ColumnDef("SwingType", false, "TINYINT", "SwingType", GetAttackTypeSwingType, GetAttackTypeSwingType, (Left, Right) => { return 0; }));
            AttackType.ColumnDefs.Add("Type", new AttackType.ColumnDef("Type", true, "VARCHAR(64)", "Type", (Data) => { return Data.Type; }, (Data) => { return Data.Type; }, (Left, Right) => { return Left.Type.CompareTo(Right.Type); }));
            AttackType.ColumnDefs.Add("StartTime", new AttackType.ColumnDef("StartTime", false, "TIMESTAMP", "StartTime", (Data) => { return Data.StartTime == DateTime.MaxValue ? "--:--:--" : Data.StartTime.ToString("T"); }, (Data) => { return Data.StartTime == DateTime.MaxValue ? "0000-00-00 00:00:00" : Data.StartTime.ToString("u").TrimEnd(new char[] { 'Z' }); }, (Left, Right) => { return Left.StartTime.CompareTo(Right.StartTime); }));
            AttackType.ColumnDefs.Add("EndTime", new AttackType.ColumnDef("EndTime", false, "TIMESTAMP", "EndTime", (Data) => { return Data.EndTime == DateTime.MinValue ? "--:--:--" : Data.EndTime.ToString("T"); }, (Data) => { return Data.EndTime == DateTime.MinValue ? "0000-00-00 00:00:00" : Data.EndTime.ToString("u").TrimEnd(new char[] { 'Z' }); }, (Left, Right) => { return Left.EndTime.CompareTo(Right.EndTime); }));
            AttackType.ColumnDefs.Add("Duration", new AttackType.ColumnDef("Duration", false, "INT", "Duration", (Data) => { return Data.DurationS; }, (Data) => { return Data.Duration.TotalSeconds.ToString("0"); }, (Left, Right) => { return Left.Duration.CompareTo(Right.Duration); }));
            AttackType.ColumnDefs.Add("Damage", new AttackType.ColumnDef("Damage", true, "BIGINT", "Damage", (Data) => { return Data.Damage.ToString(GetIntCommas()); }, (Data) => { return Data.Damage.ToString(); }, (Left, Right) => { return Left.Damage.CompareTo(Right.Damage); }));
            AttackType.ColumnDefs.Add("EncDPS", new AttackType.ColumnDef("EncDPS", true, "DOUBLE", "EncDPS", (Data) => { return Data.EncDPS.ToString(GetFloatCommas()); }, (Data) => { return Data.EncDPS.ToString(usCulture); }, (Left, Right) => { return Left.EncDPS.CompareTo(Right.EncDPS); }));
            AttackType.ColumnDefs.Add("CharDPS", new AttackType.ColumnDef("CharDPS", false, "DOUBLE", "CharDPS", (Data) => { return Data.CharDPS.ToString(GetFloatCommas()); }, (Data) => { return Data.CharDPS.ToString(usCulture); }, (Left, Right) => { return Left.CharDPS.CompareTo(Right.CharDPS); }));
            AttackType.ColumnDefs.Add("DPS", new AttackType.ColumnDef("DPS", false, "DOUBLE", "DPS", (Data) => { return Data.DPS.ToString(GetFloatCommas()); }, (Data) => { return Data.DPS.ToString(usCulture); }, (Left, Right) => { return Left.DPS.CompareTo(Right.DPS); }));
            AttackType.ColumnDefs.Add("Average", new AttackType.ColumnDef("Average", true, "DOUBLE", "Average", (Data) => { return Data.Average.ToString(GetFloatCommas()); }, (Data) => { return Data.Average.ToString(usCulture); }, (Left, Right) => { return Left.Average.CompareTo(Right.Average); }));
            AttackType.ColumnDefs.Add("Median", new AttackType.ColumnDef("Median", true, "BIGINT", "Median", (Data) => { return Data.Median.ToString(GetIntCommas()); }, (Data) => { return Data.Median.ToString(); }, (Left, Right) => { return Left.Median.CompareTo(Right.Median); }));
            AttackType.ColumnDefs.Add("MinHit", new AttackType.ColumnDef("MinHit", true, "BIGINT", "MinHit", (Data) => { return Data.MinHit.ToString(GetIntCommas()); }, (Data) => { return Data.MinHit.ToString(); }, (Left, Right) => { return Left.MinHit.CompareTo(Right.MinHit); }));
            AttackType.ColumnDefs.Add("MaxHit", new AttackType.ColumnDef("MaxHit", true, "BIGINT", "MaxHit", (Data) => { return Data.MaxHit.ToString(GetIntCommas()); }, (Data) => { return Data.MaxHit.ToString(); }, (Left, Right) => { return Left.MaxHit.CompareTo(Right.MaxHit); }));
            AttackType.ColumnDefs.Add("Resist", new AttackType.ColumnDef("Resist", true, "VARCHAR(64)", "Resist", (Data) => { return Data.Resist; }, (Data) => { return Data.Resist; }, (Left, Right) => { return Left.Resist.CompareTo(Right.Resist); }));
            AttackType.ColumnDefs.Add("Hits", new AttackType.ColumnDef("Hits", true, "INT", "Hits", (Data) => { return Data.Hits.ToString(GetIntCommas()); }, (Data) => { return Data.Hits.ToString(); }, (Left, Right) => { return Left.Hits.CompareTo(Right.Hits); }));
            AttackType.ColumnDefs.Add("CritHits", new AttackType.ColumnDef("CritHits", false, "INT", "CritHits", (Data) => { return Data.CritHits.ToString(GetIntCommas()); }, (Data) => { return Data.CritHits.ToString(); }, (Left, Right) => { return Left.CritHits.CompareTo(Right.CritHits); }));
            AttackType.ColumnDefs.Add("Avoids", new AttackType.ColumnDef("Avoids", false, "INT", "Blocked", (Data) => { return Data.Blocked.ToString(GetIntCommas()); }, (Data) => { return Data.Blocked.ToString(); }, (Left, Right) => { return Left.Blocked.CompareTo(Right.Blocked); }));
            AttackType.ColumnDefs.Add("Misses", new AttackType.ColumnDef("Misses", false, "INT", "Misses", (Data) => { return Data.Misses.ToString(GetIntCommas()); }, (Data) => { return Data.Misses.ToString(); }, (Left, Right) => { return Left.Misses.CompareTo(Right.Misses); }));
            AttackType.ColumnDefs.Add("Swings", new AttackType.ColumnDef("Swings", true, "INT", "Swings", (Data) => { return Data.Swings.ToString(GetIntCommas()); }, (Data) => { return Data.Swings.ToString(); }, (Left, Right) => { return Left.Swings.CompareTo(Right.Swings); }));
            AttackType.ColumnDefs.Add("ToHit", new AttackType.ColumnDef("ToHit", true, "FLOAT", "ToHit", (Data) => { return Data.ToHit.ToString(GetFloatCommas()); }, (Data) => { return Data.ToHit.ToString(usCulture); }, (Left, Right) => { return Left.ToHit.CompareTo(Right.ToHit); }));
            AttackType.ColumnDefs.Add("AvgDelay", new AttackType.ColumnDef("AvgDelay", false, "FLOAT", "AverageDelay", (Data) => { return Data.AverageDelay.ToString(GetFloatCommas()); }, (Data) => { return Data.AverageDelay.ToString(usCulture); }, (Left, Right) => { return Left.AverageDelay.CompareTo(Right.AverageDelay); }));
            AttackType.ColumnDefs.Add("Specials", new AttackType.ColumnDef("Specials", true, "VARCHAR(32)", "Specials", AttackTypeGetCritTypes, AttackTypeGetCritTypes, (Left, Right) => { return AttackTypeGetCritTypes(Left).CompareTo(AttackTypeGetCritTypes(Right)); }));

            #endregion

            #region MasterSwing
            MasterSwing.ColumnDefs.Clear();
            MasterSwing.ColumnDefs.Add("EncounterId", new MasterSwing.ColumnDef("EncounterId", false, "INT", "EncounterId", (Data) => { return Data.ParentEncounter.EncId; }, (Data) => { return Data.ParentEncounter.EncId; }, (Left, Right) => { return Left.ParentEncounter.EncId.CompareTo(Right.ParentEncounter.EncId); }));
            MasterSwing.ColumnDefs.Add("Time", new MasterSwing.ColumnDef("Time", true, "TIMESTAMP", "STime", (Data) => { return Data.Time.ToString("T"); }, (Data) => { return Data.Time.ToString("u").TrimEnd(new char[] { 'Z' }); }, (Left, Right) => { return Left.Time.CompareTo(Right.Time); }));
            MasterSwing.ColumnDefs.Add("RelativeTime", new MasterSwing.ColumnDef("RelativeTime", true, "FLOAT", "RelativeTime", (Data) => { return Data.ParentEncounter != null ? (Data.Time - Data.ParentEncounter.StartTime).ToString("g") : String.Empty; }, (Data) => { return Data.ParentEncounter != null ? (Data.Time - Data.ParentEncounter.StartTime).TotalSeconds.ToString(usCulture) : String.Empty; }, (Left, Right) => { return Left.Time.CompareTo(Right.Time); }));
            MasterSwing.ColumnDefs.Add("Attacker", new MasterSwing.ColumnDef("Attacker", true, "VARCHAR(64)", "Attacker", (Data) => { return Data.Attacker; }, (Data) => { return Data.Attacker; }, (Left, Right) => { return Left.Attacker.CompareTo(Right.Attacker); }));
            MasterSwing.ColumnDefs.Add("SwingType", new MasterSwing.ColumnDef("SwingType", false, "TINYINT", "SwingType", (Data) => { return Enum.GetName(typeof(SwingTypeEnum), (SwingTypeEnum)Data.SwingType); }, (Data) => { return Data.SwingType.ToString(); }, (Left, Right) => { return Left.SwingType.CompareTo(Right.SwingType); }));
            MasterSwing.ColumnDefs.Add("AttackType", new MasterSwing.ColumnDef("AttackType", true, "VARCHAR(64)", "AttackType", (Data) => { return Data.AttackType; }, (Data) => { return Data.AttackType; }, (Left, Right) => { return Left.AttackType.CompareTo(Right.AttackType); }));
            MasterSwing.ColumnDefs.Add("DamageType", new MasterSwing.ColumnDef("DamageType", true, "VARCHAR(64)", "DamageType", (Data) => { return Data.DamageType; }, (Data) => { return Data.DamageType; }, (Left, Right) => { return Left.DamageType.CompareTo(Right.DamageType); }));
            MasterSwing.ColumnDefs.Add("Victim", new MasterSwing.ColumnDef("Victim", true, "VARCHAR(64)", "Victim", (Data) => { return Data.Victim; }, (Data) => { return Data.Victim; }, (Left, Right) => { return Left.Victim.CompareTo(Right.Victim); }));
            MasterSwing.ColumnDefs.Add("Damage", new MasterSwing.ColumnDef("Damage", true, "BIGINT", "Damage", (Data) => { return Data.DamageType == "Miss" ? 0.ToString() : ((long)Data.Damage).ToString(); }, (Data) => { return Data.DamageType == "Miss" ? 0.ToString() : ((long)Data.Damage).ToString(); }, (Left, Right) => { return (Left.DamageType == "Miss" ? 0 : Left.Damage).CompareTo(Right.DamageType == "Miss" ? 0 : Right.Damage); }));
            MasterSwing.ColumnDefs.Add("Special", new MasterSwing.ColumnDef("Special", true, "VARCHAR(90)", "Special", (Data) => { return Data.Special == "None" ? String.Empty : Data.Special; }, (Data) => { return Data.Special; }, (Left, Right) => { return Left.Special.CompareTo(Right.Special); }));
            MasterSwing.ColumnDefs.Add("Log Time Δ", new MasterSwing.ColumnDef("Log Time Δ", true, "BIGINT", "Log Time Δ", (Data) => { return Data.Tags.ContainsKey(logTimestamp) ? ((DateTime)Data.Tags[logTimestamp] - Data.Time).ToString() : "Missing log timestamp"; }, (Data) => { return Data.Tags.ContainsKey(logTimestamp) ? ((DateTime)Data.Tags[logTimestamp] - Data.Time).ToString() : "Missing log timestamp"; }, (Left, Right) => { return (Left.Tags.ContainsKey(logTimestamp) && Right.Tags.ContainsKey(logTimestamp)) ? ((DateTime)Left.Tags[logTimestamp] - Left.Time).CompareTo((DateTime)Right.Tags[logTimestamp] - Right.Time) : 0; }));
            #endregion

            MasterSwing.ColumnDefs.ToList().ForEach((pair) =>
            {
                pair.Value.GetCellForeColor = (Data) => { return GetSwingTypeColor(Data.SwingType); };
            }
            );

            ActGlobals.oFormActMain.ValidateLists();
            ActGlobals.oFormActMain.ValidateTableSetup();
        }
        
        private string CombatantDataGetCritTypes(CombatantData Data)
        {
            AttackType at;
            if (Data.AllOut.TryGetValue(ActGlobals.ActLocalization.LocalizationStrings["attackTypeTerm-all"].DisplayedText, out at))
            {
                return AttackTypeGetCritTypes(at);
            }
            else
                return String.Empty;
        }
        
        private string DamageTypeDataGetCritTypes(DamageTypeData Data)
        {
            AttackType at;
            if (Data.Items.TryGetValue(ActGlobals.ActLocalization.LocalizationStrings["attackTypeTerm-all"].DisplayedText, out at))
            {
                return AttackTypeGetCritTypes(at);
            }
            else
                return String.Empty;
        }
        
        private string AttackTypeGetCritTypes(AttackType Data)
        {
            int special = 0;
            int specialCripplingBlow = 0;
            int specialLocked = 0;
            int specialCritical = 0;
            int specialStrikethrough = 0;
            int specialRiposte = 0;
            int specialNonDefined = 0;
            int specialFlurry = 0;
            int specialLucky = 0;
            int specialDoubleBowShot = 0;
            int specialTwincast = 0;

            for (int i = 1; i <= Data.Items.Count; i++)
            {
                MasterSwing ms = Data.Items[i];
                if (ms.Special.Length > 0 && ms.Special != "None")
                {
                    special++;
                    bool cripplingBlowFound = ms.Special.Contains(SpecialCripplingBlow);
                    bool lockedFound = ms.Special.Contains(SpecialLocked);
                    bool strikethroughFound = ms.Special.Contains(SpecialStrikethrough);
                    bool riposteFound = ms.Special.Contains(SpecialRiposte);
                    bool flurryFound = ms.Special.Contains(SpecialFlurry);
                    bool luckyFound = ms.Special.Contains(SpecialLucky);
                    bool doubleBowShotFound = ms.Special.Contains(SpecialDoubleBowShot);
                    bool twincastFound = ms.Special.Contains(SpecialTwincast);
                    if (cripplingBlowFound)
                    {
                        specialCripplingBlow++;
                    }
                    if (lockedFound)
                    {
                        specialLocked++;
                    }
                    if (ms.Critical)
                    {
                        specialCritical++;
                    }
                    if (strikethroughFound)
                    {
                        specialStrikethrough++;
                    }
                    if (riposteFound)
                    {
                        specialRiposte++;
                    }
                    if (flurryFound)
                    {
                        specialFlurry++;
                    }
                    if (luckyFound)
                    {
                        specialLucky++;
                    }
                    if (doubleBowShotFound)
                    {
                        specialDoubleBowShot++;
                    }
                    if (twincastFound)
                    {
                        specialTwincast++;
                    }
                    if (!(cripplingBlowFound || lockedFound || ms.Critical || strikethroughFound || riposteFound || flurryFound || luckyFound || doubleBowShotFound || twincastFound))
                        specialNonDefined++;
                }
            }
            if (special == 0)
                return string.Empty;
            float specialCripplingBlowPerc = ((float)specialCripplingBlow / (float)special) * 100f;
            float specialLockedPerc = ((float)specialLocked / (float)special) * 100f;
            float specialCriticalPerc = ((float)specialCritical / (float)special) * 100f;
            float specialNonDefinedPerc = ((float)specialNonDefined / (float)special) * 100f;
            float specialStrikethroughPerc = ((float)specialStrikethrough / (float)special) * 100f;
            float specialRipostePerc = ((float)specialRiposte / (float)special) * 100f;
            float specialFlurryPerc = ((float)specialFlurry / (float)special) * 100f;
            float speicalLuckyPerc = ((float)specialLucky / (float)special) * 100f;
            float specialDoubleBowShotPerc = ((float)specialDoubleBowShot / (float)special) * 100f;
            float specialTwincastPerc = ((float)specialTwincast / (float)special) * 100f;

            return $"{specialCripplingBlowPerc:000.0}%CB-{specialLockedPerc:000.0}%Locked-{specialCriticalPerc:000.0}%C-{specialStrikethroughPerc:000.0}%S-{specialRipostePerc:000.0}%R-{specialFlurryPerc:000.0}%F-{speicalLuckyPerc:000.0}%Lucky-{specialDoubleBowShotPerc:000.0}%DB-{specialTwincastPerc:000.0}%TC-{specialNonDefinedPerc:000.0}%ND";
        }
        
        private Color GetSwingTypeColor(int SwingType)
        {
            switch (SwingType)
            {
                case 1:
                case 2:
                    return Color.Crimson;
                case 3:
                    return Color.Blue;
                case 4:
                    return Color.DarkRed;
                case 5:
                    return Color.DarkOrange;
                case 8:
                    return Color.DarkOrchid;
                case 9:
                    return Color.DodgerBlue;
                default:
                    return Color.Black;
            }
        }
        
        private string EncounterFormatSwitch(EncounterData Data, List<CombatantData> SelectiveAllies, string VarName, string Extra)
        {
            long damage = 0;
            long healed = 0;
            int swings = 0;
            int hits = 0;
            int crits = 0;
            int heals = 0;
            int critheals = 0;
            int cures = 0;
            int misses = 0;
            int hitfail = 0;
            float tohit = 0;
            double dps = 0;
            double hps = 0;
            long healstaken = 0;
            long damagetaken = 0;
            int kills = 0;
            int deaths = 0;

            switch (VarName)
            {
                case "maxheal":
                    return Data.GetMaxHeal(true, false, false);
                case "MAXHEAL":
                    return Data.GetMaxHeal(false, false, false);
                case "maxheal-*":
                    return Data.GetMaxHeal(true, false, true);
                case "MAXHEAL-*":
                    return Data.GetMaxHeal(false, false, true);
                case "maxhealward":
                    return Data.GetMaxHeal(true, true, false);
                case "MAXHEALWARD":
                    return Data.GetMaxHeal(false, true, false);
                case "maxhealward-*":
                    return Data.GetMaxHeal(true, true, true);
                case "MAXHEALWARD-*":
                    return Data.GetMaxHeal(false, true, true);
                case "maxhit":
                    return Data.GetMaxHit(true, false);
                case "MAXHIT":
                    return Data.GetMaxHit(false, false);
                case "maxhit-*":
                    return Data.GetMaxHit(true, true);
                case "MAXHIT-*":
                    return Data.GetMaxHit(false, true);
                case "duration":
                    if (ActGlobals.wallClockDuration)
                    {
                        if (Data.Active)
                        {
                            if (ActGlobals.oFormActMain.LastEstimatedTime > Data.StartTime)
                            {
                                TimeSpan wallDuration = ActGlobals.oFormActMain.LastEstimatedTime - Data.StartTime;
                                if (wallDuration.Hours == 0)
                                    return String.Format("{0:00}:{1:00}", wallDuration.Minutes, wallDuration.Seconds);
                                else
                                    return String.Format("{0:00}:{1:00}:{2:00}", wallDuration.Hours, wallDuration.Minutes, wallDuration.Seconds);
                            }
                            else
                            {
                                return "00:00";
                            }
                        }
                        else
                        {
                            return Data.DurationS;
                        }
                    }
                    else
                        return Data.DurationS;
                case "DURATION":
                    if (ActGlobals.wallClockDuration)
                    {
                        if (Data.Active)
                        {
                            if (ActGlobals.oFormActMain.LastEstimatedTime > Data.StartTime)
                            {
                                TimeSpan wallDuration = ActGlobals.oFormActMain.LastEstimatedTime - Data.StartTime;
                                return ((int)wallDuration.TotalSeconds).ToString("0");
                            }
                            else
                            {
                                return "0";
                            }
                        }
                        else
                        {
                            return ((int)Data.Duration.TotalSeconds).ToString("0");
                        }
                    }
                    else
                        return Data.Duration.TotalSeconds.ToString("0");
                case "damage":
                    foreach (CombatantData cd in SelectiveAllies)
                        damage += cd.Damage;
                    return damage.ToString();
                case "damage-m":
                    foreach (CombatantData cd in SelectiveAllies)
                        damage += cd.Damage;
                    return (damage / 1000000.0).ToString("0.00");
                case "damage-b":
                    foreach (CombatantData cd in SelectiveAllies)
                        damage += cd.Damage;
                    return (damage / 1000000000.0).ToString("0.00");
                case "damage-*":
                    foreach (CombatantData cd in SelectiveAllies)
                        damage += cd.Damage;
                    return ActGlobals.oFormActMain.CreateDamageString(damage, true, true);
                case "DAMAGE-k":
                    foreach (CombatantData cd in SelectiveAllies)
                        damage += cd.Damage;
                    return (damage / 1000.0).ToString("0");
                case "DAMAGE-m":
                    foreach (CombatantData cd in SelectiveAllies)
                        damage += cd.Damage;
                    return (damage / 1000000.0).ToString("0");
                case "DAMAGE-b":
                    foreach (CombatantData cd in SelectiveAllies)
                        damage += cd.Damage;
                    return (damage / 1000000000.0).ToString("0");
                case "DAMAGE-*":
                    foreach (CombatantData cd in SelectiveAllies)
                        damage += cd.Damage;
                    return ActGlobals.oFormActMain.CreateDamageString(damage, true, false);
                case "healed":
                    foreach (CombatantData cd in SelectiveAllies)
                        healed += cd.Healed;
                    return healed.ToString();
                case "healed-*":
                    foreach (CombatantData cd in SelectiveAllies)
                        healed += cd.Healed;
                    return ActGlobals.oFormActMain.CreateDamageString(healed, true, true);
                case "swings":
                    foreach (CombatantData cd in SelectiveAllies)
                        swings += cd.Swings;
                    return swings.ToString();
                case "hits":
                    foreach (CombatantData cd in SelectiveAllies)
                        hits += cd.Hits;
                    return hits.ToString();
                case "crithits":
                    foreach (CombatantData cd in SelectiveAllies)
                        crits += cd.CritHits;
                    return crits.ToString();
                case "crithit%":
                    foreach (CombatantData cd in SelectiveAllies)
                        crits += cd.CritHits;
                    foreach (CombatantData cd in SelectiveAllies)
                        hits += cd.Hits;
                    float crithitperc = (float)crits / (float)hits;
                    return crithitperc.ToString("0'%");
                case "heals":
                    foreach (CombatantData cd in SelectiveAllies)
                        heals += cd.Heals;
                    return heals.ToString();
                case "critheals":
                    foreach (CombatantData cd in SelectiveAllies)
                        critheals += cd.CritHits;
                    return critheals.ToString();
                case "critheal%":
                    foreach (CombatantData cd in SelectiveAllies)
                        critheals += cd.CritHeals;
                    foreach (CombatantData cd in SelectiveAllies)
                        heals += cd.Heals;
                    float crithealperc = (float)critheals / (float)heals;
                    return crithealperc.ToString("0'%");
                case "cures":
                    foreach (CombatantData cd in SelectiveAllies)
                        cures += cd.CureDispels;
                    return cures.ToString();
                case "misses":
                    foreach (CombatantData cd in SelectiveAllies)
                        misses += cd.Misses;
                    return misses.ToString();
                case "hitfailed":
                    foreach (CombatantData cd in SelectiveAllies)
                        hitfail += cd.Blocked;
                    return hitfail.ToString();
                case "TOHIT":
                    foreach (CombatantData cd in SelectiveAllies)
                        tohit += cd.ToHit;
                    tohit /= SelectiveAllies.Count;
                    return tohit.ToString("0");
                case "DPS":
                case "ENCDPS":
                    foreach (CombatantData cd in SelectiveAllies)
                        damage += cd.Damage;
                    dps = damage / Data.Duration.TotalSeconds;
                    return dps.ToString("0");
                case "DPS-*":
                case "ENCDPS-*":
                    foreach (CombatantData cd in SelectiveAllies)
                        damage += cd.Damage;
                    dps = damage / Data.Duration.TotalSeconds;
                    return ActGlobals.oFormActMain.CreateDamageString((long)dps, true, false);
                case "DPS-k":
                case "ENCDPS-k":
                    foreach (CombatantData cd in SelectiveAllies)
                        damage += cd.Damage;
                    dps = damage / Data.Duration.TotalSeconds;
                    return (dps / 1000.0).ToString("0");
                case "ENCDPS-m":
                    foreach (CombatantData cd in SelectiveAllies)
                        damage += cd.Damage;
                    dps = damage / Data.Duration.TotalSeconds;
                    return (dps / 1000000.0).ToString("0");
                case "ENCHPS":
                    foreach (CombatantData cd in SelectiveAllies)
                        healed += cd.Healed;
                    hps = healed / Data.Duration.TotalSeconds;
                    return hps.ToString("0");
                case "ENCHPS-k":
                    foreach (CombatantData cd in SelectiveAllies)
                        healed += cd.Healed;
                    hps = healed / Data.Duration.TotalSeconds;
                    return (hps / 1000.0).ToString("0");
                case "ENCHPS-m":
                    foreach (CombatantData cd in SelectiveAllies)
                        healed += cd.Healed;
                    hps = healed / Data.Duration.TotalSeconds;
                    return (hps / 1000000.0).ToString("0");
                case "ENCHPS-*":
                    foreach (CombatantData cd in SelectiveAllies)
                        healed += cd.Healed;
                    hps = healed / Data.Duration.TotalSeconds;
                    return ActGlobals.oFormActMain.CreateDamageString((long)hps, true, false);
                case "tohit":
                    foreach (CombatantData cd in SelectiveAllies)
                        tohit += cd.ToHit;
                    tohit /= SelectiveAllies.Count;
                    return tohit.ToString("F");
                case "dps":
                case "encdps":
                    foreach (CombatantData cd in SelectiveAllies)
                        damage += cd.Damage;
                    dps = damage / Data.Duration.TotalSeconds;
                    return dps.ToString("F");
                case "dps-k":
                case "encdps-k":
                    foreach (CombatantData cd in SelectiveAllies)
                        damage += cd.Damage;
                    dps = damage / Data.Duration.TotalSeconds;
                    return (dps / 1000.0).ToString("F");
                case "encdps-m":
                    foreach (CombatantData cd in SelectiveAllies)
                        damage += cd.Damage;
                    dps = damage / Data.Duration.TotalSeconds;
                    return (dps / 1000000.0).ToString("F");
                case "encdps-*":
                    foreach (CombatantData cd in SelectiveAllies)
                        damage += cd.Damage;
                    dps = damage / Data.Duration.TotalSeconds;
                    return ActGlobals.oFormActMain.CreateDamageString((long)dps, true, true);
                case "enchps":
                    foreach (CombatantData cd in SelectiveAllies)
                        healed += cd.Healed;
                    hps = healed / Data.Duration.TotalSeconds;
                    return hps.ToString("F");
                case "enchps-k":
                    foreach (CombatantData cd in SelectiveAllies)
                        healed += cd.Healed;
                    hps = healed / Data.Duration.TotalSeconds;
                    return (hps / 1000.0).ToString("F");
                case "enchps-m":
                    foreach (CombatantData cd in SelectiveAllies)
                        healed += cd.Healed;
                    hps = healed / Data.Duration.TotalSeconds;
                    return (hps / 1000000.0).ToString("F");
                case "enchps-*":
                    foreach (CombatantData cd in SelectiveAllies)
                        healed += cd.Healed;
                    hps = healed / Data.Duration.TotalSeconds;
                    return ActGlobals.oFormActMain.CreateDamageString((long)hps, true, true);
                case "healstaken":
                    foreach (CombatantData cd in SelectiveAllies)
                        healstaken += cd.HealsTaken;
                    return healstaken.ToString();
                case "healstaken-*":
                    foreach (CombatantData cd in SelectiveAllies)
                        healstaken += cd.HealsTaken;
                    return ActGlobals.oFormActMain.CreateDamageString(healstaken, true, true);
                case "damagetaken":
                    foreach (CombatantData cd in SelectiveAllies)
                        damagetaken += cd.DamageTaken;
                    return damagetaken.ToString();
                case "damagetaken-*":
                    foreach (CombatantData cd in SelectiveAllies)
                        damagetaken += cd.DamageTaken;
                    return ActGlobals.oFormActMain.CreateDamageString(damagetaken, true, true);
                case "kills":
                    foreach (CombatantData cd in SelectiveAllies)
                        kills += cd.Kills;
                    return kills.ToString();
                case "deaths":
                    foreach (CombatantData cd in SelectiveAllies)
                        deaths += cd.Deaths;
                    return deaths.ToString();
                case "title":
                    return Data.Title;

                default:
                    return VarName;
            }
        }
        
        private string GetAttackTypeSwingType(AttackType Data)
        {
            int swingType = 100;
            List<int> swingTypes = new List<int>();
            List<MasterSwing> cachedItems = new List<MasterSwing>(Data.Items);
            for (int i = 0; i < cachedItems.Count; i++)
            {
                MasterSwing s = cachedItems[i];
                if (swingTypes.Contains(s.SwingType) == false)
                    swingTypes.Add(s.SwingType);
            }
            if (swingTypes.Count == 1)
                swingType = swingTypes[0];

            return swingType.ToString();
        }
        
        private string GetDamageTypeGrouping(DamageTypeData Data)
        {
            string grouping = string.Empty;

            int swingTypeIndex = 0;
            if (Data.Outgoing)
            {
                grouping += "attacker=" + Data.Parent.Name;
                foreach (KeyValuePair<int, List<string>> links in CombatantData.SwingTypeToDamageTypeDataLinksOutgoing)
                {
                    foreach (string damageTypeLabel in links.Value)
                    {
                        if (Data.Type == damageTypeLabel)
                        {
                            grouping += String.Format("&swingtype{0}={1}", swingTypeIndex++ == 0 ? string.Empty : swingTypeIndex.ToString(), links.Key);
                        }
                    }
                }
            }
            else
            {
                grouping += "victim=" + Data.Parent.Name;
                foreach (KeyValuePair<int, List<string>> links in CombatantData.SwingTypeToDamageTypeDataLinksIncoming)
                {
                    foreach (string damageTypeLabel in links.Value)
                    {
                        if (Data.Type == damageTypeLabel)
                        {
                            grouping += String.Format("&swingtype{0}={1}", swingTypeIndex++ == 0 ? string.Empty : swingTypeIndex.ToString(), links.Key);
                        }
                    }
                }
            }

            return grouping;
        }
        
        private string CombatantFormatSwitch(CombatantData Data, string VarName, string Extra)
        {
            int len = 0;
            switch (VarName)
            {
                case "name":
                    return Data.Name;
                case "NAME":
                    len = Int32.Parse(Extra);
                    return Data.Name.Length - len > 0 ? Data.Name.Remove(len, Data.Name.Length - len).Trim() : Data.Name;
                case "NAME3":
                    len = 3;
                    return Data.Name.Length - len > 0 ? Data.Name.Remove(len, Data.Name.Length - len).Trim() : Data.Name;
                case "NAME4":
                    len = 4;
                    return Data.Name.Length - len > 0 ? Data.Name.Remove(len, Data.Name.Length - len).Trim() : Data.Name;
                case "NAME5":
                    len = 5;
                    return Data.Name.Length - len > 0 ? Data.Name.Remove(len, Data.Name.Length - len).Trim() : Data.Name;
                case "NAME6":
                    len = 6;
                    return Data.Name.Length - len > 0 ? Data.Name.Remove(len, Data.Name.Length - len).Trim() : Data.Name;
                case "NAME7":
                    len = 7;
                    return Data.Name.Length - len > 0 ? Data.Name.Remove(len, Data.Name.Length - len).Trim() : Data.Name;
                case "NAME8":
                    len = 8;
                    return Data.Name.Length - len > 0 ? Data.Name.Remove(len, Data.Name.Length - len).Trim() : Data.Name;
                case "NAME9":
                    len = 9;
                    return Data.Name.Length - len > 0 ? Data.Name.Remove(len, Data.Name.Length - len).Trim() : Data.Name;
                case "NAME10":
                    len = 10;
                    return Data.Name.Length - len > 0 ? Data.Name.Remove(len, Data.Name.Length - len).Trim() : Data.Name;
                case "NAME11":
                    len = 11;
                    return Data.Name.Length - len > 0 ? Data.Name.Remove(len, Data.Name.Length - len).Trim() : Data.Name;
                case "NAME12":
                    len = 12;
                    return Data.Name.Length - len > 0 ? Data.Name.Remove(len, Data.Name.Length - len).Trim() : Data.Name;
                case "NAME13":
                    len = 13;
                    return Data.Name.Length - len > 0 ? Data.Name.Remove(len, Data.Name.Length - len).Trim() : Data.Name;
                case "NAME14":
                    len = 14;
                    return Data.Name.Length - len > 0 ? Data.Name.Remove(len, Data.Name.Length - len).Trim() : Data.Name;
                case "NAME15":
                    len = 15;
                    return Data.Name.Length - len > 0 ? Data.Name.Remove(len, Data.Name.Length - len).Trim() : Data.Name;
                case "DURATION":
                    return Data.Duration.TotalSeconds.ToString("0");
                case "duration":
                    return Data.DurationS;
                case "maxhit":
                    return Data.GetMaxHit(true, false);
                case "MAXHIT":
                    return Data.GetMaxHit(false, false);
                case "maxhit-*":
                    return Data.GetMaxHit(true, true);
                case "MAXHIT-*":
                    return Data.GetMaxHit(false, true);
                case "maxheal":
                    return Data.GetMaxHeal(true, false, false);
                case "MAXHEAL":
                    return Data.GetMaxHeal(false, false, false);
                case "maxheal-*":
                    return Data.GetMaxHeal(true, false, true);
                case "MAXHEAL-*":
                    return Data.GetMaxHeal(false, false, true);
                case "maxhealward":
                    return Data.GetMaxHeal(true, true, false);
                case "MAXHEALWARD":
                    return Data.GetMaxHeal(false, true, false);
                case "maxhealward-*":
                    return Data.GetMaxHeal(true, true, true);
                case "MAXHEALWARD-*":
                    return Data.GetMaxHeal(false, true, true);
                case "damage":
                    return Data.Damage.ToString();
                case "damage-k":
                    return (Data.Damage / 1000.0).ToString("0.00");
                case "damage-m":
                    return (Data.Damage / 1000000.0).ToString("0.00");
                case "damage-b":
                    return (Data.Damage / 1000000000.0).ToString("0.00");
                case "damage-*":
                    return ActGlobals.oFormActMain.CreateDamageString(Data.Damage, true, true);
                case "DAMAGE-k":
                    return (Data.Damage / 1000.0).ToString("0");
                case "DAMAGE-m":
                    return (Data.Damage / 1000000.0).ToString("0");
                case "DAMAGE-b":
                    return (Data.Damage / 1000000000.0).ToString("0");
                case "DAMAGE-*":
                    return ActGlobals.oFormActMain.CreateDamageString(Data.Damage, true, false);
                case "healed":
                    return Data.Healed.ToString();
                case "healed-*":
                    return ActGlobals.oFormActMain.CreateDamageString(Data.Healed, true, true);
                case "swings":
                    return Data.Swings.ToString();
                case "hits":
                    return Data.Hits.ToString();
                case "crithits":
                    return Data.CritHits.ToString();
                case "critheals":
                    return Data.CritHeals.ToString();
                case "crittypes":
                    return CombatantDataGetCritTypes(Data);
                case "crithit%":
                    return Data.CritDamPerc.ToString("0'%");
                case "critheal%":
                    return Data.CritHealPerc.ToString("0'%");
                case "heals":
                    return Data.Heals.ToString();
                case "cures":
                    return Data.CureDispels.ToString();
                case "misses":
                    return Data.Misses.ToString();
                case "hitfailed":
                    return Data.Blocked.ToString();
                case "TOHIT":
                    return Data.ToHit.ToString("0");
                case "DPS":
                    return Data.DPS.ToString("0");
                case "DPS-k":
                    return (Data.DPS / 1000.0).ToString("0");
                case "DPS-m":
                    return (Data.DPS / 1000000.0).ToString("0");
                case "DPS-*":
                    return ActGlobals.oFormActMain.CreateDamageString((long)Data.DPS, true, false);
                case "ENCDPS":
                    return Data.EncDPS.ToString("0");
                case "ENCDPS-k":
                    return (Data.EncDPS / 1000.0).ToString("0");
                case "ENCDPS-m":
                    return (Data.EncDPS / 1000000.0).ToString("0");
                case "ENCDPS-*":
                    return ActGlobals.oFormActMain.CreateDamageString((long)Data.EncDPS, true, false);
                case "ENCHPS":
                    return Data.EncHPS.ToString("0");
                case "ENCHPS-k":
                    return (Data.EncHPS / 1000.0).ToString("0");
                case "ENCHPS-m":
                    return (Data.EncHPS / 1000000.0).ToString("0");
                case "ENCHPS-*":
                    return ActGlobals.oFormActMain.CreateDamageString((long)Data.EncHPS, true, false);
                case "tohit":
                    return Data.ToHit.ToString("F");
                case "dps":
                    return Data.DPS.ToString("F");
                case "dps-k":
                    return (Data.DPS / 1000.0).ToString("F");
                case "dps-*":
                    return ActGlobals.oFormActMain.CreateDamageString((long)Data.DPS, true, true);
                case "encdps":
                    return Data.EncDPS.ToString("F");
                case "encdps-k":
                    return (Data.EncDPS / 1000.0).ToString("F");
                case "encdps-m":
                    return (Data.EncDPS / 1000000.0).ToString("F");
                case "encdps-*":
                    return ActGlobals.oFormActMain.CreateDamageString((long)Data.EncDPS, true, true);
                case "enchps":
                    return Data.EncHPS.ToString("F");
                case "enchps-k":
                    return (Data.EncHPS / 1000.0).ToString("F");
                case "enchps-m":
                    return (Data.EncHPS / 1000000.0).ToString("F");
                case "enchps-*":
                    return ActGlobals.oFormActMain.CreateDamageString((long)Data.EncHPS, true, true);
                case "healstaken":
                    return Data.HealsTaken.ToString();
                case "healstaken-*":
                    return ActGlobals.oFormActMain.CreateDamageString((long)Data.HealsTaken, true, true);
                case "damagetaken":
                    return Data.DamageTaken.ToString();
                case "damagetaken-*":
                    return ActGlobals.oFormActMain.CreateDamageString((long)Data.DamageTaken, true, true);
                case "powerdrain":
                    return Data.PowerDamage.ToString();
                case "powerdrain-*":
                    return ActGlobals.oFormActMain.CreateDamageString((long)Data.PowerDamage, true, true);
                case "powerheal":
                    return Data.PowerReplenish.ToString();
                case "powerheal-*":
                    return ActGlobals.oFormActMain.CreateDamageString((long)Data.PowerReplenish, true, true);
                case "kills":
                    return Data.Kills.ToString();
                case "deaths":
                    return Data.Deaths.ToString();
                case "damage%":
                    return Data.DamagePercent;
                case "healed%":
                    return Data.HealedPercent;
                case "threatstr":
                    return Data.GetThreatStr("Threat (Out)");
                case "threatdelta":
                    return Data.GetThreatDelta("Threat (Out)").ToString();
                case "n":
                    return "\n";
                case "t":
                    return "\t";

                default:
                    return VarName;
            }
        }
    }
}
