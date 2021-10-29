using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DrawSpell
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private Enemy[] enemies;
        [SerializeField] private Player player;

        private Enemy enemy;
        private int randomEnemy;
        private int spawnDistance = 70;
        private float spawnDelay => DrawSpellGeneralConfig.instance.spawnDelay;
        private float minOffset = 1.2f;
        private float maxOffset = 3.8f;

        void Start()
        {
            StartCoroutine(SpawnEnemy());
        }

        private IEnumerator SpawnEnemy()
        {
            randomEnemy = Random.Range(0, enemies.Length);
            enemy = Instantiate(enemies[randomEnemy], new Vector3(Random.Range(minOffset, maxOffset), enemies[randomEnemy].transform.position.y, player.transform.position.z + spawnDistance), Quaternion.identity, gameObject.transform);
            enemy.Player = player;
            player.Enemies.Add(enemy);
            yield return new WaitForSeconds(spawnDelay);
            StartCoroutine(SpawnEnemy());
        }
    }
}