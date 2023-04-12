using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace View
{
    public class AnimationPresets : MonoBehaviour
    {
        public static AnimationPresets Instance { get; private set; }

        [field: SerializeField] public AnimationCurve PlayerMoveCurve { get; private set; }
            = AnimationCurve.Linear(0f, 0f, 1f, 1f);
        [field: SerializeField] public float PlayerMoveDuration { get; private set; } = 1f;

        private void Awake()
        {
            Instance = this;
        }
    }
}