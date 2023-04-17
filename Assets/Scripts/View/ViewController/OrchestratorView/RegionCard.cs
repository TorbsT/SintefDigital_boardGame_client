using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Threading;

namespace View
{
    public class RegionCard : MonoBehaviour
    {
        public District district;

        private OrchestratorViewHandler handler;
        public GameObject[] vehicleAttributePrefab;

        public GameObject accessButton;
        public Point [] accessPoints;
        private List<IconScript> activeAccessRestrictions =  new List<IconScript>();

        public GameObject priorityButton;
        public Point[] priorityPoints;
        private List<IconScript> activePriorityRestrictions = new List<IconScript>();

        public GameObject tollButton;
        public GameObject tollCostIcon;
        public Point tollPoint;
        private IconScript activeTollRestriction;

        public PriorityMarker[] priorityMarkers;

        public Material cardMaterial;

   
   
        void Awake()
        {
            accessButton.transform.position = accessPoints[0].GetPos();
            priorityButton.transform.position = priorityPoints[0].GetPos();
            tollButton.transform.position = tollPoint.GetPos();
            accessButton.SetActive(false);
            priorityButton.SetActive(false);
            tollButton.SetActive(false);
            tollCostIcon.SetActive(false);
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


        public GameObject setIcon(int id, List<IconScript> activeRestricions, Point[] points, GameObject button,bool isOrchestrator )
        {
            if (id >= vehicleAttributePrefab.Length) { return null; } //non legal id
            int activeRestrictionsCount = activeRestricions.Count + 1;
            if (activeRestrictionsCount > 2) { return null; } //should not be possible

            if (!isOrchestrator || activeRestrictionsCount == 2)
            {
                button.SetActive(false);
            }
            else
            {
                button.transform.position = points[1].GetPos();
            }
            GameObject icon = Instantiate(vehicleAttributePrefab[id], points[activeRestrictionsCount - 1].GetPos(), Quaternion.identity, this.transform);
            return icon;
        }

        public bool removeIcon(IconScript iconScript, List<IconScript> activeRestricions, Point[] points, GameObject button)
        {
            if (!activeRestricions.Contains(iconScript)) { return false; } //should be in list
            if (activeRestricions.Count <= 0) { return false; } //should have icons

            int activeRestrictionsCount = activeRestricions.Count - 1;
            if (activeRestrictionsCount < 2)
            {
                button.SetActive(true);
                button.transform.position = points[activeRestrictionsCount].GetPos();
            }
            return true;
        }

        public void addToll()
        {
            this.handler.showTollScreen(this);
        }

        public void setToll(int cost,bool isOrchestrator)
        {
            if (activeTollRestriction != null) return;
            activeTollRestriction = tollCostIcon.GetComponent<IconScript>(); 
            activeTollRestriction.setTypeOfRestriction(Restriction.Toll);
            activeTollRestriction.setAttachedRegionCard(this);
            activeTollRestriction.setValue(cost);
            activeTollRestriction.setDeleteButton(isOrchestrator);
            tollCostIcon.transform.Find("costText").gameObject.GetComponent<Text>().text = "€" + cost;
            tollCostIcon.SetActive(true);

            addUpdateServer(Restriction.Toll, activeTollRestriction);
        }


        public void addAccess()
        {
            this.handler.showAccessScreen(this);
        }

        public bool removeToll(IconScript iconScript)
        {
            tollCostIcon.SetActive(false);
            activeTollRestriction = null;
            removeUpdateServer(Restriction.Toll, iconScript);
            return true;
        }

        public void setAccess(int id, bool isOrchestrator)
        {
            //if (activeTollRestriction.Any(res => res.getId() == id)) return;
            if (activeAccessRestrictions.Any(res => res.getId() == id)) return;
            GameObject icon = setIcon(id, activeAccessRestrictions, accessPoints, accessButton,isOrchestrator);
            IconScript iconScript = icon.GetComponent<IconScript>();
            activeAccessRestrictions.Add(iconScript);
            iconScript.setTypeOfRestriction(Restriction.Access);
            iconScript.setDeleteButton(isOrchestrator);
            iconScript.setAttachedRegionCard(this);


            addUpdateServer(Restriction.Access, iconScript);
        }

        public bool removeAccess(IconScript iconScript)
        {
    
            if(!removeIcon(iconScript,activeAccessRestrictions, accessPoints, accessButton)) { return false; }
            activeAccessRestrictions.Remove(iconScript);

       
            for (int i = 0; i < activeAccessRestrictions.Count; i++)
            {
                IconScript script = activeAccessRestrictions[i];
                script.moveTo(accessPoints[i].GetPos());
       
            }
            removeUpdateServer(Restriction.Access, iconScript);
            return true;
        }

        private void addPriorityMarker(int i,int value,Point point, Vector3 dimentions)
        {
            PriorityMarker priorityMarker = priorityMarkers[i];
            priorityMarker.transform.position = point.GetPos() + new Vector3(dimentions.x,0,0);
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

        public void setPriority(int id,int value, bool isOrchestrator)
        {
            if (activePriorityRestrictions.Any(res => res.getId() == id)) return;

            GameObject icon = setIcon(id, activePriorityRestrictions, priorityPoints, priorityButton,isOrchestrator);
            IconScript iconScript = icon.GetComponent<IconScript>();
            activePriorityRestrictions.Add(iconScript);
            iconScript.setTypeOfRestriction(Restriction.Priority);
            iconScript.setDeleteButton(isOrchestrator);
            iconScript.setAttachedRegionCard(this);
            iconScript.setValue(value);
            int activePriorityRestrictionsCount = activePriorityRestrictions.Count;
            addPriorityMarker(activePriorityRestrictionsCount-1,value, priorityPoints[activePriorityRestrictionsCount-1], iconScript.getDimentions());
            addUpdateServer(Restriction.Priority, iconScript);

        }


        public bool removePriority(IconScript iconScript)
        {
            if (!removeIcon(iconScript, activePriorityRestrictions, priorityPoints, priorityButton)) { return false; }
            activePriorityRestrictions.Remove(iconScript);
            resetPriorityMarkers();
            for (int i = 0; i < activePriorityRestrictions.Count; i++)
            {
                IconScript script = activePriorityRestrictions[i];
                script.moveTo(priorityPoints[i].GetPos());
                addPriorityMarker(i, script.getValue(),priorityPoints[i], script.getDimentions());
            }
            removeUpdateServer(Restriction.Priority, iconScript);
            return true;
        }


        public Material getMaterial()
        {
            return cardMaterial;
        }


        public void resetCard()
        {
           
            while(activeAccessRestrictions.Count != 0)
            {
                activeAccessRestrictions[0].removeSelf();
            }
            while (activePriorityRestrictions.Count != 0)
            {
                activePriorityRestrictions[0].removeSelf();
            }
            if (activeTollRestriction != null)
            {
                activeTollRestriction.removeSelf();
            }
        }

        public void changeEditStateCard(bool boolean)
        {
            foreach(IconScript activeAccessRestriction in activeAccessRestrictions)
            {
                activeAccessRestriction.setDeleteButton(boolean);
            }
            foreach (IconScript activePriorityRestriction in activePriorityRestrictions)
            {
                activePriorityRestriction.setDeleteButton(boolean);
            }
            if (activeTollRestriction != null)
            {
                activeTollRestriction.setDeleteButton(boolean);
            }
            setOrchestratorOptions(boolean);
        }

       

        private void addUpdateServer(Restriction restriction, IconScript iconScript)
        {
            switch (restriction)
            {
                case Restriction.Access:
                    Debug.Log("Added access to vechile " + iconScript.getId() + " at region " + (int) district);
                    break;
                case Restriction.Priority:
                    Debug.Log("Added priority to vechile " + iconScript.getId() + " with priority " + iconScript.getValue() + " at region " + (int) district);
                    break;
                case Restriction.Toll:
                    Debug.Log("Added toll to vechile " + " with price " + iconScript.getValue() + " at region " + (int) district);
                    break;
                default:
                    break;
            }
        }

        private void removeUpdateServer(Restriction restriction, IconScript iconScript)
        {
            switch (restriction)
            {
                case Restriction.Access:
                    Debug.Log("Removed access to vechile " + iconScript.getId() + " at region " + (int)district);
                    break;
                case Restriction.Priority:
                    Debug.Log("Removed priority to vechile " + iconScript.getId() + " with priority " + iconScript.getValue() + " at region " + (int)district);
                    break;
                case Restriction.Toll:
                    Debug.Log("Removed toll to vechile " + " with price " + iconScript.getValue() + " at region " + (int)district);
                    break;
                default:
                    break;
            }
        }

    }

}
