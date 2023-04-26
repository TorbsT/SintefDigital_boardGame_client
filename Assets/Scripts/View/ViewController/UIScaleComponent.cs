using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace View
{
    internal class UIScaleComponent : MonoBehaviour
    {
        public enum Direction
        {
            Vertical,
            Horizontal
        }
        [field: SerializeField] public Direction ExpandDirection { get; private set; }
        [field: SerializeField, Range(0f, 2f)] public float ExpandScalar = 1f;
        private RectTransform rectTransform;
        private Vector2 originalSize;

        private void OnEnable()
        {
            rectTransform = GetComponent<RectTransform>();
            originalSize = rectTransform.sizeDelta;
            Debug.Log(originalSize);
        }

    }
}
