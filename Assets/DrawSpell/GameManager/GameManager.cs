using FriendsGamesTools;
using FriendsGamesTools.ECSGame;
using UnityEngine;

namespace DrawSpell
{
    public class GameManager: MonoBehaviourHasInstance<GameManager>
    {
        [SerializeField] private Player player;
        [SerializeField] private GameObject lean;
        private bool isPlaying = false;
        
        public bool IsPlaying { get => isPlaying; set => isPlaying = value; }

        public void OnPlay()
        {
            IsPlaying = true;
            player.OnPlay();
        }

        public void DoWin()
        {
            GameRoot.instance.Get<WinnableLocationsController>().DoWin();
            IsPlaying = false;
            lean.SetActive(false);
        }

        public void DoLose()
        {
            GameRoot.instance.Get<WinnableLocationsController>().DoLose();
            IsPlaying = false;
            lean.SetActive(false);
        }
    }
}