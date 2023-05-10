using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Common
{
    public class UIListHandler : MonoBehaviour
    {
        [SerializeField] private RectTransform content;
        private List<GameObject> items = new();
        private VerticalLayoutGroup layoutGroup;

        private void OnEnable()
        {
            layoutGroup = content.GetComponent<VerticalLayoutGroup>();
        }
        public void AddItem(GameObject gameObject)
        {
            RectTransform newLobbyTransform = gameObject.GetComponent<RectTransform>();
            newLobbyTransform.SetParent(content);
            newLobbyTransform.anchoredPosition =
                Vector2.down * content.sizeDelta;
            newLobbyTransform.sizeDelta = new(1f, newLobbyTransform.sizeDelta.y);
            content.sizeDelta +=
                Vector2.up * (newLobbyTransform.sizeDelta.y+layoutGroup.spacing);
            items.Add(gameObject);
        }
        public void Clear()
        {
            if (layoutGroup == null) layoutGroup = content.GetComponent<VerticalLayoutGroup>();
            foreach (GameObject go in items)
            {
                go.transform.SetParent(null);
                go.SetActive(false);
                PoolManager.Instance.Enpool(go);
            }
                
            items = new();
            content.sizeDelta = new(content.sizeDelta.x, layoutGroup.padding.vertical-layoutGroup.spacing);
        }
    }
}
