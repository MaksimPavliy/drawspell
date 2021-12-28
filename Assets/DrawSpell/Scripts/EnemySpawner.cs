using FriendsGamesTools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DrawSpell
{

    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private Enemy[] enemies;
        [SerializeField] private Player player;
        [SerializeField] private EnemyTrigger triggerPrefab;
        [SerializeField] private List<EnemyTrigger> triggers;

        private List<Enemy> list;
        private int randomEnemy;
        private int spawnDistance = 50;
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
                SpawnEnemyFromPool(position);
        }

        public Enemy SpawnEnemyFromPool(Vector3 position)
        {
            if (list == null) list = new List<Enemy>();

            var pooledEnemy = list.Find(x => x != null && !x.gameObject.activeSelf && x.EnemyType == enemies[randomEnemy].EnemyType);
            if (!pooledEnemy)
            {
                pooledEnemy = Instantiate(enemies[randomEnemy], position, Quaternion.identity, gameObject.transform);
                pooledEnemy.Player = player;
                pooledEnemy.GenerateShapes();
                list.Add(pooledEnemy);
            }
            pooledEnemy.transform.position = position;
            foreach (var shape in pooledEnemy.Shapes)
            {
                shape.EnableShape(true);
                shape.gameObject.SetActive(true);
            }
            pooledEnemy.gameObject.SetActive(true);
            player.Enemies.Add(pooledEnemy);

            return pooledEnemy;
        }
    }
}