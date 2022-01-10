using DigitalRubyShared;
using FriendsGamesTools;
using System;
using UnityEngine;

namespace DrawSpell
{
    public class ShapeRecognizer : MonoBehaviourHasInstance<ShapeRecognizer>
    {
        [SerializeField] private FingersImageGestureHelperComponentScript recognizer;

        public event Action<Shape.ShapeType> ShapeRecognized;
        private void Start()
        {
            FingersScript.Instance.Gestures[0].StateUpdated += Player_StateUpdated;
        }


        private void Player_StateUpdated(GestureRecognizer gesture)
        {
            if (gesture.State == GestureRecognizerState.Ended)
            {
                ImageGestureImage image = recognizer.CheckForImageMatch();

                if (image != null)
                {
                    var key = recognizer.GestureImagesToKey[image];
                    switch (key)
                    {
                        case "VerticalLine":
                            ShapeRecognized?.Invoke(Shape.ShapeType.Shape_I);
                            break;
                        case "HorizontalLine":
                            ShapeRecognized?.Invoke(Shape.ShapeType.Shape_Stroke);
                            break;
                        case "DiagonalLine1":
                            ShapeRecognized?.Invoke(Shape.ShapeType.Shape_Slash);
                            break;
                        case "DiagonalLine2":
                            ShapeRecognized?.Invoke(Shape.ShapeType.Shape_CounterSlash);
                            break;
                        default:
                            break;
                    }
                }
                gesture.Reset();

            }
        }


    }

}