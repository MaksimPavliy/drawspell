using FriendsGamesTools.DebugTools;
using HcUtils;
using UnityEngine;
using UnityEngine.UI;

namespace DrawSpell
{
    public class DrawSpellDebugPanelTab : DebugPanelItemView
    {
        [SerializeField] Text cameraIndex;
        [SerializeField] Button[] skinSetButtons;
        public override (string tab, string name) whereToShow => ("HC","HC");

        protected override void AwakePlaying()
        {
            base.AwakePlaying();

            for (int i = 0; i < skinSetButtons.Length; i++)
            {
                int index = i;
                skinSetButtons[i].onClick.AddListener(delegate { SetSkinSet(index); });
            }

        }
        private void Start()
        {
            ChangeCameraIndex(DrawSpellGeneralConfig.instance.CameraIndex);
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            for (int i = 0; i < skinSetButtons.Length; i++)
            {
                SetButtonSelected(skinSetButtons[i], i == ThemeSet.activeSet);
            }

        }
        public void SetSkinSet(int index)
        {
            ThemeSet.ThemeSetChanged?.Invoke(index);

            for (int i = 0; i < skinSetButtons.Length; i++)
            {
                SetButtonSelected(skinSetButtons[i], i == index);
            }

        }
        void SetButtonSelected(Button button, bool selected)
        {
            var colors = button.colors;

            colors.normalColor = selected ? Color.green : Color.white;
            colors.selectedColor = selected ? Color.green : Color.white;
            button.colors = colors;
        }

        public void SetButtonColor(Button button, bool active)
        {
            button.image.color = active ? Color.green : Color.white;
        }
        public void ChangeCameraIndex(int index)
        {
            int camerasCount = 5;

            var newInd = (DrawSpellGeneralConfig.instance.CameraIndex + Mathf.Clamp(index, -1, 1) + camerasCount) % camerasCount;

            DrawSpellGeneralConfig.instance.CameraIndex = newInd;
            cameraIndex.text = newInd.ToString();
            
            if (!CameraSwitch.instance) return;
            
            CameraSwitch.instance.ActivateCamera(DrawSpellGeneralConfig.instance.CameraIndex);
        }

    }
}