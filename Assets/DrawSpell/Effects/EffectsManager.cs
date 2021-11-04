using FriendsGamesTools;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DrawSpell
{
    [Serializable]
    public class EffectsList
    {
        private List<Spell> list;

        [SerializeField]
        private Spell spellPrefab;
        [SerializeField]
        private Transform parent;

        public Spell SpellPrefab => spellPrefab;

        public void Clear()
        {
            if (list == null) list = new List<Spell>();

            foreach (var spellEffect in list)
            {
                GameObject.Destroy(spellEffect.gameObject);
            }
            list.Clear();
        }
        public Spell PlayEffect(Vector3 position, Enemy enemy)
        {
            if (list == null) list = new List<Spell>();

            var spellEffect = list.Find(x => x != null && !x.gameObject.activeSelf);
            if (!spellEffect)
            {
                spellEffect = GameObject.Instantiate(spellPrefab, parent);
                list.Add(spellEffect);
            }
            spellEffect.transform.position = position;
            spellEffect.DealedDamage = false;
            spellEffect.Target = enemy;
            spellEffect.IsCasted = true;
            spellEffect.gameObject.SetActive(true);

            return spellEffect;
        }

    }

    public class EffectsManager : MonoBehaviourHasInstance<EffectsManager>
    {
        [SerializeField]
        private List<EffectsList> spellPrefabs;

        public EffectsList FindSpellEffect(Spell spell)
        {
            return spellPrefabs.Find(x => x.SpellPrefab == spell);
        }
    }
}
