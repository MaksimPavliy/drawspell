using FriendsGamesTools;
using FriendsGamesTools.ECSGame;

namespace DrawSpell
{
    public class DrawSpellProgressSkinController : ProgressSkinController
    {
        DrawSpellProgressSkinConfig config => DrawSpellProgressSkinConfig.instance;
        protected override int GetPercentsPerLevel() => Utils.Random(config.percentsPerLevelMin, config.percentsPerLevelMax);
    }
}