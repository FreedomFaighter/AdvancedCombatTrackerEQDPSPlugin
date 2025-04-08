using Advanced_Combat_Tracker;
using System;
using System.Collections.Generic;
using System.Linq;
using EverQuestDPS.Enums;

namespace EverQuestDPS.Extensions
{
    internal static class AttackTypeExtensions
    {
        internal static AttackType GetSwingWithSummonCriteria(this AttackType attackType, bool pet, bool ward, bool warder, bool familiar)
        {
            List<String> summonedEntities = new List<string>();

            if(pet)
            {
                summonedEntities.Append(Properties.PluginRegex.pet);
            }
            if(ward)
            {
                summonedEntities.Append(Properties.PluginRegex.ward);
            }
            if (warder)
            {
                summonedEntities.Append(Properties.PluginRegex.warder);
            }
            if(familiar)
            {
                summonedEntities.Append(Properties.PluginRegex.familiar);
            }
            if (summonedEntities.DefaultIfEmpty() == default)
            {
                summonedEntities.Append(String.Empty);
            }

            bool predicate(MasterSwing masterSwing)
            {
                return summonedEntities.Contains(masterSwing.Tags[Properties.PluginRegex.OutgoingTag]);
            }

            return attackType.GetListOfMasterSwingWithPredicate(predicate);
        }

        internal static long GetDamageWithSummonCriteria(this AttackType attackType, bool pet, bool ward, bool warder, bool familiar)
        {
            return attackType.GetSwingWithSummonCriteria(pet, ward, warder, familiar).Items.Sum(x => x.Damage);
        }

        internal static long GetOutgoingDamageWithSummonCriteriaAndDnumExcept(this AttackType attackType, bool pet, bool ward, bool warder, bool familiar, List<Dnum> dnums)
        {
            return new AttackType(attackType.Type
                , attackType.Parent).GetAttackTypeListWithDnumListExcept(dnums).GetDamageWithSummonCriteria(pet, ward, warder, familiar);
        }

        private static AttackType ToAttackType(this IEnumerable<MasterSwing> masterSwingEnumerable, AttackType attackType)
        {
            return new AttackType(attackType.Type, attackType.Parent)
            {
                Items = masterSwingEnumerable.ToList()
            };
        }

        private static AttackType GetListOfMasterSwingWithPredicate(this AttackType attackType, Predicate<MasterSwing> predicate)
        {
            return attackType.Items.Where(x => predicate(x)).ToAttackType(attackType);
        }

        internal static AttackType GetTagValueAsList(this AttackType attackType, string tagName)
        {
            return attackType.Items.Where(x => x.Tags.ContainsKey(tagName)).Select(x => x.Tags[tagName]).Cast<MasterSwing>().ToAttackType(attackType);
        }

        internal static int GetCount(this AttackType attackType)
        {
            return attackType.Items.Count();
        }

        internal static AttackType GetAttackTypeListWithDnumListExcept(this AttackType attackType, List<Dnum> dnums)
        {
            return attackType.Items.Where(x => !dnums.Contains(x.Damage)).ToAttackType(attackType);
        }

        internal static int GetCountWithDnumListExcept(this AttackType attackTypeData, List<Dnum> dnums)
        {
            return attackTypeData.Items.Count(item => !dnums.Contains(item.Damage));
        }

        internal static int GetCountCritType(this AttackType attackTypeData, Specials attackTypeCritType)
        {
            return attackTypeData.Items.Where(item => item.Damage >= 0).Sum((item) => ((Specials)item.Tags[Properties.PluginRegex.SpecialStringTag]).HasFlag(attackTypeCritType) ? 1 : 0);
        }

        internal static long GetPetDamageTotalFromAttackType(this AttackType attackType)
        {
            return attackType.Items.Where(attackTypeData => attackTypeData.GetPetAttack()).Sum(x => x.Damage.Number);
        }

        internal static long GetWardHealingTotal(this AttackType attackType)
        {
            return attackType.Items.Where(attackTypeData => attackTypeData.GetWardHealing()).Sum(x => x.Damage);
        }
    }
}
