using Advanced_Combat_Tracker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace EverQuestDPS.Extensions
{
    internal static class DamageTypeExtensions
    {
        internal static long GetDamageOfTag(this DamageTypeData damageTypeData, string tagName)
        {
            return damageTypeData.GetTag(tagName).Damage;
        }

        internal static DamageTypeData GetTag(this DamageTypeData damageTypeData, string tagName)
        {
            SortedList<string, AttackType> selectedByTag = new SortedList<string, AttackType>();
            foreach (KeyValuePair<string, AttackType> keyValuePair in damageTypeData.Items)
                if (keyValuePair.Value.GetTagValueAsList(tagName).Items.Any())
                    selectedByTag.Add(keyValuePair.Key, keyValuePair.Value.GetTagValueAsList(tagName));
            return new DamageTypeData(damageTypeData.Outgoing, damageTypeData.Type, damageTypeData.Parent)
            {
                Items = selectedByTag
            };
        }

        internal static int GetCount(this DamageTypeData damageTypeData)
        {
            return damageTypeData.Items.Sum((kvp) =>
            {
                return damageTypeData.Items.Values.ToList().Sum(x => x.GetCount());
            });
        }

        internal static int GetCountOfTagged(this DamageTypeData damageTypeData, string tagName)
        {
            return damageTypeData.GetTag(tagName).GetCount();
        }

        private static DamageTypeData ToDamageTypeData(this IDictionary<string, AttackType> attackTypes, bool Outgoing, string Tag, CombatantData parentCombatantData) {
            DamageTypeData damageTypeData = new DamageTypeData(Outgoing, Tag, parentCombatantData)
            {
                Items = new SortedList<string, AttackType>(attackTypes)
            };
            return damageTypeData;
        }

        internal static long GetPetDamageTotal(this DamageTypeData damageTypeData)
        {
            return damageTypeData.Items.Sum(damageType => damageType.Value.GetPetDamageTotalFromAttackType());
        }

        internal static long GetWardHealingTotal(this DamageTypeData damageTypeData)
        {
            return damageTypeData.Items.Sum(damageType => damageType.Value.GetWardHealingTotal());
        }
    }
}
