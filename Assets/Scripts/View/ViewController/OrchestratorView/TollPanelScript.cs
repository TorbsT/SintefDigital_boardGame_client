using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace View {
    public class TollPanelScript : PanelScript
    {
       

        public override void handleInput()
        {
            int selectedToll = (int)highlightedIndex + 1;
            Debug.Log(isOrchestrator());
            activeRegion.setToll(selectedToll, isOrchestrator());
            hidePanel();
        }

    }

}
