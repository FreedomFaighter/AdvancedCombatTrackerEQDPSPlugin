using Advanced_Combat_Tracker;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace EverQuestDPS
{
    public partial class EverQuestDPSPlugin
    {
        private FileSystemWatcher watcherForRaidRoster;
 
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
            Regex raidAllyLineRegex = new Regex(Properties.EQDPSPlugin.raidAllyFormat);
            Regex filename = new Regex(Properties.EQDPSPlugin.raidAllyFileName);
            Match m = filename.Match(e.Name);
            if (e.ChangeType.HasFlag(WatcherChangeTypes.Created) && m.Success)
            {
                List<CombatantData> data = new List<CombatantData>();

                using (StreamReader sr = new StreamReader(new FileStream(e.FullPath, FileMode.Open)))
                {
                    String line;

                    while ((line = sr.ReadLine()) != null)
                    {
                        Match raidLineMatch = raidAllyLineRegex.Match(line);
                        CombatantData combatantData = new CombatantData(raidLineMatch.Groups["playerName"].Value, ActGlobals.oFormActMain.ActiveZone.ActiveEncounter);

                        if (combatantData != null && combatantData.Name != ActGlobals.charName)
                            data.Add(combatantData);
                    }
                }
                
                ActGlobals.oFormActMain.ActiveZone.ActiveEncounter.SetAllies(data);
            }
        }
    }
}
