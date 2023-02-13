using Advanced_Combat_Tracker;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
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

[assembly: AssemblyTitle("EverQuest DPS Parsing")]
[assembly: AssemblyDescription("Plugin for EverQuest DPS Parsing")]
[assembly: AssemblyCompany("Blurrysticks")]
[assembly: AssemblyVersion("0.0.0.1")]
[assembly: AssemblyCopyright("2022")]

namespace ACT_Plugin
{
    public class ACT_English_Parser : UserControl, IActPluginV1
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
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(287, 122);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(142, 13);
            this.label16.TabIndex = 13;
            this.label16.Text = "Apostrophe name to be fixed";
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
        public ACT_English_Parser()
        {
            InitializeComponent();
        }

        TreeNode optionsNode = null;
        Label lblStatus;    // The status label that appears in ACT's Plugin tab
        string settingsFile = Path.Combine(ActGlobals.oFormActMain.AppDataFolder.FullName, "Config\\ACT_EverQuest_English_Parser.config.xml");
        SettingsSerializer xmlSettings;

        public void InitPlugin(TabPage pluginScreenSpace, Label pluginStatusText)
        {
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
                optionsNode = ActGlobals.oFormActMain.OptionsTreeView.Nodes[dcIndex].Nodes.Add("EverQuest English Settings");
                // Register our user control(this) to our newly create node path.  All controls added to the list will be laid out left to right, top to bottom
                ActGlobals.oFormActMain.OptionsControlSets.Add(@"Data Correction\EQ2 English Settings", new List<Control> { this });
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
            SetupEQEnvironment();   // Not really needed since ACT has this code internalized as well.
            ActGlobals.oFormActMain.BeforeLogLineRead += new LogLineEventDelegate(oFormActMain_BeforeLogLineRead);
            ActGlobals.oFormActMain.UpdateCheckClicked += new FormActMain.NullDelegate(oFormActMain_UpdateCheckClicked);
            if (ActGlobals.oFormActMain.GetAutomaticUpdatesAllowed())   // If ACT is set to automatically check for updates, check for updates to the plugin
                new Thread(new ThreadStart(oFormActMain_UpdateCheckClicked)).Start();   // If we don't put this on a separate thread, web latency will delay the plugin init phase
            ActGlobals.oFormActMain.CharacterFileNameRegex = new Regex(@"(?:.+)\/eqlog_(?<characterName>\S+)_(?<server>.+).txt");
            ActGlobals.oFormActMain.ZoneChangeRegex = new Regex(@"{logTimeStampRegexStr}(?:(?=You have entered)(You have entered (the Drunken Monkey stance adequately|(?<zoneName>.+)))).");
            lblStatus.Text = EverQuestDPSParse.PluginName + " Plugin Started";
        }

        public void DeInitPlugin()
        {
            ActGlobals.oFormActMain.BeforeLogLineRead -= oFormActMain_BeforeLogLineRead;
            ActGlobals.oFormActMain.UpdateCheckClicked -= oFormActMain_UpdateCheckClicked;

            if (optionsNode != null)    // If we added our user control to the Options tab, remove it
            {
                optionsNode.Remove();
                ActGlobals.oFormActMain.OptionsControlSets.Remove(@"Data Correction\EQ English Settings");
            }

            SaveSettings();
            lblStatus.Text = EverQuestDPSParse.PluginName + " Plugin Exited";
        }

        char[] chrApos = new char[] { '\'', '’' };
        char[] chrSpaceApos = new char[] { ' ', '\'', '’' };
        List<Tuple<Color, Regex>> regexTupleList = new List<Tuple<Color, Regex>>();
        private DateTime GetDateTimeFromGroupMatch(String dt)
        {
            String eqDateTimeStampFormat = "ddd MMM dd HH:mm:ss yyyy";
            DateTime currentEQTimeStamp;
            DateTime.TryParseExact(dt, eqDateTimeStampFormat, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.AssumeLocal, out currentEQTimeStamp);
            return currentEQTimeStamp;
        }
        private void PopulateRegexArray()
        {
            ActGlobals.oFormEncounterLogs.LogTypeToColorMapping.Clear();
            regexTupleList.Add(new Tuple<Color, Regex>(Color.Red, new Regex(EverQuestDPSParse.MeleeAttack, RegexOptions.Compiled)));
            ActGlobals.oFormEncounterLogs.LogTypeToColorMapping.Add(regexTupleList.Count - 1, regexTupleList[regexTupleList.Count - 1].Item1);
            regexTupleList.Add(new Tuple<Color, Regex>(Color.Red, new Regex(EverQuestDPSParse.DamageShield, RegexOptions.Compiled)));
            ActGlobals.oFormEncounterLogs.LogTypeToColorMapping.Add(regexTupleList.Count - 1, regexTupleList[regexTupleList.Count - 1].Item1);
            regexTupleList.Add(new Tuple<Color, Regex>(Color.Gray, new Regex(EverQuestDPSParse.MissedMeleeAttack, RegexOptions.Compiled)));
            ActGlobals.oFormEncounterLogs.LogTypeToColorMapping.Add(regexTupleList.Count - 1, regexTupleList[regexTupleList.Count - 1].Item1);
            regexTupleList.Add(new Tuple<Color, Regex>(Color.Goldenrod, new Regex(EverQuestDPSParse.SlainMessage, RegexOptions.Compiled)));
            ActGlobals.oFormEncounterLogs.LogTypeToColorMapping.Add(regexTupleList.Count - 1, regexTupleList[regexTupleList.Count - 1].Item1);
            regexTupleList.Add(new Tuple<Color, Regex>(Color.GhostWhite, new Regex(EverQuestDPSParse.TwinCast, RegexOptions.Compiled)));
            ActGlobals.oFormEncounterLogs.LogTypeToColorMapping.Add(regexTupleList.Count - 1, regexTupleList[regexTupleList.Count - 1].Item1);
            regexTupleList.Add(new Tuple<Color, Regex>(Color.Red, new Regex(EverQuestDPSParse.SpellDamage, RegexOptions.Compiled)));
        }
 
        void oFormActMain_BeforeLogLineRead(bool isImport, LogLineEventArgs logInfo)
        {

                for (int i = 0; i < regexTupleList.Length; i++)
                {
                    Match reMatch = regexArray[i].Match(logInfo.logLine);
                    if (reMatch.Success)
                    {
                        logInfo.detectedType = i + 1;
                        LogExeEnglish(reMatch, i + 1, logInfo.logLine, isImport);
                        break;
                    }
                }
            
        }

