using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Network;
namespace View
{
    public class OrchestratorViewHandler : MonoBehaviour
    {
        public static OrchestratorViewHandler Instance { get; private set; }

        public RegionCard[] regionCards;
        public TollPanelScript tollPanelScript;
        public AccessPanelScript accessPanelScript;
        public PriorityPanelScript priorityPanelScript;
        public GameObject PriorityPanel;
        private RegionCard activeRegion;
        private int selectedPriority;


        public bool isOrchestrator;

        private void Awake()
        {
            Instance = this;
            gameObject.SetActive(false);
        }

        void Start()
        {
            accessPanelScript.hidePanel();
            tollPanelScript.hidePanel();
            priorityPanelScript.hidePanel();

            foreach (RegionCard regionCard in regionCards)
            {
                regionCard.setHandler(this);
                regionCard.setOrchestratorOptions(true);
            }

   
            
        }

        public void showTollScreen(RegionCard regionCard)
        {
            activeRegion = regionCard;
            tollPanelScript.showPanel(regionCard);
        }


        public void showAccessScreen(RegionCard regionCard)
        {
            activeRegion = regionCard;
            accessPanelScript.showPanel(regionCard);
        }

        public void showPriorityScreen(RegionCard regionCard)
        {
            activeRegion = regionCard;
            priorityPanelScript.showPanel(regionCard);
        }

        public bool GetIsOrchestrator ()
        {
            return isOrchestrator;
        }

        public void resetCards()
        {
            foreach (RegionCard regionCard in regionCards)
            {
                regionCard.resetCard();
            }
        }

        public void changeEditStateCards(bool boolean)
        {
            foreach (RegionCard regionCard in regionCards)
            {
                regionCard.changeEditStateCard(boolean);
            }
        }
       

    }

}
