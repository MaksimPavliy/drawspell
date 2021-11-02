using Lean.Touch;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DrawSpell
{
    [RequireComponent(typeof(CharacterController))]
    public class Enemy : MonoBehaviour, IDamageable
    {
        private DrawSpellGeneralConfig config => DrawSpellGeneralConfig.instance;

        [SerializeField] private float runspeed => config.speedEnemy;
        [SerializeField] private int damageToPlayer = 1;
        [SerializeField] private List<Spell> spellsToKill;
        [SerializeField] private float additionalShapeHeight;
        [SerializeField] private float shapeOffset = 0.6f;
        private CharacterController enemyController;
        private ShapeTypeController prefabShape;
        private List<ShapeTypeController> shapesAsHp = new List<ShapeTypeController>();
        private Player player;
        private float moveForward = 1;
        private float attackAnimationDistance = 5f;
        private float moveSpeedScale = 1;

        public Player Player { set => player = value; }
        public List<Spell> SpellsToKill => spellsToKill;
        public List<ShapeTypeController> ShapesAsHp => shapesAsHp;
        public float MoveSpeedScale { set => moveSpeedScale = value; }

        public event IDamageable.OnDamage OnDamageTaken;
        public event Action EnemyKilled;

        private void Start()
        {           
            enemyController = GetComponent<CharacterController>();
            GenerateShapes();
        }

        void Update()
        {
            if (GameManager.instance.IsPlaying)
            {
                transform.forward = new Vector3(0, 0, -moveForward);

                enemyController.Move(new Vector3(0, 0, -moveForward * runspeed) * Time.deltaTime * moveSpeedScale);

                if (Vector3.Distance(player.transform.position, transform.position) < attackAnimationDistance)
                {
                    OnDamageTaken?.Invoke(this);
                    player.TakeDamage(damageToPlayer);
                    Destroy(gameObject);
                }
            }
        }

        public void TakeDamage(Spell spell)
        {
            prefabShape = spell.SpellSymbols[0].GetComponent<ShapeTypeController>();

            foreach (var shape in shapesAsHp)
            {
                if (shape.Type == prefabShape.Type)
                {
                    OnDamageTaken?.Invoke(this);
                    shapesAsHp.Remove(shape);
                    Destroy(shape.gameObject);

                    if (spellsToKill.Count <= 0)
                    {
                        player.UpdateKillCount();
                        Destroy(gameObject);
                    }
                    break;
                }
            }
        }

        private void GenerateShapes()
        {
            shapeOffset = spellsToKill.Count == 1 ? 0 : shapeOffset;
            int index = 0;

            foreach (var spell in spellsToKill)
            {
                shapesAsHp.Add(Instantiate(spell.SpellSymbols[0], new Vector3(transform.position.x + shapeOffset, spell.SpellSymbols[0].transform.position.y + additionalShapeHeight, transform.position.z),
                    Quaternion.Euler(0f, 180f, 0f), gameObject.transform).GetComponent<ShapeTypeController>());
                shapeOffset = -shapeOffset;
                index++;
            }
        }

        private void OnDestroy()
        {
            player.Enemies.Remove(this);
        }
    }
}