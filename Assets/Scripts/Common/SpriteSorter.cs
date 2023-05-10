using UnityEngine;

namespace Common
{
    [ExecuteAlways]
    public class SpriteSorter : MonoBehaviour
    {
        [SerializeField] private new SpriteRenderer renderer;
        [SerializeField] private float sensitivity = 0.1f;
        [SerializeField] private int baseLayer = 100;

        public int HighlightLayer { get; set; }

        private void OnEnable()
        {
            if (renderer == null)
                renderer = GetComponent<SpriteRenderer>();
        }
        private void Update()
        {
            int level = baseLayer - Mathf.FloorToInt(transform.position.y/sensitivity) + HighlightLayer;
            renderer.sortingOrder = level;
        }
    }
}
