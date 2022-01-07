using FriendsGamesTools;
using FriendsGamesTools.ECSGame;
using HcUtils;
using UnityEngine;

namespace DrawSpell
{
    public class GameManager : MonoBehaviourHasInstance<GameManager>
    {
        [SerializeField] private Player player;
        [SerializeField] private GameObject portal;
        [SerializeField] private EnemySpawner spawner;
        [SerializeField] private GameObject lean;

        public float EnemiesSpeed = 1f;
        private bool isPlaying = false;

        [SerializeField] private int colorSetIndex = 0;
        public bool IsPlaying { get => isPlaying; set => isPlaying = value; }
        private void Start()
        {

            ThemeSet.instance.ActivateSet(DrawSpellGeneralConfig.instance.customColorSets?colorSetIndex:0);
            player.LevelPassed += LevelPassed;
        }

        private void LevelPassed()
        {
            portal.gameObject.SetActive(true);
        }

        public void OnPlay()
        {
            lean.SetActive(true);
            IsPlaying = true;
            player.OnPlay();
            spawner.OnPlay();
        }

        public void DoWin()
        {
            RemoveLineRenderers();
            lean.SetActive(false);
            GameRoot.instance.Get<WinnableLocationsController>().DoWin();
            IsPlaying = false;
        }

        public void DoLose()
        {
            RemoveLineRenderers();
            lean.SetActive(false);
            GameRoot.instance.Get<WinnableLocationsController>().DoLose();
            IsPlaying = false;
        }

        private void RemoveLineRenderers()
        {
            FindObjectsOfType<LineRenderer>().ForEach(lineRenderer => Destroy(lineRenderer.gameObject));
        }
    }
}