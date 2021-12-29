using System.Collections.Generic;
using UnityEngine;

namespace DrawSpell
{
    public class PlayerAbilities : MonoBehaviour
    {
        [SerializeField] private GameObject shield;
        [SerializeField] private GameObject slow;
        [SerializeField] private Player player;

        private GameObject abilityInstance;
        private AbilityType abilityType;
        private List<GameObject> AbilityEffects = new List<GameObject>();
        private float abilityDuration;
        private float abilityCooldown;
        private bool abilityIsUsed;

        public enum AbilityType
        {
            shield = 0,
            slow = 1,
        }

        private void Update()
        {
            if (abilityIsUsed)
            {
                abilityDuration -= Time.deltaTime;

                if (abilityDuration <= 0)
                {
                    EndAbility(abilityType);

                    foreach (var abilityEffect in AbilityEffects)
                    {
                        Destroy(abilityEffect);
                    }

                    abilityIsUsed = false;
                }
            }
            else if (abilityCooldown > 0)
            {
                abilityCooldown -= Time.deltaTime;
            }
        }

        public void OnAbilityPressed(int value)
        {
            if (abilityCooldown <= 0)
            {
                switch ((AbilityType)value)
                {
                    case AbilityType.shield:
                        abilityInstance = Instantiate(shield, transform.position, Quaternion.identity, player.gameObject.transform);
                        abilityType = AbilityType.shield;
                        AbilityEffects.Add(abilityInstance);
                     //   player.ShieldIsActive = true;
                        UseAbility(5, 10);
                        break;

                    case AbilityType.slow:
                        foreach (var enemy in player.Enemies)
                        {
                            abilityInstance = Instantiate(slow, enemy.transform.position, Quaternion.identity, enemy.gameObject.transform);
                            AbilityEffects.Add(abilityInstance);
                        }
                        abilityType = AbilityType.slow;
                        UseAbility(3, 6);
                        break;
                }
            }
        }

        private void UseAbility(float duration, float cooldown)
        {
            abilityDuration = duration;
            abilityCooldown = cooldown;
            abilityIsUsed = true;
        }

        private void EndAbility(AbilityType abilityType)
        {
            if (abilityType == AbilityType.shield)
            {
             //   player.ShieldIsActive = false;
            }
        }
    }
}