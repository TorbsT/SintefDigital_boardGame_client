using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace View
{
    public class OrchestratorViewHandler : MonoBehaviour
    {
        public RegionCard[] regionCards;
        public GameObject TollPanel;
        public GameObject AccessPanel;
        public GameObject PriorityPanel;
        private RegionCard activeRegion;
        private int selectedPriority;
        void Start()
        {
            AccessPanel.SetActive(false);
            PriorityPanel.SetActive(false);
            foreach (RegionCard regionCard in regionCards)
            {
                regionCard.setHandler(this);
            }
        }

        public void showTollScreen(RegionCard regionCard)
        {
            activeRegion = regionCard;
            setColorOfPanel(AccessPanel, regionCard.getMaterial());
            TollPanel.SetActive(true);
        }

        public void handleTollInput(int cost)
        {
            activeRegion.setToll(cost);
            activeRegion = null;
            TollPanel.SetActive(false);
        }

        public void showAccessScreen(RegionCard regionCard)
        {
            activeRegion = regionCard;
            setColorOfPanel(AccessPanel, regionCard.getMaterial());
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
            setColorOfPanel(PriorityPanel, regionCard.getMaterial());
            PriorityPanel.SetActive(true);
        }

        private void setColorOfPanel(GameObject panel,Material mat)
        {
            panel.GetComponent<Image>().color = mat.GetColor("_Color");
            //panel.GetComponent<Renderer>().material = mat;
        }

        public void handlePriorityInput(int id, int value)
        {
            Debug.Log(id + " " + value);
            activeRegion.setPriority(id, value);
            activeRegion = null;
            PriorityPanel.SetActive(false);
        }

      
    }

}
