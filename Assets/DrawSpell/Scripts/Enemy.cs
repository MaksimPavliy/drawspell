using System.Collections.Generic;
using UnityEngine;
using static Shape;

namespace DrawSpell
{
    [RequireComponent(typeof(CharacterController))]
    public class Enemy : MonoBehaviour, IDamageable
    {
        private DrawSpellGeneralConfig config => DrawSpellGeneralConfig.instance;

        [SerializeField] private float runspeed => config.speedEnemy;
        [SerializeField] private Transform HpShapes;
        [SerializeField] private int damageToPlayer = 1;
        [SerializeField] private List<Spell> spellsToKill;
        [SerializeField] private float additionalShapeHeight;
        [SerializeField] private float shapeOffset = 0.6f;
        [SerializeField] private EnemyType enemyType;

        private CharacterController enemyController;
        private List<Shape> shapes = new List<Shape>();
        private Player player;
        private float moveForward = 1;
        private float attackAnimationDistance = 5f;
        private int activeShapesCount;

        public EnemyType EnemyType => enemyType;
        public Player Player { set => player = value; }
        public List<Spell> SpellsToKill => spellsToKill;
        public List<Shape> Shapes => shapes;

        public event IDamageable.OnDamage OnPlayerAttacked;

        private void Start()
        {
            enemyController = GetComponent<CharacterController>();
            activeShapesCount = shapes.Count;
        }

        void Update()
        {
            if (GameManager.instance.IsPlaying)
            {
                transform.forward = new Vector3(0, 0, -moveForward);

                enemyController.Move(new Vector3(0, 0, -moveForward * runspeed) * Time.deltaTime);

                if (Vector3.Distance(player.transform.position, transform.position) < attackAnimationDistance)
                {
                    OnPlayerAttacked?.Invoke(this);
                    player.TakeDamage(damageToPlayer);
                    gameObject.SetActive(false);
                }
            }
        }

        public void TakeDamage(ShapeType shapeType)
        {
            shapes.Find(shapeClone => shapeClone.Type == shapeType).gameObject.SetActive(false);
            activeShapesCount--;

            if (activeShapesCount <= 0)
            {
                gameObject.SetActive(false);
            }
        }

        public void GenerateShapes()
        {
            shapeOffset = spellsToKill.Count == 1 ? 0 : shapeOffset;
            var enemyInfo = GameSettings.instance.GetEnemyInfo(enemyType);
            if (enemyInfo==null) return;
            
            foreach (var shape in enemyInfo.shapesToKill)
            {
                var shapeInfo = GameSettings.instance.GetShapeInfo(shape);
                shapes.Add(Instantiate(shapeInfo.shape, new Vector3(transform.position.x + shapeOffset, shapeInfo.shape.transform.position.y + additionalShapeHeight, transform.position.z),
                    Quaternion.Euler(0, 180, 0), HpShapes).GetComponent<Shape>());
                shapeOffset = -shapeOffset;
            }
        }

        private void OnDisable()
        {
            player.UpdateKillCount();
            player.Enemies.Remove(this);
            activeShapesCount = shapes.Count;
        }
    }

    public enum EnemyType
    {
        Pumpkin,
        Spirit,
        Spider,
        Skeleton,
        Bat
    }
}