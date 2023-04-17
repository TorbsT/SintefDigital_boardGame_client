using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace View
{
    public abstract class PanelScript : MonoBehaviour
    {
        public OrchestratorViewHandler handler;
        public GameObject[] iconSelections;
        public GameObject selectionMarker;
        public int? highlightedIndex;
        protected int chosenIndex;
        protected RegionCard activeRegion;

        public abstract void handleInput();


        internal virtual void Awake()
        {
            for (int i = 0; i < iconSelections.Length; i++)
            {
                int index = i;
                GameObject icon = iconSelections[index];
                addPointerEnterTrigger(icon,index);
            }
            addPointerExitTrigger(selectionMarker);
            selectionMarker.GetComponent<Button>().onClick.AddListener(() => handleInput());
        }



        private void addPointerEnterTrigger(GameObject icon,int index)
        {
            EventTrigger trigger = icon.GetComponent<EventTrigger>();
            if (trigger == null)
            {
                trigger = icon.AddComponent<EventTrigger>();
            }
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerEnter;
            entry.callback.AddListener((data) => { mouseIn((PointerEventData)data, index); });
            trigger.triggers.Add(entry);
        }

        
        private void addPointerExitTrigger(GameObject icon)
        {
            EventTrigger trigger = icon.GetComponent<EventTrigger>();
            if (trigger == null)
            {
                trigger = icon.AddComponent<EventTrigger>();
            }
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerExit;
            entry.callback.AddListener((data) => { mouseOut((PointerEventData)data); });
            trigger.triggers.Add(entry);
        }
        private void highlightIcon(int index)
        {
            highlightedIndex = index;
            selectionMarker.SetActive(true);
            selectionMarker.transform.position = iconSelections[index].transform.position;
        }

        private void mouseOut(PointerEventData data)
        {
            highlightedIndex = null;
            selectionMarker.SetActive(false);
        }

        private void mouseIn(PointerEventData data,int index)
        {
            if (highlightedIndex != null) return;
            highlightIcon(index);
        }

        public void showPanel(RegionCard regionCard)
        {
            activeRegion = regionCard;
            gameObject.SetActive(true);
            setColorOfPanel(regionCard.getMaterial());

        }

        public void hidePanel()
        {
            gameObject.SetActive(false);
            resetPanels();
        }

        private void setColorOfPanel(Material mat)
        {
            gameObject.GetComponent<Image>().color = mat.GetColor("_Color");
        }

        protected void resetPanels()
        {
            activeRegion = null;
            selectionMarker.SetActive(false);
            highlightedIndex = null;
        }


        
    }

}
