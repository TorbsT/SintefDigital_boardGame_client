using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace View
{
    public class OrchestratorViewHandler : MonoBehaviour
    {
        public RegionCard[] regionCards;
        public GameObject AccessPanel;
        public GameObject PriorityPanel;
        private RegionCard activeRegion;
        private int selectedPriority;
        void Start()
        {
            AccessPanel.SetActive(false);
            foreach (RegionCard regionCard in regionCards)
            {
                regionCard.setHandler(this);
            }
        }
        public void showAccessScreen(RegionCard regionCard)
        {
            activeRegion = regionCard;
            AccessPanel.SetActive(true);
        }

        public void handleAcessInput(int id)
        {
            activeRegion.setAccess(id);
            activeRegion = null;
            AccessPanel.SetActive(false);
        }

        public void showPriorityScreen(RegionCard regionCard)
        {
            activeRegion = regionCard;
            PriorityPanel.SetActive(true);
        }

        public void handlePriorityInput(int id, int value)
        {
            activeRegion.setPriority(id, value);
            activeRegion = null;
            PriorityPanel.SetActive(false);
        }

      
    }

}
