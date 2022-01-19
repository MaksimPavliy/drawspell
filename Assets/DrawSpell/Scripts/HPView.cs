using System.Collections.Generic;
using UnityEngine;

namespace DrawSpell
{
    public class HPView :MonoBehaviour
    {

        [SerializeField] private Transform heartPrefab;
        private List<Transform> hearts=new List<Transform>();

        private float _distance = 0.32f;
       public void InitHP(int HP)
        {
            foreach(var heart in hearts)
            {
                Destroy(heart.gameObject);

            }
            hearts.Clear();
            for (int i = 0; i < HP; i++)
            {
                var h = Instantiate(heartPrefab, transform);
                h.localPosition = Vector3.up* i * _distance;
                hearts.Add(h);
            }
        }

        public void TakeDamage()
        {
            if(hearts.Count==0) return;

            var lastHeart = hearts[hearts.Count - 1];
            hearts.Remove(lastHeart);
            Destroy(lastHeart.gameObject);
            EffectsManager.instance.PlayBrokenHeart(transform.position+Vector3.up*3);
        }
    }
}