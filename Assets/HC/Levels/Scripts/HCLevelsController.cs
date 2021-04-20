﻿using FriendsGamesTools;
using FriendsGamesTools.ECSGame;

namespace HC
{
    public class HCLevelsController : LevelBasedController
    {
        protected override (bool win, bool lose) CheckWinLose() => (false, false);
        HCMoneyConfig moneyConfig => HCMoneyConfig.instance;
        public override int levelWinMoney => moneyConfig.levelWinMoneyBase + currLocationInd * moneyConfig.levelWinMoneyCoef;
        public override float levelWinX3Chance => moneyConfig.levelWinX3Chance;
        public override int winStarsCount => Utils.Random(0, 1f) <= 0.7f ? 3 : 2;
        public bool LastGameIsWin { private set; get; } = false;
        public override void RestartLocation()
        {
            base.RestartLocation();

            HCLevelsView.instance?.Reset();
        }

        public override void Play()
        {
            base.Play();

            HCAnalyticsManager.LevelStart(currLocationInd.ToString());
        }

        protected override void OnWin()
        {
            base.OnWin();
            LastGameIsWin = true;
            HCAnalyticsManager.LevelFinish(currLocationInd.ToString());
        }

        protected override void OnLose()
        {
            base.OnLose();
            LastGameIsWin = false;
            HCAnalyticsManager.LevelFailed(currLocationInd.ToString());
        }
    }
}