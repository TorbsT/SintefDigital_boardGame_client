using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace View
{
    [ExecuteAlways]
    internal class SpriteSorter : MonoBehaviour
    {
        [SerializeField] private new SpriteRenderer renderer;
        [SerializeField] private float sensitivity = 0.1f;
        [SerializeField] private int baseLayer = 100;
        private void OnEnable()
        {
            if (renderer == null)
                renderer = GetComponent<SpriteRenderer>();
        }
        private void Update()
        {
            int level = baseLayer + -Mathf.FloorToInt(transform.position.y/sensitivity);
            renderer.sortingOrder = level;
        }
    }
}
