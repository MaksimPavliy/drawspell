using DigitalRubyShared;
using FriendsGamesTools;
using FriendsGamesTools.ECSGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DrawSpell
{
    public class DrawSpellCoreGameView : CoreGameView
    {
        [SerializeField] private FingersImageGestureHelperComponentScript recognizer;

        int _lastTouchesCount = 0;
        private void Start()
        {
           // Joystick.instance.DragEnded += Instance_DragEnded;
            
        }

        private void Instance_DragEnded()
        {
            var im=recognizer.CheckForImageMatch();
            Debug.Log(im);
        }

        private void LateUpdate()
        {
            if(_lastTouchesCount!=0 && FingersScript.Instance.CurrentTouches.Count == 0)
            {
                var im = recognizer.CheckForImageMatch();
                if (im!=null)
                {
                    Debug.Log(im.Name);
                }
            }

            _lastTouchesCount = FingersScript.Instance.CurrentTouches.Count;
        }
    }
}