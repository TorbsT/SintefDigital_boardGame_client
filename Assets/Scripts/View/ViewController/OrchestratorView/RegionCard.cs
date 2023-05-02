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
            setOrchestratorOptions(false);
            tollCostIcon.SetActive(false);

            setColor();
            setDistrictText();
            setTraffic(traffic);

        }


        public void setHandler(OrchestratorViewHandler handler)
        {
            this.handler = handler;
        }

        public void setOrchestratorOptions(bool boolean)
        {
            accessButton.SetActive(boolean);
            priorityButton.SetActive(boolean);
            tollButton.SetActive(boolean);
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

        public GameObject setIcon(int id, List<IconScript> activeRestricions, Point[] points, GameObject button, bool isOrchestrator)
        {
            if (id >= vehicleAttributePrefab.Length) { return null; } //non legal id
            int activeModifiersCount = activeRestricions.Count + 1;
            if (activeModifiersCount > 2) { return null; } //should not be possible

            if (!isOrchestrator || activeModifiersCount == 2)
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
                button.SetActive(true);
                button.transform.position = points[activeModifiersCount].GetPos();
            }
            return true;
        }

        public void addToll()
        {
            this.handler.showTollScreen(this);
        }

        public void setToll(int cost, bool isOrchestrator)
        {

            activeTollModifier = tollCostIcon.GetComponent<IconScript>();
            activeTollModifier.setTypeOfModifier(NetworkData.DistrictModifierType.Toll);
            activeTollModifier.setAttachedRegionCard(this);
            activeTollModifier.setValue(cost);
            activeTollModifier.setDeleteButton(isOrchestrator);
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


        public void setAccess(int id, bool isOrchestrator)
        {

            GameObject icon = setIcon(id, activeAccessModifiers, accessPoints, accessButton, isOrchestrator);
            IconScript iconScript = icon.GetComponent<IconScript>();
            activeAccessModifiers.Add(iconScript);
            iconScript.setTypeOfModifier(NetworkData.DistrictModifierType.Access);
            iconScript.setDeleteButton(isOrchestrator);
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

        public void setPriority(int id, int value, bool isOrchestrator)
        {
            GameObject icon = setIcon(id, activePriorityModifiers, priorityPoints, priorityButton, isOrchestrator);
            IconScript iconScript = icon.GetComponent<IconScript>();
            activePriorityModifiers.Add(iconScript);
            iconScript.setTypeOfModifier(NetworkData.DistrictModifierType.Priority);
            iconScript.setDeleteButton(isOrchestrator);
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

        public void changeEditStateCard(bool boolean)
        {
            foreach (IconScript activeAccessModifier in activeAccessModifiers)
            {
                activeAccessModifier.setDeleteButton(boolean);
            }
            foreach (IconScript activePriorityModifier in activePriorityModifiers)
            {
                activePriorityModifier.setDeleteButton(boolean);
            }
            if (activeTollModifier != null)
            {
                activeTollModifier.setDeleteButton(boolean);
            }
            setOrchestratorOptions(boolean);
        }

        //These method only sends changes to the server, visual updates are done trough the new gamestate recived
        public void setPriorityServer(int id, int value, bool isOrchestrator)
        {
            if (activePriorityModifiers.Any(res => res.getId() == id)) return;
            sendToServer(NetworkData.DistrictModifierType.Priority, id, value, null, false);
        }

        public void removePriorityServer(IconScript iconScript)
        {
            sendToServer(NetworkData.DistrictModifierType.Priority, iconScript.getId(), iconScript.getValue(), null, true);
        }

        public void setAccessServer(int id, bool isOrchestrator)
        {
            if (activeAccessModifiers.Any(res => res.getId() == id)) return;
            sendToServer(NetworkData.DistrictModifierType.Access, id, null, null, false);
        }

        public void removeAccessServer(IconScript iconScript)
        {
            sendToServer(NetworkData.DistrictModifierType.Access, iconScript.getId(), null, null, true);
        }

        public void setTollServer(int cost, bool isOrchestrator)
        {
            if (activeTollModifier != null) return;
            sendToServer(NetworkData.DistrictModifierType.Toll, null, null, cost, false);
        }

        public void removeTollServer(IconScript iconScript)
        {
            sendToServer(NetworkData.DistrictModifierType.Toll, iconScript.getId(), null, null, true);
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
            
            handler.dummyServerHandler(districtModifier);

        }

        


     }   



}
//private const string input_type = "ModifyDistrict";
//private const int related_node_id = null;