namespace HcUtils
{
    public class DrawSpellAdButtonBlocker : AdButtonBlocker
    {
#if ADS
        protected override bool GameLoaded => DrawSpellAdsManager.instance;
        protected override bool RewardedAvailable => DrawSpellAdsManager.instance.rewarded.available;
#endif
    }
}