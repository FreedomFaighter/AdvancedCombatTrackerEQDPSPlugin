using Advanced_Combat_Tracker;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EverQuestDPS.Extensions
{
    internal static class CombatantDataExtensions
    {
        internal static long GetDamageByTag(this CombatantData combatantData, string tagName)
        {
            return combatantData.GetTag(tagName).Damage;
        }

        internal static CombatantData GetTag(this CombatantData combatantData, string tagName)
        {
            Dictionary<string, DamageTypeData> damageTypeData = new Dictionary<string, DamageTypeData>();
            foreach (var item in combatantData.Items)
            {
                damageTypeData[item.Key] = item.Value.GetTag(tagName);
            }

            return damageTypeData.ToCombatantData(combatantData.Name, combatantData.Parent);
        }

        internal static int GetCount(this CombatantData combatantDataTypeData)
        {
            return combatantDataTypeData.Items.Sum(x => x.Value.GetCount());
        }

        internal static long GetPetDamageTotal(this CombatantData combatantDataTypeData)
        {
            return combatantDataTypeData.Items.Sum(combatantDataData => { return combatantDataData.Value.GetPetDamageTotal(); });
        }

        internal static long GetWardHealingTotal(this CombatantData combatantDataTypeData)
        {
            return combatantDataTypeData.Items.Sum(combatantDataData => combatantDataData.Value.GetWardHealingTotal());
        }

        private static CombatantData ToCombatantData(this IDictionary<string, DamageTypeData> combatantDataTo, string combatantName, EncounterData parentEncounterData)
        {
            return new CombatantData(combatantName, parentEncounterData)
            {
                Items = new Dictionary<string, DamageTypeData>(combatantDataTo)
            };

        }
    }
}
