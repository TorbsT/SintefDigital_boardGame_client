using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Threading;
using TMPro;
using Network;

namespace View
{
    public class RegionCard : MonoBehaviour
    {
        public NetworkData.District district;
        private int traffic = 1;
        private int cost;
        public bool isEditable;

        public TextMeshProUGUI districtName;
        public TextMeshProUGUI districtTraffic;
        public TextMeshProUGUI districtCost;

        private OrchestratorViewHandler handler;
        public GameObject[] vehicleAttributePrefab;

        public GameObject accessButton;
        public Point[] accessPoints;
        private List<IconScript> activeAccessModifiers = new List<IconScript>();

        public GameObject priorityButton;
        public Point[] priorityPoints;
        private List<IconScript> activePriorityModifiers = new List<IconScript>();

        public GameObject tollButton;
        public GameObject tollCostIcon;
        public Point tollPoint;
        private IconScript activeTollModifier;

        public PriorityMarker[] priorityMarkers;
        public GameObject[] truckMarkers;

        public Material cardMaterial;



        void Awake()
        {
            accessButton.transform.position = accessPoints[0].GetPos();
            priorityButton.transform.position = priorityPoints[0].GetPos();
            tollButton.transform.position = tollPoint.GetPos();
            setSizesOfIcons();
            setEditStateCard();
            tollCostIcon.SetActive(false);

            setColor();
            setDistrictText();
            setTraffic(traffic);

        }


        public void setHandler(OrchestratorViewHandler handler)
        {
            this.handler = handler;
        }

        private void setSizesOfIcons()
        {
            setSizePercentage(accessButton, 15);
            setSizePercentage(priorityButton, 15);
            setSizePercentage(tollButton, 15);
        }

        public void setOrchestratorOptions(bool boolean)
        {
            accessButton.SetActive(boolean);
            priorityButton.SetActive(boolean);
            tollButton.SetActive(boolean);
        }


