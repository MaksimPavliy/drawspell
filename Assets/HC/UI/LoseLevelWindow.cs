﻿using FriendsGamesTools.Ads;
using UnityEngine;
using UnityEngine.UI;


namespace HC
{
    public class LoseLevelWindow: EndLevelWindow
    {
        [SerializeField] private Button restartLevelButton;
        [SerializeField] private Button continueForAdButton;
        public override string shownText => base.shownText + "FAILED!";
        private void Awake()
        {
            restartLevelButton?.onClick.AddListener(RestartLevel);
#if ADS
            continueForAdButton?.GetComponent<WatchAdButtonView>()?.SubscribeAdWatched(ContinueForAd);
#endif
        }

        public void ContinueForAd() => Continue();
        float showDelay => 1.5f;
        public override async void Show(bool show = true)
        {
            if (show)
            {
                continueForAdButton?.gameObject.SetActive(proposeAds);
                await Awaiters.Seconds(showDelay);
            }
            base.Show(show);
        }
    }
}