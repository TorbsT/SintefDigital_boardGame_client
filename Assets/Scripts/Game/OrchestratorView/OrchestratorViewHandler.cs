using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Common.Network;
using Common;

namespace Game.OrchestratorView
{
    public class OrchestratorViewHandler : MonoBehaviour
    {
        public static OrchestratorViewHandler Instance { get; private set; }

        [SerializeField] private GameObject orchestratorViewGO;
        [SerializeField] private RectTransform otherCardsParent;  // Situation/Objective cards
        [SerializeField] private GameObject situationCardPrefab;
        [SerializeField] private GameObject objectiveCardPrefab;
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

        void OnEnable()
        {
            GameStateSynchronizer.Instance.districtModifierChanged += renderModifiers;
            TurnManager.Instance.orchestratorTurnChange += renderModifiers;
            GameStateSynchronizer.Instance.situationCardChanged += renderTraffic;
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

            RefreshOtherCards();
        }
        private void OnDisable()
        {
            GameStateSynchronizer.Instance.districtModifierChanged -= renderModifiers;
            TurnManager.Instance.orchestratorTurnChange -= renderModifiers;
            GameStateSynchronizer.Instance.situationCardChanged -= renderTraffic;
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

        public void renderTraffic(NetworkData.SituationCard situationcard)
        {
            foreach (RegionCard regionCard in regionCards)
            {
                foreach (NetworkData.CostTuple costTuple in situationcard.costs)
                {
                    if (costTuple.neighbourhood == regionCard.getDistrict().ToString())
                    {
                        NetworkData.Traffic trafficEnum = (NetworkData.Traffic)Enum.Parse(typeof(NetworkData.Traffic), costTuple.traffic);
                        regionCard.setTraffic((int)trafficEnum);
                    }
                }
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
                NetworkData.RestrictionType vehicle_type = (NetworkData.RestrictionType)Enum.Parse(typeof(NetworkData.RestrictionType), districtModifier.vehicle_type);
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
        private void RefreshOtherCards()
        {
            List<GameObject> temp = new();  // Prevent errors
            foreach (Transform t in otherCardsParent.GetComponentsInChildren<Transform>())
            {
                if (t == otherCardsParent) continue;
                if (t.parent != otherCardsParent) continue;
                temp.Add(t.gameObject);
            }
            foreach (GameObject go in temp)
                PoolManager.Enpool(go);
            var sitCard = AddOtherCard(situationCardPrefab).GetComponent<GameCard>();
            sitCard.SetValues(GameStateSynchronizer.Instance.GameState.Value.situation_card.Value);
            //var objCard = AddOtherCard(objectiveCardPrefab).GetComponent<ObjectiveCard>();
        }
        private GameObject AddOtherCard(GameObject prefab)
        {
            GameObject card = PoolManager.Depool(prefab);
            RectTransform rt = card.GetComponent<RectTransform>();
            rt.SetParent(otherCardsParent);
            return card;
        }
    }

}
