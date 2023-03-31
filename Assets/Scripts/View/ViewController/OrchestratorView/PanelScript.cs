using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace View {
    public class PanelScript : MonoBehaviour
    {
        // Start is called before the first frame update
        public OrchestratorViewHandler handler;
        public GameObject[] iconSelections;
        public GameObject selectionMarker;
        public int? highlightedIndex;
        public int chosenIndex;


      


        private void resetPanels()
        {
            selectionMarker.SetActive(false);
            highlightedIndex = null;
        }

        public void handleAccessInput()
        {
            int selectedIndex = (int) highlightedIndex;
            resetPanels();
            handler.handleAcessInput(selectedIndex);
        }

        public void handleTollInput()
        {
            int selectedToll = (int)highlightedIndex + 1;
            resetPanels();
            handler.handleTollInput(selectedToll);
        }

        public void highlightIcon(int index)
        {
            highlightedIndex = index;
            selectionMarker.SetActive(true);
            Debug.Log(index);
            selectionMarker.transform.position = iconSelections[index].transform.position;
        }

        public void mouseOut()
        {

            highlightedIndex = null;
            selectionMarker.SetActive(false);
        }

        public void mouseIn(int index)
        {
            if (highlightedIndex != null) return;
            highlightIcon(index);
        }

     
    }

}
