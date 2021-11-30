using FriendsGamesTools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using static Shape;

namespace DrawSpell
{
    [Serializable]
    public class ShapesInfo
    {
        public ShapeType type;
        public Shape shape;
        public Spell spell;
        private IObjectPool<Spell> _pool;
        public IObjectPool<Spell> Pool
        {
            get
            {
                if (_pool == null) Init();
                return _pool;
            }
        }
        private void Init()
        {
            _pool = new ObjectPool<Spell>(CreatePooledItem, OnTakeFromPool, null, OnDestroyPoolObject);
        }

        Spell CreatePooledItem()
        {
            var instance = GameObject.Instantiate(spell);
            instance.Disposed += OnReturnedToPool;
            return instance;
        }

        void OnReturnedToPool(Spell spell)
        {
        }
        void OnDestroyPoolObject(Spell spell)
        {
            GameObject.Destroy(spell);
        }
        void OnTakeFromPool(Spell spell)
        {
            spell.DealedDamage = false;
            spell.IsCasted = true;
            spell.gameObject.SetActive(true);
        }

        public Spell CastSpellInstance(Vector3 position, Enemy enemy)
        {
            var spell=Pool.Get();
            spell.transform.position = position;
            spell.Target = enemy;
            return spell;
        }
    }
    [Serializable]
    public class EnemyInfo
    {
        public EnemyType type;
        public List<ShapeType> shapesToKill;
    }
    public class GameSettings : SettingsScriptable<GameSettings>
    {
        public List<EnemyInfo> enemies;
        public List<ShapesInfo> shapes;

        public EnemyInfo GetEnemyInfo(EnemyType type) => enemies.Find(x => x.type == type);
        public ShapesInfo GetShapeInfo(ShapeType type) => shapes.Find(x => x.type == type);
    }
}