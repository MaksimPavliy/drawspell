using FriendsGamesTools;
using FriendsGamesTools.ECSGame;
using HcUtils;
using UnityEngine;

namespace DrawSpell
{
    public class GameManager : MonoBehaviourHasInstance<GameManager>
    {
        [SerializeField] private Player player;
        [SerializeField] private Transform m_levelPath;
        [SerializeField] private GameObject portal;
        private EnemySpawner _spawner;
        public int CoinsCollected { set; get; }

        public int playerHPModifier = 0;

        public bool HasBoss => Boss.instance != null;

        public EnemySpawner EnemySpawner { get
            {
                if (!_spawner) _spawner = GetComponent<EnemySpawner>();
                return _spawner;
            } 
        }
        public float EnemiesSpeed = 1f;
        private bool isPlaying = false;

        [SerializeField] private int colorSetIndex = 0;
        public bool IsPlaying { get => isPlaying; set => isPlaying = value; }
        private void Start()
        {
            ThemeSet.instance.ActivateSet(DrawSpellGeneralConfig.instance.customColorSets ? colorSetIndex : 0);
            player.LevelPassed += LevelPassed;
            EnemySpawner.EnemyKilled += EnemySpawner_EnemyKilled;
            ShapeRecognizer.instance.Show(true);
        }

        private void EnemySpawner_EnemyKilled(Enemy obj)
        {
            if (EnemySpawner.AllEnemiesKilled)
            {
                player.SpeedUp();
                if (!HasBoss)
                {
                    portal.transform.position = player.transform.position + Vector3.forward * 6f;
                    
                    portal.gameObject.SetActive(true);

                }
            }
        }

        [ContextMenu("Create Level")]
        public void CreateLevel()
        {
            
            EnemySpawner.SpawnTriggers(out Vector3 lastSpawn);
            m_levelPath.transform.position = Vector3.forward * (lastSpawn.z);
            portal.transform.position = m_levelPath.transform.position + Vector3.up * 5f;
            var boss = GetComponentInChildren<Boss>();
            if (boss)
            {
                boss.SetShapes(EnemySpawner.AllowedShapes);
            }
        }
        private void LevelPassed()
        {
            portal.gameObject.SetActive(true);
        }

        public void OnPlay()
        {
            //lean.SetActive(true);
            IsPlaying = true;
            player.OnPlay();
        }

        public void DoWin()
        {
            ShapeRecognizer.instance.Show(false);
            GameRoot.instance.Get<WinnableLocationsController>().DoWin();
            IsPlaying = false;
        }

        public void DoLose()
        {
            ShapeRecognizer.instance.Show(false);
            GameRoot.instance.Get<WinnableLocationsController>().DoLose();
            IsPlaying = false;
        }


    }
}