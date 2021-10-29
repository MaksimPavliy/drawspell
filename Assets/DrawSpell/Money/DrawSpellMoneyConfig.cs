using FriendsGamesTools.DebugTools;

namespace DrawSpell
{
    public class DrawSpellMoneyConfig : BalanceSettings<DrawSpellMoneyConfig>
    {
        public int levelWinMoneyBase = 50;
        public int levelWinMoneyCoef = 0;
        public float levelWinX3Chance => 0.25f;
    }
}