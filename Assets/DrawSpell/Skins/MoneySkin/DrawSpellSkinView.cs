using System.Collections.Generic;
using UnityEngine;

namespace DrawSpell
{
    public class DrawSpellSkinView : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> skins;
      public void SetActiveSkin(int index)
        {
            for (int i = 0; i < skins.Count; i++)
            {
                skins[i].SetActive(i == index);
            }
        }
        private void Start()
        {
            DrawSpellSelectorView.AddSkin(this);
        }
    }
}