using System.Collections.Generic;
using UnityEngine;

namespace View
{
    internal class UIListHandler : MonoBehaviour
    {
        [SerializeField] private RectTransform content;
        private List<GameObject> items = new();
        public void AddItem(GameObject gameObject)
        {
            RectTransform newLobbyTransform = gameObject.GetComponent<RectTransform>();
            newLobbyTransform.SetParent(content);
            newLobbyTransform.anchoredPosition =
                Vector2.down * content.sizeDelta;
            newLobbyTransform.sizeDelta = new(1f, newLobbyTransform.sizeDelta.y);
            content.sizeDelta +=
                Vector2.up * newLobbyTransform.sizeDelta.y;
            items.Add(gameObject);
        }
        public void Clear()
        {
            foreach (GameObject gameObject in items)
                PoolManager.Instance.Enpool(gameObject);
            items = new();
            content.sizeDelta = new(content.sizeDelta.x, 0f);
        }
    }
}
