using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Game.OrchestratorView
{
    public class TollPanelScript : PanelScript
    {
        public override void handleInput()
        {
            int selectedToll = (int)highlightedIndex + 1;
            activeRegion.setTollServer(selectedToll);

            hidePanel();
        }

    }

}
