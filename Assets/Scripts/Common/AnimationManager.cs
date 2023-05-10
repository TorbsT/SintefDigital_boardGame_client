using System;
using UnityEngine;

namespace Common
{
    public static class AnimationManager
    {
        public static IAnimationManager Instance { get; set; }
    }
    public class Animation<T>
    {
        public Action<T> Action { get; set; }
        public T StartValue { get; set; } = default;
        public T EndValue { get; set; } = default;
        public float Duration { get; set; } = 1f;
        public AnimationCurve Curve { get; set; } = AnimationCurve.Linear(0f, 0f, 1f, 1f);
        public AnimationLoopMode Loop { get; set; } = AnimationLoopMode.None;
        public int Id { get; private set; } = -1;

        public void Start()
        {
            Id = AnimationManager.Instance.StartAnimation(this);
        }
        public void Stop()
        {
            AnimationManager.Instance.StopAnimation<T>(Id);
        }
    }
    public interface IAnimationManager
    {
        int StartAnimation<T>(Animation<T> anim);
        bool StopAnimation<T>(int id, float? stopAtTime = null);
    }
    public enum AnimationLoopMode
    {
        None,
        Reset,
        InvertSameCurve,
        InvertMirrorCurve
    }
}
