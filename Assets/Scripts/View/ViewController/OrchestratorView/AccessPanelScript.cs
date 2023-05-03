using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace View {
    public class AccessPanelScript : PanelScript
    {

        public override void handleInput()
        {
            int selectedIndex = (int) highlightedIndex;
            activeRegion.setAccessServer(selectedIndex);
            hidePanel();
        }


    }

}
