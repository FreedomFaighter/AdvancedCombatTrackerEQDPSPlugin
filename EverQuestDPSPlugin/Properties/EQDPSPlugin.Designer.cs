﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EverQuestDPS.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class EQDPSPlugin {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal EQDPSPlugin() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("EverQuestDPS.Properties.EQDPSPlugin", typeof(EQDPSPlugin).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to backstab|throw|pierce|gore|crush|slash|hit|kick|slam|bash|shoot|strike|bite|grab|punch|scratch|rake|swipe|claw|maul|smash|frenzies on|frenzy.
        /// </summary>
        internal static string attackTypes {
            get {
                return ResourceManager.GetString("attackTypes", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to (?&lt;attacker&gt;.+) hit(|s) (?&lt;victim&gt;.+) for (?&lt;baneDamage&gt;[\d]+) points of (?&lt;typeOfDamage&gt;.+) by Banestrike (?&lt;baneAbilityRank&gt;.+\.)(?:\s\((?&lt;baneSpecial&gt;.+)\)){0,1}.
        /// </summary>
        internal static string Banestrike {
            get {
                return ResourceManager.GetString("Banestrike", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to (?&lt;victim&gt;.+) (was|is) chilled to the bone for (?&lt;damageAmount&gt;[\d]+) point(|s) of non-melee damage.(?:[\s][\(](?&lt;damageShieldSpecial&gt;.+)[\)]){0,1}.
        /// </summary>
        internal static string chilledDamageShield {
            get {
                return ResourceManager.GetString("chilledDamageShield", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Crippling Blow.
        /// </summary>
        internal static string CripplingBlow {
            get {
                return ResourceManager.GetString("CripplingBlow", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Critical.
        /// </summary>
        internal static string Critical {
            get {
                return ResourceManager.GetString("Critical", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to (?&lt;victim&gt;.+) is (?&lt;damageShieldDamageType&gt;\S+) by (?&lt;attacker&gt;.+)(&apos;s) (?&lt;damageShieldType&gt;\S+) for (?&lt;damagePoints&gt;[\d]+) points of non-melee damage..
        /// </summary>
        internal static string DamageShield {
            get {
                return ResourceManager.GetString("DamageShield", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to (?&lt;victim&gt;.+) was (?&lt;damageShieldResponse&gt;.+) for (?&lt;damagePoints&gt;[\d]+) point(|s) of non-melee damage..
        /// </summary>
        internal static string DamageShieldUnknownOrigin {
            get {
                return ResourceManager.GetString("DamageShieldUnknownOrigin", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to damageSpecial.
        /// </summary>
        internal static string DamageSpecial {
            get {
                return ResourceManager.GetString("DamageSpecial", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to dateTimeOfLogLine.
        /// </summary>
        internal static string dateTimeOfLogLine {
            get {
                return ResourceManager.GetString("dateTimeOfLogLine", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Double Bow Shot.
        /// </summary>
        internal static string DoubleBowShot {
            get {
                return ResourceManager.GetString("DoubleBowShot", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ddd MMM dd HH:mm:ss yyyy.
        /// </summary>
        internal static string eqDateTimeStampFormat {
            get {
                return ResourceManager.GetString("eqDateTimeStampFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to block(|s)|dodge(|s)|parr(ies|y)|riposte(|s).
        /// </summary>
        internal static string evasionTypes {
            get {
                return ResourceManager.GetString("evasionTypes", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to (?:.+)[\\]eqlog_(?&lt;characterName&gt;\S+)_(?&lt;server&gt;.+).txt.
        /// </summary>
        internal static string fileNameForLog {
            get {
                return ResourceManager.GetString("fileNameForLog", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Finishing Blow.
        /// </summary>
        internal static string FinishingBlow {
            get {
                return ResourceManager.GetString("FinishingBlow", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Flurry.
        /// </summary>
        internal static string Flurry {
            get {
                return ResourceManager.GetString("Flurry", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to (?&lt;victim&gt;.+) has taken (?&lt;damagePoints&gt;[\d]+) damage from (?&lt;attacker&gt;(your)+) (?&lt;damageEffect&gt;.[^.]+)\.(?:[\s][\(](?&lt;focusSpecial&gt;.+)[\)]){0,1}.
        /// </summary>
        internal static string FocusDamageEffect {
            get {
                return ResourceManager.GetString("FocusDamageEffect", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to (?&lt;attacker&gt;.+?) (?:has been\s){0,1}healed (?&lt;victim&gt;^[over time]|.+?)(?:\s(?&lt;overTime&gt;over time)){0,1} for (?&lt;pointsOfHealing&gt;[\d]+)(?:\s\((?&lt;pointsOfDamage&gt;[\d]+)\)){0,1} hit point(|s)(?:\sby(?&lt;healingSpellName&gt;.+)){0,1}\.(?:[\s][\(](?&lt;damageSpecial&gt;.+)[\)]){0,1}.
        /// </summary>
        internal static string Heal {
            get {
                return ResourceManager.GetString("Heal", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Locked.
        /// </summary>
        internal static string Locked {
            get {
                return ResourceManager.GetString("Locked", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to eqlog_*.txt.
        /// </summary>
        internal static string logFilter {
            get {
                return ResourceManager.GetString("logFilter", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Lucky.
        /// </summary>
        internal static string Lucky {
            get {
                return ResourceManager.GetString("Lucky", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to (?&lt;attacker&gt;.+) (?:tr(?:ies|y)) to (?&lt;attackType&gt;(\S+|frenzy on)) (?&lt;victim&gt;.+), but (?:miss(?:|es))!(?:\s\((?&lt;damageSpecial&gt;.+)\)){0,1}.
        /// </summary>
        internal static string MissedMeleeAttack {
            get {
                return ResourceManager.GetString("MissedMeleeAttack", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to (?&lt;playerName&gt;.+)`s\s(?&lt;petName&gt;(pet|familiar|ward|warder)).
        /// </summary>
        internal static string petAndPlayerName {
            get {
                return ResourceManager.GetString("petAndPlayerName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Plugin Exited.
        /// </summary>
        internal static string pluginExited {
            get {
                return ResourceManager.GetString("pluginExited", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to EverQuest Damage Per Second Parser.
        /// </summary>
        internal static string pluginName {
            get {
                return ResourceManager.GetString("pluginName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Plugin Started.
        /// </summary>
        internal static string pluginStarted {
            get {
                return ResourceManager.GetString("pluginStarted", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to possesiveOf.
        /// </summary>
        internal static string possesiveOf {
            get {
                return ResourceManager.GetString("possesiveOf", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to RaidRoster_(?&lt;serverName&gt;.+)-(?&lt;date&gt;[\d]+)-(?&lt;time&gt;[\d]+).txt.
        /// </summary>
        internal static string raidAllyFileName {
            get {
                return ResourceManager.GetString("raidAllyFileName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to (?&lt;groupId&gt;[\d]+)\s(?&lt;playerName&gt;\S+)\s(?&lt;playerLevel&gt;[\d]+)\s(?&lt;playerClass&gt;\S+)\s(?&lt;raidRole&gt;.+\b).
        /// </summary>
        internal static string raidAllyFormat {
            get {
                return ResourceManager.GetString("raidAllyFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Riposte.
        /// </summary>
        internal static string Riposte {
            get {
                return ResourceManager.GetString("Riposte", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to secondaryPossesiveOf.
        /// </summary>
        internal static string secondaryPossesiveOf {
            get {
                return ResourceManager.GetString("secondaryPossesiveOf", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to (You|you|yourself|Yourself|YOURSELF|YOU|your|YOUR).
        /// </summary>
        internal static string selfMatch {
            get {
                return ResourceManager.GetString("selfMatch", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to (?&lt;attacker&gt;.+) ha(ve|s) slain (?&lt;victim&gt;.+)!.
        /// </summary>
        internal static string SlainMessage1 {
            get {
                return ResourceManager.GetString("SlainMessage1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to (?&lt;victim&gt;.+) ha(ve|s) been slain by (?&lt;attacker&gt;.+)!.
        /// </summary>
        internal static string SlainMessage2 {
            get {
                return ResourceManager.GetString("SlainMessage2", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to (?&lt;attacker&gt;.+) hit (?&lt;victim&gt;.*) for (?&lt;damagePoints&gt;[\d]+) (?:point[|s]) of (?&lt;typeOfDamage&gt;.+) damage by (?&lt;attackType&gt;.*)\.(?:[\s][\(](?&lt;damageSpecial&gt;.+)[\)]){0,1}.
        /// </summary>
        internal static string SpellDamage {
            get {
                return ResourceManager.GetString("SpellDamage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to (?&lt;victim&gt;.+) has taken (?&lt;damagePoints&gt;[\d]+) damage from (?&lt;damageEffect&gt;.*) by (?&lt;attacker&gt;.*)\.(?:[\s][\(](?&lt;spellSpecial&gt;.+)[\)]){0,1}.
        /// </summary>
        internal static string SpellDamageOverTime {
            get {
                return ResourceManager.GetString("SpellDamageOverTime", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to (?&lt;victim&gt;.+) resisted (?&lt;attacker&gt;.+) (?&lt;spellName&gt;.+)\!(?:\s\((?&lt;damageSpecial&gt;.+)\)){0,1}.
        /// </summary>
        internal static string spellResist {
            get {
                return ResourceManager.GetString("spellResist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Strikethrough.
        /// </summary>
        internal static string Strikethrough {
            get {
                return ResourceManager.GetString("Strikethrough", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to (?&lt;CharacterName&gt;.+) (tells|told|say(|s)|said).
        /// </summary>
        internal static string tellsRegex {
            get {
                return ResourceManager.GetString("tellsRegex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Twincast.
        /// </summary>
        internal static string Twincast {
            get {
                return ResourceManager.GetString("Twincast", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to (?&lt;Unknown&gt;(u|U)nknown).
        /// </summary>
        internal static string Unknown {
            get {
                return ResourceManager.GetString("Unknown", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Wild Rampage.
        /// </summary>
        internal static string WildRampage {
            get {
                return ResourceManager.GetString("WildRampage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to (?&lt;victim&gt;.+) died..
        /// </summary>
        internal static string youDied {
            get {
                return ResourceManager.GetString("youDied", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You have entered (?!.*an area where levitation effects do not function)(?!.*the Drunken Monkey stance adequately)(?&lt;zoneName&gt;.*)..
        /// </summary>
        internal static string zoneChange {
            get {
                return ResourceManager.GetString("zoneChange", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to \[(?:.+)\](?:[\d]+):Received MSG_EQ_ADDPLAYER, Player = (?&lt;characterEnteringZone&gt;.+), zone = (?&lt;ZoneName&gt;.+).
        /// </summary>
        internal static string zoneEnter {
            get {
                return ResourceManager.GetString("zoneEnter", resourceCulture);
            }
        }
    }
}
