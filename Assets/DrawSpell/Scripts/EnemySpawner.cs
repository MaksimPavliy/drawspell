using FriendsGamesTools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DrawSpell
{

    public class EnemySpawner : MonoBehaviourHasInstance<EnemySpawner>
    {
        [SerializeField] private Enemy[] enemies;
        [SerializeField] private Player player;
        [SerializeField] private EnemyTrigger triggerPrefab;
        [SerializeField] private List<EnemyTrigger> triggers;
        [SerializeField] private List<Enemy> spawnedEnemies = new List<Enemy>();
        public List<Enemy> SpawnedEnemies => spawnedEnemies;


        private List<Enemy> list;
        private int randomEnemy;
        [SerializeField]
        private int spawnDistance = 60;
        private float spawnDelay => DrawSpellGeneralConfig.instance.spawnDelay;
        private float minOffset = -1.6f;
        private float maxOffset = 1.6f;

        [SerializeField]
        private float _spawnInterval = 10;
        [SerializeField]
        private float _spawnIntervalDeviation = 4;
        [SerializeField]
        private float _enemiesCount = 10;
        public void OnPlay()
        {
          //  StartCoroutine(SpawnEnemy());
          
        }

        private void Start()
        {
            foreach (var tr in triggers)
            {
                tr.Triggered += Tr_Triggered;
            }
        }

        [ContextMenu("Spawn Enemy Triggers")]
        public void SpawnTriggers()
        {
            
            foreach (var tr in triggers)
            {
                DestroyImmediate(tr.gameObject);
            }
            triggers.Clear();

            float _lastPosition=-30;

            for (int i = 0; i < _enemiesCount; i++)
            {
                var trigger = PrefabUtils.InstantiatePrefab<EnemyTrigger>(triggerPrefab, transform);
                trigger.transform.position = new Vector3(0, transform.position.y, transform.position.z + _lastPosition);
                _lastPosition += _spawnInterval + Utils.Random(-_spawnIntervalDeviation, _spawnIntervalDeviation);
                triggers.Add(trigger);
            }

            PrefabUtils.SetPrefabDirty();
        }
        private void Tr_Triggered(EnemyTrigger trigger)
        {
            SpawnEnemy(new Vector3(transform.position.x + UnityEngine.Random.Range(minOffset, maxOffset), transform.position.y, trigger.transform.position.z + spawnDistance));
        }

        private void SpawnEnemy(Vector3 position)
        {
                randomEnemy = UnityEngine.Random.Range(0, enemies.Length);
                var enemy=SpawnEnemyFromPool(position);
          
        }

        private void Enemy_Died(IDamageable damageable)
        {
            spawnedEnemies.Remove(damageable as Enemy);
        }

        public Enemy SpawnEnemyFromPool(Vector3 position)
        {
            if (list == null) list = new List<Enemy>();

            var pooledEnemy = list.Find(x => x != null && !x.gameObject.activeSelf && x.EnemyType == enemies[randomEnemy].EnemyType);
            if (!pooledEnemy)
            {
                pooledEnemy = Instantiate(enemies[randomEnemy], position, Quaternion.identity, gameObject.transform);
                pooledEnemy.Player = player;
                list.Add(pooledEnemy);
            }
            pooledEnemy.transform.position = position;
            pooledEnemy.GenerateShapes();
            pooledEnemy.gameObject.SetActive(true);
            spawnedEnemies.Add(pooledEnemy);
            pooledEnemy.Died += PooledEnemy_Killed;
            //player.Enemies.Add(pooledEnemy);

            return pooledEnemy;
        }

        private void PooledEnemy_Killed(IDamageable obj)
        {
            spawnedEnemies.Remove(obj as Enemy);
        }
    }
}