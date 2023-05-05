using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using Network;

namespace View 
{
    public class GameCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [field: SerializeField] public TextMeshProUGUI Id { get; private set; }
        [field: SerializeField] public TextMeshProUGUI Title { get; private set; }
        [field: SerializeField] public TextMeshProUGUI Description { get; private set; }
        [field: SerializeField] public TextMeshProUGUI Traffic { get; private set; }
        [field: SerializeField] public TextMeshProUGUI Goal { get; private set; }
        [field: SerializeField] public Animator Animator { get; private set; }
        public int id;

        public NetworkData.SituationCard Source { get; set; }

        public int GetId() { return id; }
        public void Clicked()
        {
            GameCardController.Instance.Click(this);
        }
        public void moveTo(Vector3 newPos) //TODO write with vector3 instead
        {
            return;
            this.transform.position = newPos;
        }

        private void Awake()
        {
            Animator = GetComponent<Animator>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Animator.SetBool("hover", true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Animator.SetBool("hover", false);
        }
    }
}
