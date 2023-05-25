//Enum describing currently known special attacks that are written to the log file while actions occur in game
//Currently appear after a combat action in parenthesis as a space seperated string
namespace EverQuestDPSPlugin
{
    internal enum SpecialAttacks
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
}
