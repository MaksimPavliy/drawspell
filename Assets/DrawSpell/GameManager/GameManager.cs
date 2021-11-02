using FriendsGamesTools;
using FriendsGamesTools.ECSGame;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DrawSpell
{
    public class GameManager : MonoBehaviourHasInstance<GameManager>
    {
        [SerializeField] private Player player;
        [SerializeField] private EnemySpawner spawner;
        [SerializeField] private GameObject lean;

        private bool isPlaying = false;

        public bool IsPlaying { get => isPlaying; set => isPlaying = value; }

        private void Start()
        {
            lean.SetActive(false);
        }

        public void OnPlay()
        {
            IsPlaying = true;
            player.OnPlay();
            spawner.OnPlay();
            lean.SetActive(true);
        }

        public void DoWin()
        {
            FindObjectsOfType<LineRenderer>().ForEach(lineRenderer => Destroy(lineRenderer.gameObject));
            GameRoot.instance.Get<WinnableLocationsController>().DoWin();
            IsPlaying = false;
            lean.SetActive(false);
        }

        public void DoLose()
        {
            FindObjectsOfType<LineRenderer>().ForEach(lineRenderer => Destroy(lineRenderer.gameObject));
            GameRoot.instance.Get<WinnableLocationsController>().DoLose();
            IsPlaying = false;
            lean.SetActive(false);
        }
    }
}