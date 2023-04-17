using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace View {
    public class PriorityPanelScript : PanelScript
    {
  
        public GameObject p1;
        public GameObject p2;
     

        internal override void Awake()
        {
            base.Awake();
            p1.GetComponent<Button>().onClick.AddListener(() => handleValue(1));
            p2.GetComponent<Button>().onClick.AddListener(() => handleValue(2));
        }


        public override void handleInput() //Called from onclick listerner on icons
        {
            chosenIndex = (int) highlightedIndex;
            float selectionHeight = iconSelections[chosenIndex].GetComponent<RectTransform>().rect.height;
            float selectionWidth = iconSelections[chosenIndex].GetComponent<RectTransform>().rect.height;
            p1.transform.position = iconSelections[chosenIndex].transform.position + new Vector3(-selectionWidth/4, -selectionHeight*3/4, 0);
            p2.transform.position = iconSelections[chosenIndex].transform.position + new Vector3(selectionWidth / 4, -selectionHeight * 3 / 4, 0);
            p1.SetActive(true);
            p2.SetActive(true);
            
        }

        private void setOnclickListener(GameObject icon, int value)
        {
            icon.GetComponent<Button>().onClick.AddListener(() => handleValue(value));
        }

        private void addOnclickHandler(int id)
        {

        }

        public void handleValue(int value) //Called from onclick listerners on p1/p2
        {
            p1.SetActive(false);
            p2.SetActive(false);
            activeRegion.setPriority(chosenIndex, value, isOrchestrator());
            hidePanel();
        }


    }

}
