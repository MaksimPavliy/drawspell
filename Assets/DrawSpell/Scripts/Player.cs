using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DrawSpell
{
    public class Boss : MonoBehaviour, IDamageable
    {
        public event IDamageable.OnDamage OnPlayerAttacked;
        public event IDamageable.OnDied Died;

        public float _hp = 200;

        public int HP => throw new System.NotImplementedException();

        public HPShapes Shapes => throw new System.NotImplementedException();

        public void TakeDamage(Shape.ShapeType shapeType)
        {
            _hp -= 10;
            Debug.Log($"Boss hp {_hp}");
        }

        private IEnumerator ThinkingRoutine()
        {
            yield return new WaitForSeconds(1f);
        }

        private void CastSpell()
        {

        }
    }
    [RequireComponent(typeof(CharacterController))]
    public class Player : MonoBehaviour, IDamageable
    {
        [SerializeField] private float runspeed;
        [SerializeField] private Transform playerWand;

        private DrawSpellGeneralConfig config => DrawSpellGeneralConfig.instance;
        private CharacterController characterController;
        private float moveForward = 1;
        private int hp = 1;

        public event IDamageable.OnDamage OnPlayerAttacked;
        public event IDamageable.OnDied Died;

        public virtual List<Enemy> Enemies => EnemySpawner.instance.SpawnedEnemies;

        public int HP => hp;

        public HPShapes Shapes => null;
        [SerializeField] private HPShapes shapes;

        void Start()
        {
            runspeed = 0f;

            characterController = GetComponent<CharacterController>();
            shapes.CreateShape(Shape.ShapeType.ShapeType1);
        }

        public void OnPlay()
        {
            runspeed = config.speedPlayer;
        }

        void Update()
        {
            if (GameManager.instance.IsPlaying)
            {
                transform.forward = new Vector3(0, 0, moveForward);

                characterController.Move(new Vector3(0, 0, moveForward * runspeed) * Time.deltaTime);
            }
        }


        public void Attack(Shape.ShapeType shapeType)
        {
            Debug.Log(shapeType);
            foreach (var enemy in Enemies)
            {
                foreach (var shape in enemy.Shapes.Shapes)
                {
                    if (shape.IsEnabled && shape.Type == shapeType)
                    {
                        shape.EnableShape(false);

                        var spellInfo = GameSettings.instance.GetShapeInfo(shape.Type);

                        bool isTargetSpell = spellInfo.spell is TargetSpell;

                        spellInfo.CastSpellInstance(isTargetSpell ? playerWand.position : enemy.transform.position, enemy);

                        break;
                    }
                }
            }
        }
        public void Attack(LeanShapeDetector detector)
        {
            Attack(detector.Shape.GetComponent<Shape>().Type);
          
        }

        public void TakeDamage(int damageTaken)
        {
            hp -= damageTaken;

            if (hp <= 0)
            {
                GameManager.instance.DoLose();
                /*EffectsManager.instance.;*/
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Portal"))
            {
                GameManager.instance.DoWin();
            }
        }

        public void TakeDamage(Shape.ShapeType shapeType)
        {
            hp--;
        }
    }
}