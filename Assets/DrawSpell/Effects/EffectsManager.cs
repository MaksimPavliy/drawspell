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

    [Serializable]
    public class ParticleList
    {
        private List<ParticleSystem> list;

        [SerializeField]
        private ParticleSystem spellPrefab;
        [SerializeField]
        private Transform parent;

        public void Clear()
        {
            if (list == null) list = new List<ParticleSystem>();

            foreach (var spellEffect in list)
            {
                GameObject.Destroy(spellEffect.gameObject);
            }
            list.Clear();
        }
        public ParticleSystem PlayEffect(Vector3 position)
        {
            if (list == null) list = new List<ParticleSystem>();

            var effect = list.Find(x => x != null && !x.gameObject.activeSelf);
            if (!effect)
            {
                effect = GameObject.Instantiate(spellPrefab, parent);
                list.Add(effect);
            }
            effect.transform.position = position;
            effect.gameObject.SetActive(true);

            return effect;
        }

    }

    public class EffectsManager : MonoBehaviourHasInstance<EffectsManager>
    {
        [SerializeField]
        private ParticleList brokenHeart;
        [SerializeField]
        private ParticleList slashEffect;
        [SerializeField]
        private ParticleList enemySpawnEffect;
        public void PlayBrokenHeart(Vector3 position)
        {
            brokenHeart.PlayEffect(position);
        }

        public void PlaySlashEffect(Vector3 position)
        {
            slashEffect.PlayEffect(position);
        }
        public void PlayEnemySpawn(Vector3 position)
        {
            enemySpawnEffect.PlayEffect(position);
        }
    }
}
