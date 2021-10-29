using FriendsGamesTools.IAP;

namespace DrawSpell
{
    public class NoAdsView : PurchasableProductView
    {
#if IAP
        private void Update()
        {
            this.gameObject.SetActive(!IAPManager.instance.interstitialsRemoved);
        }
#endif
    }
}