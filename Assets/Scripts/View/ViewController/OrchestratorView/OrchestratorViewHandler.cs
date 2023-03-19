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

        public void handlePriorityInput(int id)
        {
            activeRegion.setPriority(id);
            activeRegion = null;
            PriorityPanel.SetActive(false);
        }
    }

}
