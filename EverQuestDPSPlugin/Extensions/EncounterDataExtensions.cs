using Advanced_Combat_Tracker;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EverQuestDPS.Extensions
{
    internal static class EncounterDataExtensions
    {
        internal static EncounterData GetTag(this EncounterData encounterData, String tagName)
        {
            IDictionary<string, CombatantData> sortedListOfCombatantData = new SortedList<string, CombatantData>();
            
            foreach(var item in encounterData.Items.Where(kvp => kvp.Value.GetTag(tagName).Items.Any()).Select(kvp => new KeyValuePair<string, CombatantData>(kvp.Key, kvp.Value.GetTag(tagName))))
                sortedListOfCombatantData.Add(item.Key, item.Value);
            return sortedListOfCombatantData.ToEncounterData(encounterData);
        }

        internal static long GetPetDamageTotal(this EncounterData encounterData)
        {
            return encounterData.Items.Sum(x => x.Value.GetDamageByTag(Properties.PluginRegex.pet));
        }

        internal static double GetPetDPS(this EncounterData encounterData)
        {
            return (double)encounterData.GetPetDamageTotal() / (double)encounterData.Duration.Seconds;
        }

        internal static int GetCount(this EncounterData encounterData)
        {
            return encounterData.Items.Sum(x => x.Value.GetCount());
        }

        private static EncounterData ToEncounterData(this IDictionary<string, CombatantData> dictionaryOfPairs, EncounterData encounterData)
        {
            return new EncounterData(encounterData.CharName, encounterData.ZoneName, encounterData.Parent)
            {
                Items = new SortedList<string, CombatantData>(dictionaryOfPairs)
            };
        }
    }
}
