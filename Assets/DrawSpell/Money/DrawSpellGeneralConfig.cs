using FriendsGamesTools.DebugTools;

namespace DrawSpell
{
    public class DrawSpellGeneralConfig : BalanceSettings<DrawSpellGeneralConfig>
    {
        public int CameraIndex = 0;
        public float speedPlayer = 4;
        public float speedEnemy = 4;
        public float spawnDelay = 1f;
        public int levelCompletionKillCount = 15;
        public bool customColorSets = true;
        public int startPlayerHP = 2;
    }
}