using UnityEngine;

namespace Common
{
    public class AnimationPresets : MonoBehaviour
    {
        public static AnimationPresets Instance { get; private set; }

        [field: SerializeField] public AnimationCurve PlayerMoveCurve { get; private set; }
            = AnimationCurve.Linear(0f, 0f, 1f, 1f);
        [field: SerializeField] public float PlayerMoveDuration { get; private set; } = 1f;
        [field: SerializeField] public AnimationCurve PackageMoveCurve { get; private set; }
            = AnimationCurve.Linear(0f, 0f, 1f, 1f);
        [field: SerializeField] public float PackageMoveDuration { get; private set; } = 0.2f;

        private void Awake()
        {
            Instance = this;
        }
    }
}