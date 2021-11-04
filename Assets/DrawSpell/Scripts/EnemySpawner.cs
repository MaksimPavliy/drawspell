using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DrawSpell
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private Enemy[] enemies;
        [SerializeField] private Player player;

        private List<Enemy> list;
        private int randomEnemy;
        private int spawnDistance = 70;
        private float spawnDelay => DrawSpellGeneralConfig.instance.spawnDelay;
        private float minOffset = 1.2f;
        private float maxOffset = 3.8f;

        public void OnPlay()
        {
            StartCoroutine(SpawnEnemy());
        }

        private IEnumerator SpawnEnemy()
        {
            if (GameManager.instance.IsPlaying)
            {
                randomEnemy = Random.Range(0, enemies.Length);
                SpawnEnemyFromPool(new Vector3(Random.Range(minOffset, maxOffset),
                enemies[randomEnemy].transform.position.y, player.transform.position.z + spawnDistance));
                yield return new WaitForSeconds(spawnDelay);
                StartCoroutine(SpawnEnemy());
            }
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