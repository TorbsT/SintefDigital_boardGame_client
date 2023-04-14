using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace View {
    public class AccessPanelScript : PanelScript
    {

        public override void handleInput()
        {
            Debug.Log(highlightedIndex);
            int selectedIndex = (int) highlightedIndex;
            activeRegion.setAccess(selectedIndex);
            hidePanel();
        }


    }

}
