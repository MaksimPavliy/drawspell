﻿using FriendsGamesTools.ECSGame;
using UnityEngine;

namespace HC
{
    public class CoreGameStub : MonoBehaviour
    {
        LevelBasedController levels => HCRoot.instance.levels;
        [SerializeField] GameObject buttonsParent;
        private void Update()
        {
            var isPlaying = levels.state == Level.State.playing;
            buttonsParent.SetActive(isPlaying);
        }
        public void OnWinPressed() => levels.DebugWin();
        public void OnLosePressed() => levels.DebugLose();
    }
}