using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    [ExecuteAlways]
    public class ColorSchemeSystem : MonoBehaviour
    {
        public enum Scheme
        {
            PrimaryBox,
            PrimaryDropshadow,
            PrimaryBackground,
            SecondaryBox,
            SecondaryDropshadow,
            SecondaryBackground,
            TertiaryBox,
            TertiaryDropshadow,
            TertiaryBackground,
        }
        [Serializable]
        private class SchemeColorPair
        {
            public Scheme scheme;
            public Color color = Color.red;
        }
        public static ColorSchemeSystem Instance { get; private set; }
        public event Action ColorChanged;
        [SerializeField] private List<SchemeColorPair> pairs = new();
        
        public Color GetColor(Scheme scheme)
        {
            foreach (SchemeColorPair pair in pairs)
                if (pair.scheme == scheme)
                    return pair.color;
            Debug.LogWarning("Couldn't find a color for " + scheme);
            return Color.magenta;
        }
        private void Awake()
        {
            Instance = this;
        }
        private void OnEnable()
        {
            Instance = this;
        }
        private void OnValidate()
        {
            ColorChanged?.Invoke();
        }
    }
}
