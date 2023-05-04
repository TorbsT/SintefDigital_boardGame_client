using System;
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
            //gameObject.SetActive(false); temperarly removed
        }

        void Start()
        {
            GameStateSynchronizer.Instance.districtModifierChanged += renderModifiers;
            TurnManager.Instance.orchestratorTurnChange += renderModifiers;
            
            gameObject.SetActive(false);
            accessPanelScript.hidePanel();
            tollPanelScript.hidePanel();
            priorityPanelScript.hidePanel();

            foreach (RegionCard regionCard in regionCards)
            {
                regionCard.setHandler(this);
                
            }
            //dummy tests for traffic
            regionCards[1].setTraffic(4);
            regionCards[2].setTraffic(2);
            regionCards[5].setTraffic(4);
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

        public void resetCards()
        {
            foreach (RegionCard regionCard in regionCards)
            {
                regionCard.resetCard();
            }
        }

        public void setEditStateCards() //set edit state of cards based on roles and the turn 
        {
            foreach (RegionCard regionCard in regionCards)
            {
                regionCard.setEditStateCard();
            }
        }

        
     
        public void renderModifiers(List<NetworkData.DistrictModifier> modifierList)
        {
            
           
            foreach (RegionCard regionCard in regionCards)
            {
                regionCard.resetCard();

                foreach(NetworkData.DistrictModifier disModifier in modifierList)
                {
                    if (disModifier.district == regionCard.getDistrict().ToString())
                    {
                        addModifierToRegionCard(regionCard, disModifier);
                    }
                }
            }
            
        }

        private void addModifierToRegionCard(RegionCard regioncard, NetworkData.DistrictModifier districtModifier)
        {
            NetworkData.DistrictModifierType modifier = (NetworkData.DistrictModifierType)Enum.Parse(typeof(NetworkData.DistrictModifierType), districtModifier.modifier);
            int? vehicle_type_id = null;
            if (districtModifier.vehicle_type != null)
            {
                NetworkData.VehicleType vehicle_type = (NetworkData.VehicleType)Enum.Parse(typeof(NetworkData.VehicleType), districtModifier.vehicle_type);
                vehicle_type_id = (int)vehicle_type;
            }
            switch (modifier)
            {
                case NetworkData.DistrictModifierType.Access:
                    regioncard.setAccess((int)vehicle_type_id);
                    break;
                case NetworkData.DistrictModifierType.Priority:
                    regioncard.setPriority((int)vehicle_type_id, (int)districtModifier.associated_movement_value);
                    break;
                case NetworkData.DistrictModifierType.Toll:
                    regioncard.setToll((int)districtModifier.associated_money_value);
                    break;
                default:
                    break;
            }
        }
       

    }

}
