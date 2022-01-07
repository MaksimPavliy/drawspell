using DG.Tweening;
using Lean.Touch;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DrawSpell
{
    [RequireComponent(typeof(CharacterController))]
    public class Player : MonoBehaviour, IDamageable, ISpellTarget
    {
        [SerializeField] private float runspeed;
        [SerializeField] private Transform playerWand;

        private DrawSpellGeneralConfig config => DrawSpellGeneralConfig.instance;
        private CharacterController characterController;
        private float moveForward = 1;
        private int hp = 5;

        public event OnDamage OnPlayerAttacked;
        public event OnDied Died;
        public event OnDamage SpellCasted;

        private List<ISpellTarget> _spellTargets = new List<ISpellTarget>();
        public int HP => hp;

        public SpellShapes Shapes => null;

        public Transform Transform => transform;

        [SerializeField] private Transform _shapeDetectors;

        private bool _isWalking = false;

        [SerializeField] private HPView hpView;
        [SerializeField] private Animator animator;

        public event Action LevelPassed;
        private bool _isWin = false;
        void Start()
        {
            runspeed = 0f;

            characterController = GetComponent<CharacterController>();

            EnemySpawner.instance.EnemySpawned += Instance_EnemySpawned;

            var shapeDetectors = _shapeDetectors.GetComponentsInChildren<LeanShapeDetector>();
            for (int i = 0; i < shapeDetectors.Length; i++)
            {
                var detector = shapeDetectors[i];
                detector.OnDetected.AddListener((LeanFinger finger) => Attack(detector));
            }
            hpView.InitHP(HP);
        }

        private void Instance_EnemySpawned(Enemy obj)
        {
            _spellTargets.Add(obj);
            obj.Died += (IDamageable damageable) => _spellTargets.Remove(damageable as ISpellTarget);
        }


        public void Walk() => _isWalking = true;
        public void Stop() => _isWalking = false;
        public void OnPlay()
        {
            runspeed = config.speedPlayer;
            Walk();
        }

        void Update()
        {
            if (_isWalking)
            {
                transform.forward = new Vector3(0, 0, moveForward);

                characterController.Move(new Vector3(0, 0, moveForward * runspeed) * Time.deltaTime);
            }
        }


        public void Attack(Shape.ShapeType shapeType)
        {
            animator.SetTrigger("Attack");
            StartCoroutine(CastRoutine(shapeType));
         
        }
        private IEnumerator CastRoutine(Shape.ShapeType shapeType)
        {
            yield return new WaitForSeconds(0.4f);

            foreach (var target in _spellTargets)
            {
                foreach (var shape in target.Shapes.Shapes)
                {
                    if (shape.IsEnabled && shape.Type == shapeType)
                    {
                        shape.EnableShape(false);

                        var spellInfo = GameSettings.instance.GetShapeInfo(shape.Type);

                        bool isTargetSpell = spellInfo.spell is TargetSpell;

                        spellInfo.CastSpellInstance(isTargetSpell ? playerWand.position : (target as IDamageable).Transform.position, target);

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
            if (_isWin) return;
            hpView.TakeDamage();
            hp -= damageTaken;

            if (hp <= 0)
            {
                Died?.Invoke(this);
                GameManager.instance.DoLose();
                animator.SetTrigger("Die");
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Portal"))
            {
                GameManager.instance.DoWin();
                _isWalking = false;
                float duration = 1;
                transform.DOMove(other.transform.position + Vector3.up * 3f + Vector3.forward * 0.5f, duration).OnComplete(() => GameManager.instance.DoWin());
                transform.DOScale(0, duration);
                transform.DORotate(new Vector3(0, 180, 0), duration / 10f).SetLoops(-1, LoopType.Incremental);
            } else if (other.CompareTag("BossArea"))
            {
                _isWalking = false;
                _spellTargets.Clear();

                if (Boss.instance)
                {

                    _spellTargets.Add(Boss.instance);
                    Boss.instance.SetSpellTarget(this);
                    Boss.instance.Died += (IDamageable damageable) =>
                     {
                         _spellTargets.Remove(damageable as ISpellTarget);
                         Win();
                     };
                }
                else
                {
                    Win();
                }
            }
        }

        private void OnDestroy()
        {
            transform.DOComplete();
        }
        private void Win()
        {
            StartCoroutine(BossDiedRoutine());
            _isWin = true;
            LevelPassed?.Invoke();
        }

        private IEnumerator BossDiedRoutine()
        {
            yield return new WaitForSeconds(1.5f);
            _isWalking = true;
        }

        public void TakeDamage(Shape.ShapeType shapeType)
        {
            TakeDamage(1);
        }
    }
}