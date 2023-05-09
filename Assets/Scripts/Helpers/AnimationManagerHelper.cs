using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TorbuTils.Anime;
using Common;

namespace Helpers
{
    public class AnimationManagerHelper : Main, IAnimationManager
    {
        public int StartAnimation<T>(Animation<T> anim)
        {
            Anim<T> a = new Anim<T>
            {
                StartValue = anim.StartValue,
                EndValue = anim.EndValue,
                Curve = anim.Curve,
                Duration = anim.Duration,
                Action = anim.Action
            };
            if (anim.Loop == AnimationLoopMode.Reset)
                a.Loop = LoopMode.Reset;
            if (anim.Loop == AnimationLoopMode.InvertSameCurve)
                a.Loop = LoopMode.InvertSameCurve;
            if (anim.Loop == AnimationLoopMode.InvertMirrorCurve)
                a.Loop = LoopMode.InvertMirrorCurve;
            int? tryId = Begin(a);
            if (!tryId.HasValue)
            {
                Debug.LogWarning($"Error starting animation of type {typeof(T)}:" +
                    $" there is no animation controller for that type in scene");
                return -1;
            }
            return tryId.Value;
        }
        public bool StopAnimation<T>(int id, float? stopAtTime = null)
        {
            return Stop<T>(id, stopAtTime);
        }

        private void Awake()
        {
            AnimationManager.Instance = this;
        }
    }
}

