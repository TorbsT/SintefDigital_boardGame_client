using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace View
{
    [ExecuteAlways]
    public class ScaleWithScreenSize : MonoBehaviour
    {
        [SerializeField] private Vector2 originalScale = Vector2.one;
        [SerializeField] private Vector2 originalScreensize = new(1280f, 720f);
        [SerializeField] private Vector2 originalPosition = Vector2.zero;

        private new RectTransform transform;
        private Vector2 prevScreensize;

        public void ResetOriginalDimensions()
        {
            originalScale = transform.localScale;
            //originalScreensize = new(Screen.width, Screen.height);
            originalPosition = transform.anchoredPosition;
        }
        private void Awake()
        {
            transform = GetComponent<RectTransform>();
        }
        private void Update()
        {
            Refresh();
        }
        private void Refresh()
        {
            float screenX = Screen.width;
            float screenY = Screen.height;
            Vector2 screensize = new(screenX, screenY);
            if (screensize != prevScreensize)
            {
                float relativeX = screenX / originalScreensize.x;
                float relativeY = screenY / originalScreensize.y;
                float scaleX = relativeX * originalScale.x;
                float scaleY = relativeY * originalScale.y;
                float finalScale = Mathf.Min(scaleX, scaleY);
                transform.anchoredPosition = originalPosition * finalScale;
                transform.localScale = Vector3.one * finalScale;
                prevScreensize = screensize;
            }
        }
    }

}