        /*string[] matchKeywords = new string[] { "damage", "point", ", but", "killed", "command", "entered", "hate", "dispel", "relieve", "reduces" };
        private bool NotQuickFail(LogLineEventArgs logInfo)
        {
            for (int i = 0; i < matchKeywords.Length; i++)
            {
                if (logInfo.logLine.Contains(matchKeywords[i]))
                    return true;
            }

            return false;
        }*/
        private void ParseEverQuestLogLine(Match reMatch, int logMatched, string logLine, bool isImport)
        {
            List<string> damages = new List<string>();
            SwingTypeEnum swingType;
            String attacker, victim;
            DateTime time = ActGlobals.oFormActMain.LastKnownTime;

            int gts = ActGlobals.oFormActMain.GlobalTimeSorter;
            if (reMatch.Groups["victim"].Success && reMatch.Groups["attacker"].Success)
                ActGlobals.oFormActMain.SetEncounter(GetDateTimeFromGroupMatch(reMatch.Groups["dateTimeOfLogLine"].Value), reMatch.Groups["victim"].Value, reMatch.Groups["attacker"].Value);

            switch (logMatched)
            {
                //Melee
                case 1:
                    if(ActGlobals.oFormActMain.InCombat)
                    {
                        String damageSpecial;
                        bool critical;
                        if (reMatch.Groups["damageSpecial"].Success)
                        {
                            damageSpecial = reMatch.Groups["damageSpecial"].Value;
                        }
                        else
                        {
                            damageSpecial = String.Empty;
                        }

                        critical = damageSpecial.Contains("Critical");
                        ActGlobals.oFormActMain.AddCombatAction(
                            SwingTypeEnum.Melee
                            , critical
                            , damageSpecial
                            , EnglishPersonaReplace(reMatch.Groups["attacker"].Value)
                            , reMatch.Groups["attackType"].Value
                            , new Dnum(Int64.Parse(reMatch.Groups["damageAmount"].Value))
                            , new DateTime(reMatch.Groups["dateTimeOfLogLine"].Value, DateTimeKind.Local)
                            , gts
                            , EnglishPersonaReplace(reMatch.Groups["victim"].Value)
                            , "Melee");
                    }
                    break;
                //Non-melee damage shield
                case 2:
                    if(ActGlobals.oFormActMain.InCombat)
                    {
                        ActGlobals.oFormActMain.AddCombatAction(SwingTypeEnum.NonMelee
                            , false
                            , String.Empty 
                            , EnglishPersonaReplace(reMatch.Groups["attacker"].Value)
                            , reMatch.Groups["damageShieldDamageType"].Value
                            , new Dnum(Int64.Parse(reMatch.Groups["damagePoints"].Value)
                            , new DateTime(reMatch.Groups["dateTimeOfLogLine"].Value
                            , gts
                            , EnglishPersonaReplace(reMatch.Groups["victim"].Value)
                            , reMatch.Groups["damageShieldType"].Value)
                    }
                //Melee miss
                case 3:
                    if(ActGlobals.oFormActMain.InCombat)
                    {
                        ActGlobals.oFormActMain.AddCombatAction(SwingTypeEnum.Melee
                            , false
                            , string.Empty
                            , EnglishPersonaReplace(reMatch.Groups["attacker"].Value)
                            , reMatch.Groups["attackType"].Value
                            , new Dnum(0)
                            , new DateTime(reMatch.Groups["dateTimeOfLogLine"].Value)
                            , gts
                            , EnglishPersonaReplace(reMatch.Groups["victim"].Value), "Melee");
                    }
                    break;
                    /*
                    #region Case 1 [unsourced skill attacks]
                    case 1:
                        if (reMatch.Groups[1].Value.Length > 60)
                            break;
                        if (ActGlobals.oFormActMain.InCombat)   // If in combat
                        {
                            victim = reMatch.Groups[1].Value;
                            crit = reMatch.Groups[2].Value;
                            skillType = reMatch.Groups[3].Value;
                            critStr = reMatch.Groups[4].Value;
                            damage = reMatch.Groups[5].Value;
                            special = "None";
                            critical = false;

                            damageAndTypeArr = EngGetDamageAndTypeArr(damage);

                            attacker = "Unknown";   // Unsourced melee hits show as "Unknown" attacking, so we do the same

                            if (crit[0] == 'c') // Critical hit check
                                critical = true;
                            if (!String.IsNullOrWhiteSpace(critStr))
                            {
                                critical = true;
                                critStr = critStr.Substring(2); // "a "
                                critStr = critStr.Substring(0, critStr.Length - 4); // " of "
                            }
                            else
                            {
                                critStr = "None";
                            }

                            if (victim == "YOU")
                                victim = ActGlobals.charName;
                            if (crit.Contains("multi attack"))
                                special = "multi";
                            AddCombatAction(attacker, victim, skillType, (int)SwingTypeEnum.NonMelee, critical, critStr, special, damageAndTypeArr, time, gts);
                        }
                        break;
                    #endregion
                    #region Case 2 [melee/non-melee attacks]
                    case 2:
                        if (reMatch.Groups[1].Value.Length > 70)
                            break;
                        attacker = reMatch.Groups[1].Value; // Contains the attacker and possibly skillType
                        crit = reMatch.Groups[2].Value;
                        victim = reMatch.Groups[3].Value;
                        critStr = reMatch.Groups[4].Value;
                        damage = reMatch.Groups[5].Value;
                        skillType = string.Empty;
                        critical = false;

                        if (damage.Contains("pain and suffering"))
                            break;
                        damageAndTypeArr = EngGetDamageAndTypeArr(damage);

                        if (crit[0] == 'c')     // Check for critical hits
                        {
                            critical = true;
                            special = crit.Substring(11);
                        }
                        else
                        {
                            special = crit;
                        }
                        if (!String.IsNullOrWhiteSpace(critStr))
                        {
                            critical = true;
                            critStr = critStr.Substring(2); // "a "
                            critStr = critStr.Substring(0, critStr.Length - 4); // " of "
                        }
                        else
                        {
                            critStr = "None";
                        }

                        special = special == "flurries" ? "flurry" : special;
                        special = special.StartsWith("aoe attack") ? "aoe" : special;
                        special = special.Contains("attack") ? special.Substring(0, special.LastIndexOf(' ')) : special;

                        victim = EnglishPersonaReplace(victim);
                        if (attacker == "YOU")  // You performing melee
                        {
                            attacker = ActGlobals.charName;
                            if (attacker == victim)
                                break;      // You don't get credit for attacking yourself or your own pet
                            if (ActGlobals.oFormActMain.SetEncounter(time, attacker, victim))
                            {
                                AddDamageAttack(attacker, victim, string.Empty, (int)SwingTypeEnum.Melee, critical, critStr, special, damageAndTypeArr, time, gts);
                            }
                            break;
                        }
                        if (attacker.StartsWith("YOUR"))        // You attacking with a skill
                        {
                            skillType = attacker.Substring(5);
                            attacker = ActGlobals.charName;
                            if (skillType == "Traumatic Swipe")
                                ActGlobals.oFormSpellTimers.ApplyTimerMod(attacker, victim, skillType, 0.5F, 30);
                            if (ActGlobals.oFormActMain.SetEncounter(time, attacker, victim))
                            {
                                AddDamageAttack(attacker, victim, skillType, (int)SwingTypeEnum.NonMelee, critical, critStr, special, damageAndTypeArr, time, gts);
                            }
                            //NotifySpell(attacker, skillType, true, victim, true);
                            break;
                        }

                        engNameSkillSplit = attacker.Split(chrApos);        // Split apart the attackerAndSkill string by apostrophes
                        if (engNameSkillSplit.Length > 1)   // If there are any apostrophes present
                        {

                            SplitAttackerSkill(ref attacker, ref skillType, engNameSkillSplit);

                            if (String.IsNullOrEmpty(skillType))    // If a skillType was not found, treat as melee
                            {
                                if (ActGlobals.oFormActMain.SetEncounter(time, attacker, victim))
                                {
                                    AddDamageAttack(attacker, victim, string.Empty, (int)SwingTypeEnum.Melee, critical, critStr, special, damageAndTypeArr, time, gts);
                                }
                            }
                            else
                            {
                                if (skillType == "Traumatic Swipe")
                                    ActGlobals.oFormSpellTimers.ApplyTimerMod(attacker, victim, skillType, 0.5F, 30);
                                if (ActGlobals.oFormActMain.SetEncounter(time, attacker, victim))
                                {
                                    AddDamageAttack(attacker, victim, skillType, (int)SwingTypeEnum.NonMelee, critical, critStr, special, damageAndTypeArr, time, gts);
                                }
                            }
                            break;
                        }
                        // If its down to here, it was a normal melee attack with no special naming tricks
                        if (attacker == victim)
                            break;      // You don't get credit for attacking yourself or your own pet
                        if (ActGlobals.oFormActMain.SetEncounter(time, attacker, victim))
                        {
                            AddDamageAttack(attacker, victim, string.Empty, (int)SwingTypeEnum.Melee, critical, critStr, special, damageAndTypeArr, time, gts);
                        }
                        break;
                    #endregion
                    #region Case 3 [healing]
                    case 3:
                        if (!ActGlobals.oFormActMain.InCombat)
                            break;
                        if (reMatch.Groups[1].Value.Length > 60)
                            break;
                        attacker = reMatch.Groups[1].Value; // Contains the healer and skillType
                        crit = reMatch.Groups[2].Value;
                        victim = reMatch.Groups[3].Value;
                        critStr = reMatch.Groups[4].Value;
                        damage = reMatch.Groups[5].Value;
                        skillType = string.Empty;
                        critical = false;

                        if (crit[0] == 'c')     // Check for critical hits
                            critical = true;

                        if (!String.IsNullOrWhiteSpace(critStr))
                        {
                            critical = true;
                            critStr = critStr.Substring(2); // "a "
                            critStr = critStr.Substring(0, critStr.Length - 4); // " of "
                        }
                        else
                        {
                            critStr = "None";
                        }
                        damage = ExpandDamageAmount(damage);

                        victim = EnglishPersonaReplace(victim);
                        if (attacker.StartsWith("YOUR"))        // You healing
                        {
                            skillType = attacker.Substring(5);
                            attacker = ActGlobals.charName;
                            MasterSwing ms = new MasterSwing((int)SwingTypeEnum.Healing, critical, "None", Int64.Parse(damage), time, gts, skillType, attacker, "Hitpoints", victim);
                            ms.Tags["CriticalStr"] = critStr;
                            ActGlobals.oFormActMain.AddCombatAction(ms);
                            break;
                        }

                        engNameSkillSplit = attacker.Split(chrApos);        // Split apart the healerAndSkill string by apostrophes
                        if (engNameSkillSplit.Length > 1)   // If there are any apostrophes present
                        {
                            SplitAttackerSkill(ref attacker, ref skillType, engNameSkillSplit);
                            MasterSwing ms = new MasterSwing((int)SwingTypeEnum.Healing, critical, "None", Int64.Parse(damage), time, gts, skillType, attacker, "Hitpoints", victim);
                            ms.Tags["CriticalStr"] = critStr;
                            ActGlobals.oFormActMain.AddCombatAction(ms);
                        }
                        break;
                    #endregion
                    #region Case 4 [misses]
                    case 4:
                        if (reMatch.Groups[1].Value.Length > 60)
                            break;
                        attacker = reMatch.Groups[1].Value;
                        attackType = reMatch.Groups[2].Value;
                        victim = reMatch.Groups[3].Value;       // Contains Victim and possibly skillType
                        why = reMatch.Groups[4].Value;
                        skillType = string.Empty;
                        special = "None";

                        attacker = EnglishPersonaReplace(attacker);
                        swingType = SwingTypeEnum.Melee;
                        int skillSplitPos = victim.IndexOf(" with ");   // If this contains "with", we know there's a skillType
                        if (skillSplitPos > -1)
                        {
                            skillType = victim.Substring(skillSplitPos + 6);
                            victim = victim.Substring(0, skillSplitPos);
                            swingType = SwingTypeEnum.NonMelee;
                        }
                        failType = GetFailTypeEnglish(victim, why, ref special);        // Get the Dnum value for the type of fail we had
                        if (victim == "YOU" || victim == "YOUR")
                            victim = ActGlobals.charName;

                        if (String.IsNullOrEmpty(skillType))
                        {
                            if (ActGlobals.oFormActMain.SetEncounter(time, attacker, victim))
                            {
                                ActGlobals.oFormActMain.AddCombatAction((int)swingType, false, special, attacker, attackType, failType, time, gts, victim, AtkToIng(attackType));
                            }
                        }
                        else
                        {
                            if (ActGlobals.oFormActMain.SetEncounter(time, attacker, victim))
                            {
                                ActGlobals.oFormActMain.AddCombatAction((int)swingType, false, special, attacker, skillType, failType, time, gts, victim, AtkToIng(attackType));
                            }
                        }
                        break;
                    #endregion
                    #region Case 5 [Twincast]
                    case 5:
                        break;
                    #endregion

                     #region Case 6 [killing]
                     case 6:
                         if (reMatch.Groups[1].Value.Length > 60)
                             break;
                         attacker = reMatch.Groups[1].Value;
                         victim = reMatch.Groups[2].Value;

                         victim = EnglishPersonaReplace(victim);

                         swingType = SwingTypeEnum.NonMelee;

                         ActGlobals.oFormSpellTimers.RemoveTimerMods(victim);
                         ActGlobals.oFormSpellTimers.DispellTimerMods(victim);
                         if (ActGlobals.oFormActMain.InCombat)
                         {
                             ActGlobals.oFormActMain.AddCombatAction((int)swingType, false, "None", attacker, "Killing", Dnum.Death, time, gts, victim, "Death");

                             //if (cbKillEnd.Checked && ActGlobals.oFormActMain.ActiveZone.ActiveEncounter.GetAllies().IndexOf(new CombatantData(attacker, null)) > -1)
                             //{
                             //    EndCombat(true);
                             //}
                         }
                         break;
                     #endregion
                     default:
                         break;*/
            }
        }

        private readonly CultureInfo enUsCulture = CultureInfo.InstalledUICulture;

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
            attacker = attacker.TrimStart(chrApos); // Remove the appostrophe at the begining of the string
            skillType = skillType.Trim();
        }

        Regex selfCheck = new Regex(@"(You|(YOU(?:(\b|R))(?:(\b|SELF))))", RegexOptions.None);

        private string EnglishPersonaReplace(string PersonaString)
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
                    catch (Exception ex)
                    {
                        lblStatus.Text = "Error loading settings: " + ex.Message;
                    }
                }
            }
        }
        SortedList<string, AposNameFix> aposNameList = new SortedList<string, AposNameFix>();
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
                    e.theAttackType =e.theDamageType.ToString();
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
                    xWriter.Flush();    // Flush the file buffer to disk
                }
            }
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

        void oFormActMain_UpdateCheckClicked()
        {
            int pluginId = 46;
            try
            {
                DateTime localDate = ActGlobals.oFormActMain.PluginGetSelfDateUtc(this);
                DateTime remoteDate = ActGlobals.oFormActMain.PluginGetRemoteDateUtc(pluginId);
                if (localDate.AddHours(2) < remoteDate)
                {
                    DialogResult result = MessageBox.Show("There is an updated version of the EQ2 English Parsing Plugin.  Update it now?\n\n(If there is an update to ACT, you should click No and update ACT first.)", "New Version", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        FileInfo updatedFile = ActGlobals.oFormActMain.PluginDownload(pluginId);
                        ActPluginData pluginData = ActGlobals.oFormActMain.PluginGetSelfData(this);
                        pluginData.pluginFile.Delete();
                        updatedFile.MoveTo(pluginData.pluginFile.FullName);
                        ThreadInvokes.CheckboxSetChecked(ActGlobals.oFormActMain, pluginData.cbEnabled, false);
                        Application.DoEvents();
                        ThreadInvokes.CheckboxSetChecked(ActGlobals.oFormActMain, pluginData.cbEnabled, true);
                    }
                }
            }
            catch (Exception ex)
            {
                ActGlobals.oFormActMain.WriteExceptionLog(ex, "Plugin Update Check");
            }
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
        private void SetupEQEnvironment()
        {
            CultureInfo usCulture = new CultureInfo("en-US");   // This is for SQL syntax; do not change

            EncounterData.ColumnDefs.Clear();
            //                                                                                      Do not change the SqlDataName while doing localization
            EncounterData.ColumnDefs.Add("EncId", new EncounterData.ColumnDef("EncId", false, "CHAR(8)", "EncId", (Data) => { return string.Empty; }, (Data) => { return Data.EncId; }));
            EncounterData.ColumnDefs.Add("Title", new EncounterData.ColumnDef("Title", true, "VARCHAR(64)", "Title", (Data) => { return Data.Title; }, (Data) => { return Data.Title; }));
            EncounterData.ColumnDefs.Add("StartTime", new EncounterData.ColumnDef("StartTime", true, "TIMESTAMP", "StartTime", (Data) => { return Data.StartTime == DateTime.MaxValue ? "--:--:--" : String.Format("{0} {1}", Data.StartTime.ToShortDateString(), Data.StartTime.ToLongTimeString()); }, (Data) => { return Data.StartTime == DateTime.MaxValue ? "0000-00-00 00:00:00" : Data.StartTime.ToString("u").TrimEnd(new char[] { 'Z' }); }));
            EncounterData.ColumnDefs.Add("EndTime", new EncounterData.ColumnDef("EndTime", true, "TIMESTAMP", "EndTime", (Data) => { return Data.EndTime == DateTime.MinValue ? "--:--:--" : Data.EndTime.ToString("T"); }, (Data) => { return Data.EndTime == DateTime.MinValue ? "0000-00-00 00:00:00" : Data.EndTime.ToString("u").TrimEnd(new char[] { 'Z' }); }));
            EncounterData.ColumnDefs.Add("Duration", new EncounterData.ColumnDef("Duration", true, "INT", "Duration", (Data) => { return Data.DurationS; }, (Data) => { return Data.Duration.TotalSeconds.ToString("0"); }));
            EncounterData.ColumnDefs.Add("Damage", new EncounterData.ColumnDef("Damage", true, "BIGINT", "Damage", (Data) => { return Data.Damage.ToString(GetIntCommas()); }, (Data) => { return Data.Damage.ToString(); }));
            EncounterData.ColumnDefs.Add("EncDPS", new EncounterData.ColumnDef("EncDPS", true, "DOUBLE", "EncDPS", (Data) => { return Data.DPS.ToString(GetFloatCommas()); }, (Data) => { return Data.DPS.ToString(usCulture); }));
            EncounterData.ColumnDefs.Add("Zone", new EncounterData.ColumnDef("Zone", false, "VARCHAR(64)", "Zone", (Data) => { return Data.ZoneName; }, (Data) => { return Data.ZoneName; }));
            EncounterData.ColumnDefs.Add("Kills", new EncounterData.ColumnDef("Kills", true, "INT", "Kills", (Data) => { return Data.AlliedKills.ToString(GetIntCommas()); }, (Data) => { return Data.AlliedKills.ToString(); }));
            EncounterData.ColumnDefs.Add("Deaths", new EncounterData.ColumnDef("Deaths", true, "INT", "Deaths", (Data) => { return Data.AlliedDeaths.ToString(); }, (Data) => { return Data.AlliedDeaths.ToString(); }));

            EncounterData.ExportVariables.Clear();
            EncounterData.ExportVariables.Add("n", new EncounterData.TextExportFormatter("n", "New Line", "Formatting after this element will appear on a new line.", (Data, SelectiveAllies, Extra) => { return "\n"; }));
            EncounterData.ExportVariables.Add("t", new EncounterData.TextExportFormatter("t", "Tab Character", "Formatting after this element will appear in a relative column arrangement.  (The formatting example cannot display this properly)", (Data, SelectiveAllies, Extra) => { return "\t"; }));

            CombatantData.ColumnDefs.Clear();
            CombatantData.ColumnDefs.Add("EncId", new CombatantData.ColumnDef("EncId", false, "CHAR(8)", "EncId", (Data) => { return string.Empty; }, (Data) => { return Data.Parent.EncId; }, (Left, Right) => { return 0; }));
            CombatantData.ColumnDefs.Add("Ally", new CombatantData.ColumnDef("Ally", false, "CHAR(1)", "Ally", (Data) => { return Data.Parent.GetAllies().Contains(Data).ToString(); }, (Data) => { return Data.Parent.GetAllies().Contains(Data) ? "T" : "F"; }, (Left, Right) => { return Left.Parent.GetAllies().Contains(Left).CompareTo(Right.Parent.GetAllies().Contains(Right)); }));
            CombatantData.ColumnDefs.Add("Name", new CombatantData.ColumnDef("Name", true, "VARCHAR(64)", "Name", (Data) => { return Data.Name; }, (Data) => { return Data.Name; }, (Left, Right) => { return Left.Name.CompareTo(Right.Name); }));
            CombatantData.ColumnDefs.Add("StartTime", new CombatantData.ColumnDef("StartTime", true, "TIMESTAMP", "StartTime", (Data) => { return Data.StartTime == DateTime.MaxValue ? "--:--:--" : Data.StartTime.ToString("T"); }, (Data) => { return Data.StartTime == DateTime.MaxValue ? "0000-00-00 00:00:00" : Data.StartTime.ToString("u").TrimEnd(new char[] { 'Z' }); }, (Left, Right) => { return Left.StartTime.CompareTo(Right.StartTime); }));
            CombatantData.ColumnDefs.Add("EndTime", new CombatantData.ColumnDef("EndTime", false, "TIMESTAMP", "EndTime", (Data) => { return Data.EndTime == DateTime.MinValue ? "--:--:--" : Data.EndTime.ToString("T"); }, (Data) => { return Data.EndTime == DateTime.MinValue ? "0000-00-00 00:00:00" : Data.EndTime.ToString("u").TrimEnd(new char[] { 'Z' }); }, (Left, Right) => { return Left.EndTime.CompareTo(Right.EndTime); }));
            CombatantData.ColumnDefs.Add("Duration", new CombatantData.ColumnDef("Duration", true, "INT", "Duration", (Data) => { return Data.DurationS; }, (Data) => { return Data.Duration.TotalSeconds.ToString("0"); }, (Left, Right) => { return Left.Duration.CompareTo(Right.Duration); }));
            CombatantData.ColumnDefs.Add("Damage", new CombatantData.ColumnDef("Damage", true, "BIGINT", "Damage", (Data) => { return Data.Damage.ToString(GetIntCommas()); }, (Data) => { return Data.Damage.ToString(); }, (Left, Right) => { return Left.Damage.CompareTo(Right.Damage); }));
            CombatantData.ColumnDefs.Add("Damage%", new CombatantData.ColumnDef("Damage%", true, "VARCHAR(4)", "DamagePerc", (Data) => { return Data.DamagePercent; }, (Data) => { return Data.DamagePercent; }, (Left, Right) => { return Left.Damage.CompareTo(Right.Damage); }));
            CombatantData.ColumnDefs.Add("Kills", new CombatantData.ColumnDef("Kills", false, "INT", "Kills", (Data) => { return Data.Kills.ToString(GetIntCommas()); }, (Data) => { return Data.Kills.ToString(); }, (Left, Right) => { return Left.Kills.CompareTo(Right.Kills); }));
            CombatantData.ColumnDefs.Add("Healed", new CombatantData.ColumnDef("Healed", false, "BIGINT", "Healed", (Data) => { return Data.Healed.ToString(GetIntCommas()); }, (Data) => { return Data.Healed.ToString(); }, (Left, Right) => { return Left.Healed.CompareTo(Right.Healed); }));
            CombatantData.ColumnDefs.Add("Healed%", new CombatantData.ColumnDef("Healed%", false, "VARCHAR(4)", "HealedPerc", (Data) => { return Data.HealedPercent; }, (Data) => { return Data.HealedPercent; }, (Left, Right) => { return Left.Healed.CompareTo(Right.Healed); }));
            CombatantData.ColumnDefs.Add("CritHeals", new CombatantData.ColumnDef("CritHeals", false, "INT", "CritHeals", (Data) => { return Data.CritHeals.ToString(GetIntCommas()); }, (Data) => { return Data.CritHeals.ToString(); }, (Left, Right) => { return Left.CritHeals.CompareTo(Right.CritHeals); }));
            CombatantData.ColumnDefs.Add("Heals", new CombatantData.ColumnDef("Heals", false, "INT", "Heals", (Data) => { return Data.Heals.ToString(GetIntCommas()); }, (Data) => { return Data.Heals.ToString(); }, (Left, Right) => { return Left.Heals.CompareTo(Right.Heals); }));
            CombatantData.ColumnDefs.Add("Cures", new CombatantData.ColumnDef("Cures", false, "INT", "CureDispels", (Data) => { return Data.CureDispels.ToString(GetIntCommas()); }, (Data) => { return Data.CureDispels.ToString(); }, (Left, Right) => { return Left.CureDispels.CompareTo(Right.CureDispels); }));
            CombatantData.ColumnDefs.Add("PowerDrain", new CombatantData.ColumnDef("PowerDrain", true, "BIGINT", "PowerDrain", (Data) => { return Data.PowerDamage.ToString(GetIntCommas()); }, (Data) => { return Data.PowerDamage.ToString(); }, (Left, Right) => { return Left.PowerDamage.CompareTo(Right.PowerDamage); }));
            CombatantData.ColumnDefs.Add("PowerReplenish", new CombatantData.ColumnDef("PowerReplenish", false, "BIGINT", "PowerReplenish", (Data) => { return Data.PowerReplenish.ToString(GetIntCommas()); }, (Data) => { return Data.PowerReplenish.ToString(); }, (Left, Right) => { return Left.PowerReplenish.CompareTo(Right.PowerReplenish); }));
            CombatantData.ColumnDefs.Add("DPS", new CombatantData.ColumnDef("DPS", false, "DOUBLE", "DPS", (Data) => { return Data.DPS.ToString(GetFloatCommas()); }, (Data) => { return Data.DPS.ToString(usCulture); }, (Left, Right) => { return Left.DPS.CompareTo(Right.DPS); }));
            CombatantData.ColumnDefs.Add("EncDPS", new CombatantData.ColumnDef("EncDPS", true, "DOUBLE", "EncDPS", (Data) => { return Data.EncDPS.ToString(GetFloatCommas()); }, (Data) => { return Data.EncDPS.ToString(usCulture); }, (Left, Right) => { return Left.Damage.CompareTo(Right.Damage); }));
            CombatantData.ColumnDefs.Add("EncHPS", new CombatantData.ColumnDef("EncHPS", true, "DOUBLE", "EncHPS", (Data) => { return Data.EncHPS.ToString(GetFloatCommas()); }, (Data) => { return Data.EncHPS.ToString(usCulture); }, (Left, Right) => { return Left.Healed.CompareTo(Right.Healed); }));
            CombatantData.ColumnDefs.Add("Hits", new CombatantData.ColumnDef("Hits", false, "INT", "Hits", (Data) => { return Data.Hits.ToString(GetIntCommas()); }, (Data) => { return Data.Hits.ToString(); }, (Left, Right) => { return Left.Hits.CompareTo(Right.Hits); }));
            CombatantData.ColumnDefs.Add("CritHits", new CombatantData.ColumnDef("CritHits", false, "INT", "CritHits", (Data) => { return Data.CritHits.ToString(GetIntCommas()); }, (Data) => { return Data.CritHits.ToString(); }, (Left, Right) => { return Left.CritHits.CompareTo(Right.CritHits); }));
            CombatantData.ColumnDefs.Add("Avoids", new CombatantData.ColumnDef("Avoids", false, "INT", "Blocked", (Data) => { return Data.Blocked.ToString(GetIntCommas()); }, (Data) => { return Data.Blocked.ToString(); }, (Left, Right) => { return Left.Blocked.CompareTo(Right.Blocked); }));
            CombatantData.ColumnDefs.Add("Misses", new CombatantData.ColumnDef("Misses", false, "INT", "Misses", (Data) => { return Data.Misses.ToString(GetIntCommas()); }, (Data) => { return Data.Misses.ToString(); }, (Left, Right) => { return Left.Misses.CompareTo(Right.Misses); }));
            CombatantData.ColumnDefs.Add("Swings", new CombatantData.ColumnDef("Swings", false, "INT", "Swings", (Data) => { return Data.Swings.ToString(GetIntCommas()); }, (Data) => { return Data.Swings.ToString(); }, (Left, Right) => { return Left.Swings.CompareTo(Right.Swings); }));
            CombatantData.ColumnDefs.Add("HealingTaken", new CombatantData.ColumnDef("HealingTaken", false, "BIGINT", "HealsTaken", (Data) => { return Data.HealsTaken.ToString(GetIntCommas()); }, (Data) => { return Data.HealsTaken.ToString(); }, (Left, Right) => { return Left.HealsTaken.CompareTo(Right.HealsTaken); }));
            CombatantData.ColumnDefs.Add("DamageTaken", new CombatantData.ColumnDef("DamageTaken", true, "BIGINT", "DamageTaken", (Data) => { return Data.DamageTaken.ToString(GetIntCommas()); }, (Data) => { return Data.DamageTaken.ToString(); }, (Left, Right) => { return Left.DamageTaken.CompareTo(Right.DamageTaken); }));
            CombatantData.ColumnDefs.Add("Deaths", new CombatantData.ColumnDef("Deaths", true, "INT", "Deaths", (Data) => { return Data.Deaths.ToString(GetIntCommas()); }, (Data) => { return Data.Deaths.ToString(); }, (Left, Right) => { return Left.Deaths.CompareTo(Right.Deaths); }));
            CombatantData.ColumnDefs.Add("ToHit%", new CombatantData.ColumnDef("ToHit%", false, "FLOAT", "ToHit", (Data) => { return Data.ToHit.ToString(GetFloatCommas()); }, (Data) => { return Data.ToHit.ToString(usCulture); }, (Left, Right) => { return Left.ToHit.CompareTo(Right.ToHit); }));
            CombatantData.ColumnDefs.Add("CritDam%", new CombatantData.ColumnDef("CritDam%", false, "VARCHAR(8)", "CritDamPerc", (Data) => { return Data.CritDamPerc.ToString("0'%"); }, (Data) => { return Data.CritDamPerc.ToString("0'%"); }, (Left, Right) => { return Left.CritDamPerc.CompareTo(Right.CritDamPerc); }));
            CombatantData.ColumnDefs.Add("CritHeal%", new CombatantData.ColumnDef("CritHeal%", false, "VARCHAR(8)", "CritHealPerc", (Data) => { return Data.CritHealPerc.ToString("0'%"); }, (Data) => { return Data.CritHealPerc.ToString("0'%"); }, (Left, Right) => { return Left.CritHealPerc.CompareTo(Right.CritHealPerc); }));

            CombatantData.ColumnDefs.Add("CritTypes", new CombatantData.ColumnDef("CritTypes", true, "VARCHAR(32)", "CritTypes", CombatantDataGetCritTypes, CombatantDataGetCritTypes, (Left, Right) => { return CombatantDataGetCritTypes(Left).CompareTo(CombatantDataGetCritTypes(Right)); }));

            CombatantData.ColumnDefs.Add("Threat +/-", new CombatantData.ColumnDef("Threat +/-", false, "VARCHAR(32)", "ThreatStr", (Data) => { return Data.GetThreatStr("Threat (Out)"); }, (Data) => { return Data.GetThreatStr("Threat (Out)"); }, (Left, Right) => { return Left.GetThreatDelta("Threat (Out)").CompareTo(Right.GetThreatDelta("Threat (Out)")); }));
            CombatantData.ColumnDefs.Add("ThreatDelta", new CombatantData.ColumnDef("ThreatDelta", false, "BIGINT", "ThreatDelta", (Data) => { return Data.GetThreatDelta("Threat (Out)").ToString(GetIntCommas()); }, (Data) => { return Data.GetThreatDelta("Threat (Out)").ToString(); }, (Left, Right) => { return Left.GetThreatDelta("Threat (Out)").CompareTo(Right.GetThreatDelta("Threat (Out)")); }));

            CombatantData.ColumnDefs["Damage"].GetCellForeColor = (Data) => { return Color.DarkRed; };
            CombatantData.ColumnDefs["Damage%"].GetCellForeColor = (Data) => { return Color.DarkRed; };
            CombatantData.ColumnDefs["Healed"].GetCellForeColor = (Data) => { return Color.DarkBlue; };
            CombatantData.ColumnDefs["Healed%"].GetCellForeColor = (Data) => { return Color.DarkBlue; };
            CombatantData.ColumnDefs["PowerDrain"].GetCellForeColor = (Data) => { return Color.DarkMagenta; };
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
            {"Power Drain (Out)", new CombatantData.DamageTypeDef("Power Drain (Out)", -1, Color.Purple)},
            {"Power Replenish (Out)", new CombatantData.DamageTypeDef("Power Replenish (Out)", 1, Color.Violet)},
            {"Cure/Dispel (Out)", new CombatantData.DamageTypeDef("Cure/Dispel (Out)", 0, Color.Wheat)},
            {"Threat (Out)", new CombatantData.DamageTypeDef("Threat (Out)", -1, Color.Yellow)},
            {"All Outgoing (Ref)", new CombatantData.DamageTypeDef("All Outgoing (Ref)", 0, Color.Black)}
        };
            CombatantData.IncomingDamageTypeDataObjects = new Dictionary<string, CombatantData.DamageTypeDef>
        {
            {"Incoming Damage", new CombatantData.DamageTypeDef("Incoming Damage", -1, Color.Red)},
            {"Healed (Inc)",new CombatantData.DamageTypeDef("Healed (Inc)", 1, Color.LimeGreen)},
            {"Power Drain (Inc)",new CombatantData.DamageTypeDef("Power Drain (Inc)", -1, Color.Magenta)},
            {"Power Replenish (Inc)",new CombatantData.DamageTypeDef("Power Replenish (Inc)", 1, Color.MediumPurple)},
            {"Cure/Dispel (Inc)", new CombatantData.DamageTypeDef("Cure/Dispel (Inc)", 0, Color.Wheat)},
            {"Threat (Inc)",new CombatantData.DamageTypeDef("Threat (Inc)", -1, Color.Yellow)},
            {"All Incoming (Ref)",new CombatantData.DamageTypeDef("All Incoming (Ref)", 0, Color.Black)}
        };
            CombatantData.SwingTypeToDamageTypeDataLinksOutgoing = new SortedDictionary<int, List<string>>
        {
            {1, new List<string> { "Auto-Attack (Out)", "Outgoing Damage" } },
            {2, new List<string> { "Skill/Ability (Out)", "Outgoing Damage" } },
            {3, new List<string> { "Healed (Out)" } },
            {10, new List<string> { "Power Drain (Out)" } },
            {13, new List<string> { "Power Replenish (Out)" } },
            {20, new List<string> { "Cure/Dispel (Out)" } },
            {16, new List<string> { "Threat (Out)" } }
        };
            CombatantData.SwingTypeToDamageTypeDataLinksIncoming = new SortedDictionary<int, List<string>>
        {
            {1, new List<string> { "Incoming Damage" } },
            {2, new List<string> { "Incoming Damage" } },
            {3, new List<string> { "Healed (Inc)" } },
            {10, new List<string> { "Power Drain (Inc)" } },
            {13, new List<string> { "Power Replenish (Inc)" } },
            {20, new List<string> { "Cure/Dispel (Inc)" } },
            {16, new List<string> { "Threat (Inc)" } }
        };

            CombatantData.DamageSwingTypes = new List<int> { 1, 2 };
            CombatantData.HealingSwingTypes = new List<int> { 3 };

            CombatantData.DamageTypeDataNonSkillDamage = "Auto-Attack (Out)";
            CombatantData.DamageTypeDataOutgoingDamage = "Outgoing Damage";
            CombatantData.DamageTypeDataOutgoingHealing = "Healed (Out)";
            CombatantData.DamageTypeDataIncomingDamage = "Incoming Damage";
            CombatantData.DamageTypeDataIncomingHealing = "Healed (Inc)";
            CombatantData.DamageTypeDataOutgoingPowerReplenish = "Power Replenish (Out)";
            CombatantData.DamageTypeDataOutgoingPowerDamage = "Power Drain (Out)";
            CombatantData.DamageTypeDataOutgoingCures = "Cure/Dispel (Out)";

            CombatantData.ExportVariables.Clear();
            CombatantData.ExportVariables.Add("n", new CombatantData.TextExportFormatter("n", "New Line", "Formatting after this element will appear on a new line.", (Data, Extra) => { return "\n"; }));
            CombatantData.ExportVariables.Add("t", new CombatantData.TextExportFormatter("t", "Tab Character", "Formatting after this element will appear in a relative column arrangement.  (The formatting example cannot display this properly)", (Data, Extra) => { return "\t"; }));
           
            DamageTypeData.ColumnDefs.Clear();
            DamageTypeData.ColumnDefs.Add("EncId", new DamageTypeData.ColumnDef("EncId", false, "CHAR(8)", "EncId", (Data) => { return string.Empty; }, (Data) => { return Data.Parent.Parent.EncId; }));
            DamageTypeData.ColumnDefs.Add("Combatant", new DamageTypeData.ColumnDef("Combatant", false, "VARCHAR(64)", "Combatant", (Data) => { return Data.Parent.Name; }, (Data) => { return Data.Parent.Name; }));
            DamageTypeData.ColumnDefs.Add("Grouping", new DamageTypeData.ColumnDef("Grouping", false, "VARCHAR(92)", "Grouping", (Data) => { return string.Empty; }, GetDamageTypeGrouping));
            DamageTypeData.ColumnDefs.Add("Type", new DamageTypeData.ColumnDef("Type", true, "VARCHAR(64)", "Type", (Data) => { return Data.Type; }, (Data) => { return Data.Type; }));
            DamageTypeData.ColumnDefs.Add("StartTime", new DamageTypeData.ColumnDef("StartTime", false, "TIMESTAMP", "StartTime", (Data) => { return Data.StartTime == DateTime.MaxValue ? "--:--:--" : Data.StartTime.ToString("T"); }, (Data) => { return Data.StartTime == DateTime.MaxValue ? "0000-00-00 00:00:00" : Data.StartTime.ToString("u").TrimEnd(new char[] { 'Z' }); }));
            DamageTypeData.ColumnDefs.Add("EndTime", new DamageTypeData.ColumnDef("EndTime", false, "TIMESTAMP", "EndTime", (Data) => { return Data.EndTime == DateTime.MinValue ? "--:--:--" : Data.EndTime.ToString("T"); }, (Data) => { return Data.EndTime == DateTime.MinValue ? "0000-00-00 00:00:00" : Data.EndTime.ToString("u").TrimEnd(new char[] { 'Z' }); }));
            DamageTypeData.ColumnDefs.Add("Duration", new DamageTypeData.ColumnDef("Duration", false, "INT", "Duration", (Data) => { return Data.DurationS; }, (Data) => { return Data.Duration.TotalSeconds.ToString("0"); }));
            DamageTypeData.ColumnDefs.Add("Damage", new DamageTypeData.ColumnDef("Damage", true, "BIGINT", "Damage", (Data) => { return Data.Damage.ToString(GetIntCommas()); }, (Data) => { return Data.Damage.ToString(); }));
            DamageTypeData.ColumnDefs.Add("EncDPS", new DamageTypeData.ColumnDef("EncDPS", true, "DOUBLE", "EncDPS", (Data) => { return Data.EncDPS.ToString(GetFloatCommas()); }, (Data) => { return Data.EncDPS.ToString(usCulture); }));
            DamageTypeData.ColumnDefs.Add("CharDPS", new DamageTypeData.ColumnDef("CharDPS", false, "DOUBLE", "CharDPS", (Data) => { return Data.CharDPS.ToString(GetFloatCommas()); }, (Data) => { return Data.CharDPS.ToString(usCulture); }));
            DamageTypeData.ColumnDefs.Add("DPS", new DamageTypeData.ColumnDef("DPS", false, "DOUBLE", "DPS", (Data) => { return Data.DPS.ToString(GetFloatCommas()); }, (Data) => { return Data.DPS.ToString(usCulture); }));
            DamageTypeData.ColumnDefs.Add("Average", new DamageTypeData.ColumnDef("Average", true, "DOUBLE", "Average", (Data) => { return Data.Average.ToString(GetFloatCommas()); }, (Data) => { return Data.Average.ToString(usCulture); }));
            DamageTypeData.ColumnDefs.Add("Median", new DamageTypeData.ColumnDef("Median", false, "BIGINT", "Median", (Data) => { return Data.Median.ToString(GetIntCommas()); }, (Data) => { return Data.Median.ToString(); }));
            DamageTypeData.ColumnDefs.Add("MinHit", new DamageTypeData.ColumnDef("MinHit", true, "BIGINT", "MinHit", (Data) => { return Data.MinHit.ToString(GetIntCommas()); }, (Data) => { return Data.MinHit.ToString(); }));
            DamageTypeData.ColumnDefs.Add("MaxHit", new DamageTypeData.ColumnDef("MaxHit", true, "BIGINT", "MaxHit", (Data) => { return Data.MaxHit.ToString(GetIntCommas()); }, (Data) => { return Data.MaxHit.ToString(); }));
            DamageTypeData.ColumnDefs.Add("Hits", new DamageTypeData.ColumnDef("Hits", true, "INT", "Hits", (Data) => { return Data.Hits.ToString(GetIntCommas()); }, (Data) => { return Data.Hits.ToString(); }));
            DamageTypeData.ColumnDefs.Add("CritHits", new DamageTypeData.ColumnDef("CritHits", false, "INT", "CritHits", (Data) => { return Data.CritHits.ToString(GetIntCommas()); }, (Data) => { return Data.CritHits.ToString(); }));
            DamageTypeData.ColumnDefs.Add("Avoids", new DamageTypeData.ColumnDef("Avoids", false, "INT", "Blocked", (Data) => { return Data.Blocked.ToString(GetIntCommas()); }, (Data) => { return Data.Blocked.ToString(); }));
            DamageTypeData.ColumnDefs.Add("Misses", new DamageTypeData.ColumnDef("Misses", false, "INT", "Misses", (Data) => { return Data.Misses.ToString(GetIntCommas()); }, (Data) => { return Data.Misses.ToString(); }));
            DamageTypeData.ColumnDefs.Add("Swings", new DamageTypeData.ColumnDef("Swings", true, "INT", "Swings", (Data) => { return Data.Swings.ToString(GetIntCommas()); }, (Data) => { return Data.Swings.ToString(); }));
            DamageTypeData.ColumnDefs.Add("ToHit", new DamageTypeData.ColumnDef("ToHit", false, "FLOAT", "ToHit", (Data) => { return Data.ToHit.ToString(GetFloatCommas()); }, (Data) => { return Data.ToHit.ToString(); }));
            DamageTypeData.ColumnDefs.Add("AvgDelay", new DamageTypeData.ColumnDef("AvgDelay", false, "FLOAT", "AverageDelay", (Data) => { return Data.AverageDelay.ToString(GetFloatCommas()); }, (Data) => { return Data.AverageDelay.ToString(usCulture); }));
            DamageTypeData.ColumnDefs.Add("Crit%", new DamageTypeData.ColumnDef("Crit%", false, "VARCHAR(8)", "CritPerc", (Data) => { return Data.CritPerc.ToString("0'%"); }, (Data) => { return Data.CritPerc.ToString("0'%"); }));
            DamageTypeData.ColumnDefs.Add("CritTypes", new DamageTypeData.ColumnDef("CritTypes", true, "VARCHAR(32)", "CritTypes", DamageTypeDataGetCritTypes, DamageTypeDataGetCritTypes));


            AttackType.ColumnDefs.Clear();
            AttackType.ColumnDefs.Add("EncId", new AttackType.ColumnDef("EncId", false, "CHAR(8)", "EncId", (Data) => { return string.Empty; }, (Data) => { return Data.Parent.Parent.Parent.EncId; }, (Left, Right) => { return 0; }));
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
            AttackType.ColumnDefs.Add("Crit%", new AttackType.ColumnDef("Crit%", true, "VARCHAR(8)", "CritPerc", (Data) => { return Data.CritPerc.ToString("0'%"); }, (Data) => { return Data.CritPerc.ToString("0'%"); }, (Left, Right) => { return Left.CritPerc.CompareTo(Right.CritPerc); }));
            AttackType.ColumnDefs.Add("CritTypes", new AttackType.ColumnDef("CritTypes", true, "VARCHAR(32)", "CritTypes", AttackTypeGetCritTypes, AttackTypeGetCritTypes, (Left, Right) => { return AttackTypeGetCritTypes(Left).CompareTo(AttackTypeGetCritTypes(Right)); }));


            MasterSwing.ColumnDefs.Clear();
            MasterSwing.ColumnDefs.Add("EncId", new MasterSwing.ColumnDef("EncId", false, "CHAR(8)", "EncId", (Data) => { return string.Empty; }, (Data) => { return Data.ParentEncounter.EncId; }, (Left, Right) => { return 0; }));
            MasterSwing.ColumnDefs.Add("Time", new MasterSwing.ColumnDef("Time", true, "TIMESTAMP", "STime", (Data) => { return Data.Time.ToString("T"); }, (Data) => { return Data.Time.ToString("u").TrimEnd(new char[] { 'Z' }); }, (Left, Right) => { return Left.Time.CompareTo(Right.Time); }));
            MasterSwing.ColumnDefs.Add("RelativeTime", new MasterSwing.ColumnDef("RelativeTime", true, "FLOAT", "RelativeTime", (Data) => { return Data.ParentEncounter != null ? (Data.Time - Data.ParentEncounter.StartTime).ToString("g") : String.Empty; }, (Data) => { return Data.ParentEncounter != null ? (Data.Time - Data.ParentEncounter.StartTime).TotalSeconds.ToString() : String.Empty; }, (Left, Right) => { return Left.Time.CompareTo(Right.Time); }));
            MasterSwing.ColumnDefs.Add("Attacker", new MasterSwing.ColumnDef("Attacker", true, "VARCHAR(64)", "Attacker", (Data) => { return Data.Attacker; }, (Data) => { return Data.Attacker; }, (Left, Right) => { return Left.Attacker.CompareTo(Right.Attacker); }));
            MasterSwing.ColumnDefs.Add("SwingType", new MasterSwing.ColumnDef("SwingType", false, "TINYINT", "SwingType", (Data) => { return Data.SwingType.ToString(); }, (Data) => { return Data.SwingType.ToString(); }, (Left, Right) => { return Left.SwingType.CompareTo(Right.SwingType); }));
            MasterSwing.ColumnDefs.Add("AttackType", new MasterSwing.ColumnDef("AttackType", true, "VARCHAR(64)", "AttackType", (Data) => { return Data.AttackType; }, (Data) => { return Data.AttackType; }, (Left, Right) => { return Left.AttackType.CompareTo(Right.AttackType); }));
            MasterSwing.ColumnDefs.Add("DamageType", new MasterSwing.ColumnDef("DamageType", true, "VARCHAR(64)", "DamageType", (Data) => { return Data.DamageType; }, (Data) => { return Data.DamageType; }, (Left, Right) => { return Left.DamageType.CompareTo(Right.DamageType); }));
            MasterSwing.ColumnDefs.Add("Victim", new MasterSwing.ColumnDef("Victim", true, "VARCHAR(64)", "Victim", (Data) => { return Data.Victim; }, (Data) => { return Data.Victim; }, (Left, Right) => { return Left.Victim.CompareTo(Right.Victim); }));
            MasterSwing.ColumnDefs.Add("DamageNum", new MasterSwing.ColumnDef("DamageNum", false, "BIGINT", "Damage", (Data) => { return ((long)Data.Damage).ToString(); }, (Data) => { return ((long)Data.Damage).ToString(); }, (Left, Right) => { return Left.Damage.CompareTo(Right.Damage); }));
            MasterSwing.ColumnDefs.Add("Damage", new MasterSwing.ColumnDef("Damage", true, "VARCHAR(128)", "DamageString", (Data) => { return Data.Damage.ToString(); }, (Data) => { return Data.Damage.ToString(); }, (Left, Right) => { return Left.Damage.CompareTo(Right.Damage); }));
            MasterSwing.ColumnDefs.Add("Critical", new MasterSwing.ColumnDef("Critical", false, "CHAR(1)", "Critical", (Data) => { return Data.Critical.ToString(); }, (Data) => { return Data.Critical.ToString(usCulture)[0].ToString(); }, (Left, Right) => { return Left.Critical.CompareTo(Right.Critical); }));
            MasterSwing.ColumnDefs.Add("Twinproc", new MasterSwing.ColumnDef("Twinproc", true, "BOOLEAN", "Twinproc", (Data) => { return Data.Tags["Twinproc"]; }, (Data) => { return Data.Tags["Twinproc"]; }, (Left, Right) => { return Left.Tags["Twinproc"].CompareTo(Right.Tags["Twinproc"]); }));
            MasterSwing.ColumnDefs.Add("CriticalStr", new MasterSwing.ColumnDef("CriticalStr", true, "VARCHAR(32)", "CriticalStr", (Data) =>
            {
                if (Data.Tags.ContainsKey("CriticalStr"))
                    return (string)Data.Tags["CriticalStr"];
                else
                    return "None";
            }, (Data) =>
            {
                if (Data.Tags.ContainsKey("CriticalStr"))
                    return (string)Data.Tags["CriticalStr"];
                else
                    return "None";
            }, (Left, Right) =>
            {
                string left = Left.Tags.ContainsKey("CriticalStr") ? (string)Left.Tags["CriticalStr"] : "None";
                string right = Right.Tags.ContainsKey("CriticalStr") ? (string)Right.Tags["CriticalStr"] : "None";
                return left.CompareTo(right);
            }));
            MasterSwing.ColumnDefs.Add("Special", new MasterSwing.ColumnDef("Special", true, "VARCHAR(64)", "Special", (Data) => { return Data.Special; }, (Data) => { return Data.Special; }, (Left, Right) => { return Left.Special.CompareTo(Right.Special); }));

            foreach (KeyValuePair<string, MasterSwing.ColumnDef> pair in MasterSwing.ColumnDefs)
                pair.Value.GetCellForeColor = (Data) => { return GetSwingTypeColor(Data.SwingType); };

            ActGlobals.oFormActMain.ValidateLists();
            ActGlobals.oFormActMain.ValidateTableSetup();
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
        private string AttackTypeGetCritTypes(AttackType Data)
        {
            int crit = 0;
            int lCrit = 0;
            int fCrit = 0;
            int mCrit = 0;
            for (int i = 0; i < Data.Items.Count; i++)
            {
                MasterSwing ms = Data.Items[i];
                if (ms.Critical)
                {
                    crit++;
                    if (!ms.Tags.ContainsKey("CriticalStr"))
                        continue;
                    if (((string)ms.Tags["CriticalStr"]).Contains("Legendary"))
                    {
                        lCrit++;
                        continue;
                    }
                    if (((string)ms.Tags["CriticalStr"]).Contains("Fabled"))
                    {
                        fCrit++;
                        continue;
                    }
                    if (((string)ms.Tags["CriticalStr"]).Contains("Mythical"))
                    {
                        mCrit++;
                        continue;
                    }
                }
            }
            float lCritPerc = ((float)lCrit / (float)crit) * 100f;
            float fCritPerc = ((float)fCrit / (float)crit) * 100f;
            float mCritPerc = ((float)mCrit / (float)crit) * 100f;
            if (crit == 0)
                return "-";
            return String.Format("{0:0.0}%L - {1:0.0}%F - {2:0.0}%M", lCritPerc, fCritPerc, mCritPerc);
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

        private string CombatantDataGetCritTypes(CombatantData Data)
        {
            AttackType at;
            if (Data.AllOut.TryGetValue(ActGlobals.ActLocalization.LocalizationStrings["attackTypeTerm-all"].DisplayedText, out at))
            {
                return AttackTypeGetCritTypes(at);
            }
            else
                return "-";
        }
        private string DamageTypeDataGetCritTypes(DamageTypeData Data)
        {
            AttackType at;
            if (Data.Items.TryGetValue(ActGlobals.ActLocalization.LocalizationStrings["attackTypeTerm-all"].DisplayedText, out at))
            {
                return AttackTypeGetCritTypes(at);
            }
            else
                return "-";
        }
    }
}
