using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace EverQuestDPS.Enums
{
    [Flags]
    internal enum Specials
    {
        [Description("")]
        None,
        [Description("Crippling Blow")]
        CripplingBlow = 1,
        [Description("Wild Rampage")]
        WildRampage = 2,
        [Description("Twincast")]
        Twincast = 4,
        [Description("Strikethrough")]
        Strikethrough = 8,
        [Description("Riposte")]
        Riposte = 16,
        [Description("Lucky")]
        Lucky = 32,
        [Description("Locked")]
        Locked = 64,
        [Description("Flurry")]
        Flurry = 128,
        [Description("Double Bow Shot")]
        DoubleBowShot = 256,
        [Description("Finishing Blow")]
        FinishingBlow = 512
    }



    internal static class SpecialParsers
    {
        internal static String GetString(Specials specials)
        {
            var enumType = typeof(Specials);
            String returnValue = ((DescriptionAttribute)enumType.GetField(Enum.GetName(enumType, specials)).GetCustomAttribute(typeof(DescriptionAttribute))).Description;
            return returnValue;
        }

        internal static Specials GetSpecialByString(String special)
        {
            foreach (Specials s in Enum.GetValues(typeof(Specials)))
            {
                if (GetString(s) == special)
                    return s;
            }
            return Specials.None;
        }
    }
}