        private void setSizePercentage(GameObject gameObject, float percentage)
        {
      
            RectTransform iconRect = gameObject.transform.GetComponent<RectTransform>();
            RectTransform cardRect = GetComponent<RectTransform>();
            float cardScaleX = transform.lossyScale.x;
            iconRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, cardRect.rect.width * percentage / 100f);
            iconRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, cardRect.rect.width * percentage / 100f);
            gameObject.transform.localScale = new Vector3(1f,1f,1f);

        }
        public void setColor()
        {
            gameObject.GetComponent<Image>().color = cardMaterial.GetColor("_Color");
        }

        public void setDistrictText()
        {
            districtName.text = district.ToString();
        }

        public void setTraffic(int trafficNumber)
        {
            traffic = trafficNumber;
            districtTraffic.text = "" + trafficNumber;
            setTruckMarkers(trafficNumber);
            setCost(trafficNumber);
        }

        public bool getEditState()
        {
            return isEditable & GameStateSynchronizer.Instance.IsOrchestrator;
        }

        private void setCost(int trafficNumber)
        {

            switch (trafficNumber)
            {
                case 3:
                    cost = -1;
                    break;
                case 4:
                    cost = -2;
                    break;
                case 5:
                    cost = -4;
                    break;
                default:
                    cost = 0;
                    break;
            }
            districtCost.text = "" + cost;
        }

        private void setTruckMarkers(int numberOfTrucks)
        {

            for (int i = 0; i < truckMarkers.Length; i++)
            {
                GameObject truckMarker = truckMarkers[i];
                truckMarker.SetActive(i < numberOfTrucks);
            }
        }

        public GameObject setIcon(int id, List<IconScript> activeRestricions, Point[] points, GameObject button)
        {
            if (id >= vehicleAttributePrefab.Length) { return null; } //non legal id
            int activeModifiersCount = activeRestricions.Count + 1;
            if (activeModifiersCount > 2) { return null; } //should not be possible

            if (!getEditState() || activeModifiersCount == 2)
            {
                button.SetActive(false);
            }
            else
            {
                button.transform.position = points[1].GetPos();
            }
            //GameObject icon = Instantiate(vehicleAttributePrefab[id], points[activeModifiersCount - 1].GetPos(), Quaternion.identity, this.transform);
            GameObject icon = PoolManager.Instance.Depool(vehicleAttributePrefab[id]);
            icon.transform.SetParent(this.transform);
            setSizePercentage(icon, 15);
            icon.transform.position = points[activeModifiersCount - 1].GetPos();
            return icon;
        }

        public bool removeIcon(IconScript iconScript, List<IconScript> activeRestricions, Point[] points, GameObject button)
        {
            if (!activeRestricions.Contains(iconScript)) { return false; } //should be in list
            if (activeRestricions.Count <= 0) { return false; } //should have icons

            int activeModifiersCount = activeRestricions.Count - 1;
            if (activeModifiersCount < 2)
            {
                button.SetActive(getEditState());
                button.transform.position = points[activeModifiersCount].GetPos();
            }
            return true;
        }

        public void addToll()
        {
            this.handler.showTollScreen(this);
        }

        public void setToll(int cost)
        {

            activeTollModifier = tollCostIcon.GetComponent<IconScript>();
            activeTollModifier.setTypeOfModifier(NetworkData.DistrictModifierType.Toll);
            activeTollModifier.setAttachedRegionCard(this);
            activeTollModifier.setValue(cost);
            activeTollModifier.setDeleteButton(getEditState());
            tollCostIcon.transform.position = tollPoint.GetPos();
            tollCostIcon.transform.Find("costText").gameObject.GetComponent<Text>().text = "€" + cost;
            tollCostIcon.SetActive(true);


        }


        public bool removeToll(IconScript iconScript)
        {
            //sendToServer(Modifier.Toll, iconScript.getId(), null, null, true);
            tollCostIcon.SetActive(false);
            activeTollModifier = null;
            return true;
        }

        public void addAccess()
        {
            this.handler.showAccessScreen(this);
        }


        public void setAccess(int id)
        {

            GameObject icon = setIcon(id, activeAccessModifiers, accessPoints, accessButton);
            IconScript iconScript = icon.GetComponent<IconScript>();
            activeAccessModifiers.Add(iconScript);
            iconScript.setTypeOfModifier(NetworkData.DistrictModifierType.Access);
            iconScript.setDeleteButton(getEditState());
            iconScript.setAttachedRegionCard(this);
        }

        public bool removeAccess(IconScript iconScript)
        {
            if (!removeIcon(iconScript, activeAccessModifiers, accessPoints, accessButton)) { return false; }
            activeAccessModifiers.Remove(iconScript);

            for (int i = 0; i < activeAccessModifiers.Count; i++)
            {
                IconScript script = activeAccessModifiers[i];
                script.moveTo(accessPoints[i].GetPos());
            }
        
            return true;
        }

        private void addPriorityMarker(int i, int value, Point point, Vector3 dimentions)
        {
            PriorityMarker priorityMarker = priorityMarkers[i];
            priorityMarker.transform.position = point.GetPos() + new Vector3(dimentions.x, 0, 0);
            priorityMarker.setPriority(value);
            priorityMarker.SetActive(true);

        }
        private void resetPriorityMarkers()
        {
            foreach (PriorityMarker priorityMarker in priorityMarkers)
            {
                priorityMarker.SetActive(false);
            }
        }

        public void addPriority()
        {

            this.handler.showPriorityScreen(this);
        }

        public void setPriority(int id, int value)
        {
            GameObject icon = setIcon(id, activePriorityModifiers, priorityPoints, priorityButton);
            IconScript iconScript = icon.GetComponent<IconScript>();
            activePriorityModifiers.Add(iconScript);
            iconScript.setTypeOfModifier(NetworkData.DistrictModifierType.Priority);
            iconScript.setDeleteButton(getEditState());
            iconScript.setAttachedRegionCard(this);
            iconScript.setValue(value);
            int activePriorityModifiersCount = activePriorityModifiers.Count;
            addPriorityMarker(activePriorityModifiersCount - 1, value, priorityPoints[activePriorityModifiersCount - 1], iconScript.getDimentions());
 

        }


        public bool removePriority(IconScript iconScript)
        {
        
            if (!removeIcon(iconScript, activePriorityModifiers, priorityPoints, priorityButton)) { return false; }
            activePriorityModifiers.Remove(iconScript);
            resetPriorityMarkers();
            for (int i = 0; i < activePriorityModifiers.Count; i++)
            {
                IconScript script = activePriorityModifiers[i];
                script.moveTo(priorityPoints[i].GetPos());
                addPriorityMarker(i, script.getValue(), priorityPoints[i], script.getDimentions());
            }
            
            return true;
        }

        public NetworkData.District getDistrict()
        {
            return district;
        }

        public Material getMaterial()
        {
            return cardMaterial;
        }

        public void resetCard()
        {

            while (activeAccessModifiers.Count != 0)
            {
                activeAccessModifiers[0].removeSelf();
            }
            while (activePriorityModifiers.Count != 0)
            {
                activePriorityModifiers[0].removeSelf();
            }
            if (activeTollModifier != null)
            {
                activeTollModifier.removeSelf();
            }
        }

        public void setEditStateCard()
        {
            bool editable = getEditState();
            foreach (IconScript activeAccessModifier in activeAccessModifiers)
            {
                activeAccessModifier.setDeleteButton(editable);
            }
            foreach (IconScript activePriorityModifier in activePriorityModifiers)
            {
                activePriorityModifier.setDeleteButton(editable);
            }
            if (activeTollModifier != null)
            {
                activeTollModifier.setDeleteButton(editable);
            }
            setOrchestratorOptions(editable);
        }

        //These method only sends changes to the server, visual updates are done trough the new gamestate recived
        public void setPriorityServer(int id, int value)
        {
            if (activePriorityModifiers.Any(res => res.getId() == id)) return;
            sendToServer(NetworkData.DistrictModifierType.Priority, id, value, null, false);
        }

        public void removePriorityServer(IconScript iconScript)
        {
            sendToServer(NetworkData.DistrictModifierType.Priority, iconScript.getId(), iconScript.getValue(), null, true);
        }

        public void setAccessServer(int id)
        {
            if (activeAccessModifiers.Any(res => res.getId() == id)) return;
            sendToServer(NetworkData.DistrictModifierType.Access, id, null, null, false);
        }

        public void removeAccessServer(IconScript iconScript)
        {
            sendToServer(NetworkData.DistrictModifierType.Access, iconScript.getId(), null, null, true);
        }

        public void setTollServer(int cost)
        {
            if (activeTollModifier != null) return;
            sendToServer(NetworkData.DistrictModifierType.Toll, null, null, cost, false);
        }

        public void removeTollServer(IconScript iconScript)
        {
            sendToServer(NetworkData.DistrictModifierType.Toll, null, null, iconScript.getValue(), true);
        }


        private void sendToServer(NetworkData.DistrictModifierType restriction, int? vehicle_type_id, int? associated_movement_value, int? associated_money_value, bool delete)
        {
            const string input_type = "ModifyDistrict";
            string districtString = district.ToString();
            string modifierString = restriction.ToString();
            string vehicle_typeString = null;
            if (vehicle_type_id != null)
            {
                vehicle_typeString = ((NetworkData.VehicleType)vehicle_type_id).ToString();
            }

            NetworkData.DistrictModifier districtModifier = new()
            {
                district = districtString,
                modifier = modifierString,
                vehicle_type =  vehicle_typeString,
                associated_movement_value = associated_movement_value,
                associated_money_value = associated_money_value,
                delete = delete
            };
            
            NetworkData.PlayerInput playerInput = new()
            {
                player_id = NetworkData.Instance.UniqueID,
                game_id =  (int)GameStateSynchronizer.Instance.LobbyId,
                input_type = "ModifyDistrict",
                related_role =  null,
                related_node_id =  null,
                district_modifier = districtModifier
            };

            RestAPI.Instance.SendPlayerInput(success => { }, failure => {
                Debug.LogWarning($"Could not send Orchestrator input{failure}");
            }, playerInput);
            //handler.dummyServerHandler(districtModifier);
            Debug.Log("Server is called");

        }

        


     }   



}
