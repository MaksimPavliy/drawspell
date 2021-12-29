using System;
using UnityEngine;
using static Shape;

namespace DrawSpell
{
    [RequireComponent(typeof(CharacterController))]
    public class Enemy : MonoBehaviour, IDamageable
    {
        private DrawSpellGeneralConfig config => DrawSpellGeneralConfig.instance;

        [SerializeField] private float runspeed => config.speedEnemy;
        [SerializeField] private HPShapes hpShapes;
        [SerializeField] private int damageToPlayer = 1;
        [SerializeField] private float additionalShapeHeight;
        [SerializeField] private float shapeOffset = 0.6f;
        [SerializeField] private EnemyType enemyType;

        private CharacterController enemyController;
        private Player player;
        private float moveForward = 1;
        private float attackAnimationDistance = 5f;

        public EnemyType EnemyType => enemyType;
        public Player Player { set => player = value; }

        public int HP => throw new NotImplementedException();

        public HPShapes Shapes => hpShapes;

        public event IDamageable.OnDamage OnPlayerAttacked;
        public event IDamageable.OnDied Died;
        private void Start()
        {
            enemyController = GetComponent<CharacterController>();
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
            hpShapes.DisposeShape(shapeType);
            if (hpShapes.Shapes.Count <= 0)
            {
                Died?.Invoke(this);
                gameObject.SetActive(false);
            }
        }

        public void GenerateShapes()
        {
            var enemyInfo = GameSettings.instance.GetEnemyInfo(enemyType);
            if (enemyInfo==null) return;

            hpShapes.ClearShapes();
            foreach (var shape in enemyInfo.shapesToKill)
            {
                hpShapes.CreateShape(shape);
            }
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