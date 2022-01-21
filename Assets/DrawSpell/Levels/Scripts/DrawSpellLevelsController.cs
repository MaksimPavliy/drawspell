using FriendsGamesTools;
using FriendsGamesTools.ECSGame;

namespace DrawSpell
{
    public class DrawSpellLevelsController : LevelBasedController
    {
        protected override (bool win, bool lose) CheckWinLose() => (false, false);
        DrawSpellMoneyConfig moneyConfig => DrawSpellMoneyConfig.instance;
        public override int levelWinMoney => moneyConfig.levelWinMoneyBase + currLocationInd * moneyConfig.levelWinMoneyCoef+GameManager.instance.CoinsCollected;
#if ADS
        public override float levelWinX3Chance => moneyConfig.levelWinX3Chance;
#endif
        public override int winStarsCount => 3;
        public bool LastGameIsWin { private set; get; } = false;
        public override void RestartLocation()
        {
            base.RestartLocation();

            DrawSpellLevelsView.instance?.Reset();
        }

        public override void Play()
        {
            base.Play();

            GameManager.instance.OnPlay();
            HCAnalyticsManager.LevelStart(currLocationInd.ToString());
        }

        protected override void OnWin()
        {
            base.OnWin();
            LastGameIsWin = true;
            HCAnalyticsManager.LevelFinish(currLocationInd.ToString(), winStarsCount);
        }

        protected override void OnLose()
        {
            base.OnLose();
            LastGameIsWin = false;
            HCAnalyticsManager.LevelFailed(currLocationInd.ToString());
        }

        public override void DebugChangeLocation(int newLocationInd)
        {
            base.DebugChangeLocation(newLocationInd);
            GoToMenu();
        }
    }
}