using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Common
{
    internal class Hoverable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public bool Interactable => Button == null || Button.interactable;
        [field: SerializeField] public HoverSystem.CursorId CursorMode { get; private set; } = HoverSystem.CursorId.NONE;
        [field: SerializeField] public int Priority { get; private set; }
        public Animator Animator { get; private set; }
        private Button Button { get; set; }
        public void OnPointerEnter(PointerEventData eventData)
            => ChangeHoverState(true);
        public void OnPointerExit(PointerEventData eventData)
            => ChangeHoverState(false);

        private void OnMouseEnter()
            => ChangeHoverState(true);
        void OnMouseExit()
            => ChangeHoverState(false);
        void OnDisable()
        {
            if (Application.isPlaying)
                ChangeHoverState(false);
        }
        void Awake()
        {
            Animator = GetComponent<Animator>();
            Button = GetComponent<Button>();
        }

        private void ChangeHoverState(bool enter)
            => HoverSystem.Instance.Hover(this, enter);
    }
}
