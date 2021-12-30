using FriendsGamesTools;
using FriendsGamesTools.ECSGame;
using UnityEngine;

namespace DrawSpell
{
    public class GameManager : MonoBehaviourHasInstance<GameManager>
    {
        [SerializeField] private Player player;
        [SerializeField] private Boss boss;
        [SerializeField] private GameObject portal;
        [SerializeField] private EnemySpawner spawner;
        [SerializeField] private GameObject lean;

        private bool isPlaying = false;

        public bool IsPlaying { get => isPlaying; set => isPlaying = value; }
        private void Start()
        {
            boss.Died += Boss_Died;
        }

        private void Boss_Died(IDamageable damageable)
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