using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace View
{
    [ExecuteAlways]
    internal class UIScaleComponent : MonoBehaviour
    {
        public enum Direction
        {
            Vertical,
            Horizontal
        }
        [field: SerializeField] public Direction ExpandDirection { get; private set; }
        [field: SerializeField] public bool UseScale { get; private set; } = true;
        [field: SerializeField, Range(0f, 2f)] public float ExpandScalar = 1f;
        private RectTransform rectTransform;

        private Vector2 originalScale;
        private Vector2 originalSize;
        private Vector2 originalScreenSize;

        private void OnEnable()
        {
            rectTransform = GetComponent<RectTransform>();
            originalSize = rectTransform.sizeDelta;
            originalScale = new(rectTransform.localScale.x, rectTransform.localScale.y);
            originalScreenSize = new(Screen.width, Screen.height);
        }
        private void Update()
        {
            float scale;
            if (ExpandDirection == Direction.Vertical)
                scale = Screen.height / originalScreenSize.y;
            else scale = Screen.width / originalScreenSize.x;
            scale = Mathf.Pow(scale, ExpandScalar);

            if (UseScale)
                rectTransform.localScale = originalScale * scale;
            else
                rectTransform.sizeDelta = originalSize * scale;
        }
    }
}
