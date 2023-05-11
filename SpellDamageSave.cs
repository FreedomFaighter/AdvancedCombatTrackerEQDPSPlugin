using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACT_EverQuest_DPS_Plugin
{
    internal enum SpellDamageSave
    {
        Fire = 1,
        Cold = 2,
        Poison = 4,
        Disease = 8,
        Magic = 16,
        Corruption = 32,
        Unresistable = 64
    }
}
