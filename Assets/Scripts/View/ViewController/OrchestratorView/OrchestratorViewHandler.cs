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
            //GameStateSynchronizer.Instance.districtModifierChanged += funkson( //som har liste med district modifiers
            accessPanelScript.hidePanel();
            tollPanelScript.hidePanel();
            priorityPanelScript.hidePanel();

            foreach (RegionCard regionCard in regionCards)
            {
                regionCard.setHandler(this);
                regionCard.setOrchestratorOptions(true);
            }
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

        public bool GetIsOrchestrator () //TODO remove this dummy solution
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

        private RegionCard getRegionCardsByDistrict(NetworkData.District district)
        {
            List<RegionCard> cards = new List<RegionCard>();
            foreach (RegionCard regioncard in regionCards)
            {
                if (regioncard.getDistrict() == district)
                {
                    return regioncard;
                }
            }
            return null;
        }

        public void dummyServerHandler(string districtString, string modifierString, string vehicle_type_string, int? associated_movement_value , int? associated_money_value, bool delete)
        {
            NetworkData.District district = (NetworkData.District) Enum.Parse(typeof(NetworkData.District), districtString);
            //List<RegionCard> cards = getRegionCardsByDistrict(district);
            RegionCard regioncard = getRegionCardsByDistrict(district);
            regioncard.resetCard();
            addToRegionCard(regioncard, district, modifierString, vehicle_type_string, associated_movement_value, associated_money_value);
            /*foreach (RegionCard regioncard in  cards)
            {
                regioncard.resetCard();
                addToRegionCard(regioncard, district, modifierString,vehicle_type_string,associated_movement_value, associated_money_value);
            }*/
        }

        private void addToRegionCard(RegionCard regioncard, NetworkData.District district, string modifierString, string vehicle_type_string, int? associated_movement_value, int? associated_money_value)
        {
            NetworkData.DistrictModifierType modifier = (NetworkData.DistrictModifierType)Enum.Parse(typeof(NetworkData.DistrictModifierType), modifierString);
            int? vehicle_type_id = null;
            if (vehicle_type_string != null)
            {
                NetworkData.VehicleType vehicle_type = (NetworkData.VehicleType)Enum.Parse(typeof(NetworkData.VehicleType), vehicle_type_string);
                vehicle_type_id = (int)vehicle_type;
            }
            switch (modifier)
            {
                case NetworkData.DistrictModifierType.Access:
                    regioncard.setAccess((int)vehicle_type_id, isOrchestrator);
                    break;
                case NetworkData.DistrictModifierType.Priority:
                    regioncard.setPriority((int)vehicle_type_id, (int)associated_movement_value, isOrchestrator);
                    break;
                case NetworkData.DistrictModifierType.Toll:
                    regioncard.setToll((int)associated_money_value, isOrchestrator);
                    break;
                default:
                    break;
            }
        }
       

    }

}
