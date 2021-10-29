using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DrawSpell
{
    [RequireComponent(typeof(CharacterController))]
    public class Player : MonoBehaviour
    {
        [SerializeField] private float runspeed;
        [SerializeField] private Transform spellEffects;
        [SerializeField] private Transform playerWand;
        private DrawSpellGeneralConfig config => DrawSpellGeneralConfig.instance;
        private List<Enemy> enemies = new List<Enemy>();
        private CharacterController characterController;
        private Spell attackingSpell;
        private float moveForward = 1;
        private int hp = 5;
        private bool shieldIsActive;

        public bool ShieldIsActive { set => shieldIsActive = value; }

        public List<Enemy> Enemies { get => enemies; set => enemies = value; }

        void Start()
        {
            runspeed = 0f;

            characterController = GetComponent<CharacterController>();
        }

        public void OnPlay()
        {
            runspeed = config.speedPlayer;
        }

        public void DoWin()
        {

        }

        public void DoLoss()
        {
            Destroy(this.gameObject);
        }

        void Update()
        {
            if (GameManager.instance.IsPlaying)
            {
                transform.forward = new Vector3(0, 0, moveForward);

                characterController.Move(new Vector3(0, 0, moveForward * runspeed) * Time.deltaTime);
            }
        }

        public void Attack(LeanShapeDetector detector)
        {
            foreach (var enemy in enemies)
            {
                foreach (var spell in enemy.SpellsToKill)
                {
                    if (enemy != null && spell.SpellSymbols.Contains(detector.Shape))
                    {
                        attackingSpell = spell.GetComponent<TargetSpell>() == null ? Instantiate(spell, enemy.transform.position, Quaternion.identity, spellEffects)
                            : Instantiate(spell, playerWand.position, Quaternion.identity, spellEffects);
                        attackingSpell.Target = enemy;
                        attackingSpell.IsCasted = true;
                        enemy.SpellsToKill.Remove(spell);
                        break;
                    }
                }
            }
        }

        public void TakeDamage(int damageTaken)
        {
            if (!shieldIsActive)
            {
                hp -= damageTaken;

                if (hp <= 0)
                {
                    GameManager.instance.DoLose();
                }
            }
        }
    }
}