using FriendsGamesTools;
using FriendsGamesTools.ECSGame;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DrawSpell
{
    public class DrawSpellSelectorView:MonoBehaviourHasInstance<DrawSpellSelectorView>
    {
        public static List<DrawSpellSkinView> ActiveSkins = new List<DrawSpellSkinView>();
        public static void AddSkin(DrawSpellSkinView skin) => ActiveSkins.Add(skin);
        public void CleanUp() => ActiveSkins = ActiveSkins.Where(item => item != null).ToList();
        int lastSkinIndex = -1;
        private void Update()
        {
            var skinInd = GameRoot.instance.Get<DrawSpellMoneySkinController>().activeSkinInd;
            if (skinInd != lastSkinIndex)
            {
                lastSkinIndex = skinInd;
                UpdateSkins();
            }
        }
        public void UpdateSkins()
        {
            CleanUp();
            foreach (var skin in ActiveSkins)
            {
                skin.SetActiveSkin(GameRoot.instance.Get<DrawSpellMoneySkinController>().activeSkinInd);
            }
        }
    }
}