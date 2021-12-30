using UnityEngine;
using static Shape;

namespace DrawSpell
{
    [RequireComponent(typeof(CharacterController))]
    public class Enemy : MonoBehaviour, IDamageable, ISpellTarget
    {
        private DrawSpellGeneralConfig config => DrawSpellGeneralConfig.instance;

        [SerializeField] private float runspeed => config.speedEnemy;
        [SerializeField] private SpellShapes hpShapes;
        [SerializeField] private int damageToPlayer = 1;
        [SerializeField] private EnemyType enemyType;

        private CharacterController enemyController;
        private Player player;
        private float moveForward = 1;
        private float attackAnimationDistance = 5f;

        public EnemyType EnemyType => enemyType;
        public Player Player { set => player = value; }

        public int HP => hpShapes.Shapes.Count;

        public SpellShapes Shapes => hpShapes;

        public Transform Transform => transform;

        public event OnDamage OnPlayerAttacked;
        public event OnDied Died;
        public event OnDamage SpellCasted;

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
                    EffectsManager.instance.playSlashEffect(player.transform.position + Vector3.up * 1.5f);
                    player.TakeDamage(damageToPlayer);
                    gameObject.SetActive(false);
                }
            }
        }


        public void TakeDamage(ShapeType shapeType)
        {
            hpShapes.DisposeShape(shapeType);
            TakeDamage(1);
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

        public void TakeDamage(int damage)
        {
            if (HP <= 0)
            {
                Died?.Invoke(this);
                gameObject.SetActive(false);
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