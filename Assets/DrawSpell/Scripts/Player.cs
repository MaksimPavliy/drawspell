using Lean.Touch;
using System.Collections.Generic;
using UnityEngine;

namespace DrawSpell
{
    [RequireComponent(typeof(CharacterController))]
    public class Player : MonoBehaviour
    {
        [SerializeField] private float runspeed;
        [SerializeField] private Transform playerWand;

        private DrawSpellGeneralConfig config => DrawSpellGeneralConfig.instance;
        private int levelCompletionKillCount => config.levelCompletionKillCount;
        private List<Enemy> enemies = new List<Enemy>();
        private CharacterController characterController;
        private float moveForward = 1;
        private int hp = 200;
        private bool shieldIsActive;
        private int killCount;

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
                foreach (var shape in enemy.Shapes)
                {
                    if (shape.IsEnabled && shape.Type == detector.Shape.GetComponent<Shape>().Type)
                    {
                        shape.EnableShape(false);
                        Spell spell = enemy.SpellsToKill.Find(spell => shape.Type == spell.Shape.Type);
                        spell.gameObject.SetActive(false);

                        if (spell.GetComponent<TargetSpell>() == null)
                        {
                            spell = EffectsManager.instance.FindSpellEffect(spell).PlayEffect(enemy.transform.position, enemy);
                        }
                        else
                        {
                            spell = EffectsManager.instance.FindSpellEffect(spell).PlayEffect(playerWand.position, enemy);
                        }

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
                    /*EffectsManager.instance.;*/
                }
            }
        }

        public void UpdateKillCount()
        {
            killCount++;

            if (killCount >= levelCompletionKillCount)
            {
                GameManager.instance.DoWin();
                /*EffectsManager.instance.;*/
            }
        }
    }
}