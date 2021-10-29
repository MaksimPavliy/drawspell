using FriendsGamesTools.Ads;
using UnityEngine;

namespace DrawSpell
{
    public class DrawSpellWatchAdButtonView : WatchAdButtonView
    {
        public GameObject adLoadingParent;
#if ADS
        protected override void InterstitialShow()
        {
            DrawSpellAdsManager.instance.ShowInterstitial(nameInAnalytics.ToString(),
                () => onWatchFinished.Invoke(true),
                () => onWatchFinished.Invoke(false));
        }

        protected override void RewardedShow()
        {
            DrawSpellAdsManager.instance.ShowRewarded(nameInAnalytics.ToString(),
                 () => onWatchFinished.Invoke(true));
        }

        private void Update()
        {
            if (adLoadingParent != null)
                adLoadingParent.gameObject.SetActive(!available);
        }
#endif
    }
}