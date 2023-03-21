using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace View {
    public class PriorityPanelScript : MonoBehaviour
    {
        // Start is called before the first frame update
        public OrchestratorViewHandler handler;
        public GameObject[] iconSelections;
        public GameObject p1;
        public GameObject p2;
        public int chosenIndex;


        public void handlePrioritySelection(int index)
        {
            chosenIndex = index;
            float selectionHeight = iconSelections[index].GetComponent<RectTransform>().rect.height;
            float selectionWidth = iconSelections[index].GetComponent<RectTransform>().rect.height;
            p1.transform.position = iconSelections[index].transform.position + new Vector3(-selectionWidth/4, -selectionHeight*3/4, 0);
            p2.transform.position = iconSelections[index].transform.position + new Vector3(selectionWidth / 4, -selectionHeight * 3 / 4, 0);
            p1.SetActive(true);
            p2.SetActive(true);
        }


        private void resetPanel()
        {
            p1.SetActive(false);
            p2.SetActive(false);
        }

        public void handlePriorityInput(int value)
        {
            resetPanel();
            handler.handlePriorityInput(chosenIndex,value);
        }
    }

}
