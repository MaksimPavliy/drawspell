using FriendsGamesTools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DrawSpell
{
    public class SpawnSettings: MonoBehaviour
    {

    }

    public class EnemySpawner : MonoBehaviourHasInstance<EnemySpawner>
    {
        [SerializeField] private Transform m_targetTransform;
        [SerializeField] private List<Shape.ShapeType> m_allowedShapes;
        [SerializeField] private List<EnemyType> m_allowedEnemies;
        [SerializeField] private EnemyTrigger m_triggerPrefab;
        [SerializeField] private List<EnemyTrigger> m_triggers;
        [SerializeField] private List<Enemy> m_spawnedEnemies = new List<Enemy>();
        [SerializeField] private Player player;
        [SerializeField] private float playerOffset = 10;
        public List<Shape.ShapeType> AllowedShapes=>m_allowedShapes;
        public List<EnemyTrigger> EnemyTriggers => m_triggers;
        public EnemyTrigger LastTrigger=>EnemyTriggers[EnemyTriggers.Count-1];
        public List<Enemy> SpawnedEnemies => m_spawnedEnemies;
        private List<Enemy> list;

        public int spawnDistance = 60;
        private float minOffset = -1.6f;
        private float maxOffset = 1.6f;

        [SerializeField]
        private float m_spawnInterval = 10;
        [SerializeField]
        private float m_spawnIntervalDeviation = 4;
        [SerializeField]
        private int m_enemiesCount = 10;

        public int EnemiesSpawnedCount { private set; get; } = 0;
        public int EnemiesKilled { private set; get; } = 0;
        public event Action<Enemy> EnemySpawned;
        public event Action<Enemy> EnemyKilled;
        public bool AllEnemiesKilled => EnemiesKilled == m_enemiesCount;
        private void Start()
        {
            foreach (var tr in m_triggers)
            {
                tr.Triggered += Tr_Triggered;
            }
        }

        public Vector3 GetLastTriggerPosition() => EnemyTriggers[EnemyTriggers.Count - 1].transform.position;
        public void SpawnTriggers(out Vector3 lastSpawnPoint)
        {
            
            foreach (var tr in m_triggers)
            {
                DestroyImmediate(tr.gameObject);
            }
            m_triggers.Clear();

            float _lastPosition= player.transform.position.z+playerOffset;
            
            EnemyTrigger trigger=null;
            for (int i = 0; i < m_enemiesCount; i++)
            {
                trigger = PrefabUtils.InstantiatePrefab<EnemyTrigger>(m_triggerPrefab, m_targetTransform);
                trigger.transform.position = new Vector3(0, m_targetTransform.position.y, m_targetTransform.position.z + _lastPosition);
                _lastPosition += m_spawnInterval + Utils.Random(-m_spawnIntervalDeviation, m_spawnIntervalDeviation);
                var enemyType = Utils.RandomElement(m_allowedEnemies);
                trigger.SetType(enemyType);
                var shapes = new List<Shape.ShapeType>();

                for (int j = 0; j < GameSettings.instance.GetEnemyInfo(enemyType).hp; j++)
                {
                    shapes.Add(Utils.RandomElement(m_allowedShapes));
                }
                trigger.SetShapes(shapes);

                m_triggers.Add(trigger);
                
            }
            lastSpawnPoint = trigger.transform.position+Vector3.forward*spawnDistance;
            PrefabUtils.SetPrefabDirty();
        }
        private void Tr_Triggered(EnemyTrigger trigger)
        {
            //m_targetTransform.position.x + UnityEngine.Random.Range(minOffset, maxOffset)
            var position = new Vector3(m_targetTransform.position.x + EnemiesSpawnedCount % 2==0?minOffset:maxOffset,
                m_targetTransform.position.y,
                trigger.transform.position.z + spawnDistance);

            SpawnEnemy(position, trigger.Type, trigger.Shapes);
        }

        private void SpawnEnemy(Vector3 position, EnemyType type, List<Shape.ShapeType> shapes)
        {
            var enemy=SpawnEnemyFromPool(position, type, shapes);
            EnemySpawned?.Invoke(enemy);
            EnemiesSpawnedCount++;

        }

        public Enemy SpawnEnemyFromPool(Vector3 position, EnemyType type, List<Shape.ShapeType> shapes)
        {
            if (list == null) list = new List<Enemy>();

            if (type == EnemyType.None)
            {
                type=Utils.RandomElement(m_allowedEnemies.Where(x => x != EnemyType.None).ToList());
            }

            if (type == EnemyType.None) return null ;

            var enemyInfo = GameSettings.instance.GetEnemyInfo(type);
            var pooledEnemy = list.Find(x => x != null && !x.gameObject.activeSelf && x.EnemyType == type);
            if (!pooledEnemy)
            {
                pooledEnemy = Instantiate(enemyInfo.prefab, position, Quaternion.identity, m_targetTransform);
                list.Add(pooledEnemy);
                pooledEnemy.Died += PooledEnemy_Killed;
            }
            pooledEnemy.transform.position = position;
            pooledEnemy.GenerateShapes(shapes);
            pooledEnemy.gameObject.SetActive(true);
            m_spawnedEnemies.Add(pooledEnemy);
   

            return pooledEnemy;
        }

        private void PooledEnemy_Killed(IDamageable obj)
        {
            m_spawnedEnemies.Remove(obj as Enemy);
            EnemiesKilled++;
            EnemyKilled?.Invoke(obj as Enemy);
        }
    }
}