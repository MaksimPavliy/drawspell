using DG.Tweening;
using DigitalRubyShared;
using FriendsGamesTools;
using System;
using UnityEngine;

namespace DrawSpell
{
    public class ShapeRecognizer : MonoBehaviourHasInstance<ShapeRecognizer>
    {
        [SerializeField] private FingersImageGestureHelperComponentScript recognizer;
        public LineRenderer line;

        public event Action<Shape.ShapeType, LineRenderer> ShapeRecognized;
        [SerializeField] private Material correctMaterial;
        private void Start()
        {
            FingersScript.Instance.Gestures[0].StateUpdated += Player_StateUpdated;
        }


        private LineRenderer GetClone()
        {
            var newLine = Instantiate(line, line.transform.parent);
            float duration = 1f;
            var pos = newLine.transform.position;

            newLine.transform.DOLocalMove(pos + new Vector3(-0.2f, 0, 5f), duration * 0.8f);
            newLine.transform.DOScale(2f, duration).OnComplete(() =>
            {
            });
            return newLine;
        }
        public void ClaimLineRendererAsCorrect(LineRenderer line, bool correct)
        {
            if (correct)
            {
                line.startColor = Color.green;
                line.endColor = Color.green;
                line.widthMultiplier *= 2f;
            }
            else
            {
                line.startColor = Color.red;
                line.endColor = Color.red;
            }

            
            var startColor = new Color2(line.startColor,line.startColor);
            var endColor = new Color2(line.startColor, line.startColor);
            endColor.ca.a = 0;
            endColor.cb.a = 0;

            line.DOColor(startColor, endColor, 1f).OnComplete(() =>
            {
                Destroy(line.gameObject);
            });
            //
        }
        private void Player_StateUpdated(GestureRecognizer gesture)
        {
            if (gesture.State == GestureRecognizerState.Ended)
            {
                ImageGestureImage image = recognizer.CheckForImageMatch();
                var lineClone = GetClone();
                recognizer.ClearLineRenderers();
                if (image != null)
                {
                    var key = recognizer.GestureImagesToKey[image];
                    switch (key)
                    {
                        case "VerticalLine":
                            ShapeRecognized?.Invoke(Shape.ShapeType.Shape_I, lineClone);
                            break;
                        case "HorizontalLine":
                            ShapeRecognized?.Invoke(Shape.ShapeType.Shape_Stroke, lineClone);
                            break;
                        case "DiagonalLine1":
                            ShapeRecognized?.Invoke(Shape.ShapeType.Shape_Slash, lineClone);
                            break;
                        case "DiagonalLine2":
                            ShapeRecognized?.Invoke(Shape.ShapeType.Shape_CounterSlash, lineClone);
                            break;
                        case "Triangle":
                            ShapeRecognized?.Invoke(Shape.ShapeType.Shape_Triangle, lineClone);
                            break;
                        case "Circle":
                            ShapeRecognized?.Invoke(Shape.ShapeType.Shape_O, lineClone);
                            break;
                        case "LightningBolt":
                            ShapeRecognized?.Invoke(Shape.ShapeType.Shape_Lightning, lineClone);
                            break;
                        case "LetterV":
                            ShapeRecognized?.Invoke(Shape.ShapeType.Shape_V, lineClone);
                            break;

                        default:
                            ClaimLineRendererAsCorrect(lineClone, false);
                            break;
                    }
                }
                else
                {
                    ClaimLineRendererAsCorrect(lineClone, false);
                }
                gesture.Reset();

            }
        }

    }

}