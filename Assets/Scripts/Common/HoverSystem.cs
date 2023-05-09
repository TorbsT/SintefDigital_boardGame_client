using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Common
{
    internal class HoverSystem : MonoBehaviour
    {
        [Serializable]
        private class CursorIdTexturePair
        {
            [field: SerializeField] public CursorId Id { get; set; }
            [field: SerializeField] public Vector2 Hotspot { get; set; }
            [field: SerializeField] public Texture2D Texture { get; set; }
        } 
        public enum CursorId
        {
            NONE,
            POINTER,
            TEXT,
            CANT
        }

        public static HoverSystem Instance { get; private set; }

        [SerializeField] private List<CursorIdTexturePair> textures = new();

        [SerializeField] private List<Hoverable> hovered = new();

        public void Hover(Hoverable hoverable, bool enter)
        {
            if (enter)
            {
                hovered.Add(hoverable);
                hovered.Sort(
                    (a, b) =>
                    {
                        return b.Priority - a.Priority;
                    });
            }
            else
            {
                SetHoverOnElement(hoverable, false);
                hovered.Remove(hoverable);
            }
        }
        private void Awake()
        {
            Instance = this;
        }
        private void Update()
        {
            CursorId chosenCursor = CursorId.NONE;
            foreach (Hoverable hoverable in hovered)
            {
                bool canHover = hoverable.Interactable;
                SetHoverOnElement(hoverable, canHover);
                if (!canHover)
                    chosenCursor = CursorId.CANT;
                else
                    chosenCursor = hoverable.CursorMode;
                break;
            }
            CursorIdTexturePair tryMatch = textures.Find(match => match.Id == chosenCursor);
            Texture2D texture;
            Vector2 hotspot;
            if (tryMatch == null)
            {
                texture = null;
                hotspot = Vector2.zero;
                Debug.LogWarning($"There is no texture for {chosenCursor}");
            } else
            {
                texture = tryMatch.Texture;
                hotspot = tryMatch.Hotspot;
            }
            Cursor.SetCursor(texture, hotspot, CursorMode.Auto);
        }
        private void SetHoverOnElement(Hoverable element, bool enter)
        {
            if (element.Animator != null)
                element.Animator.SetBool("hovered", enter);
        }
    }
}
