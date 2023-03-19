using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace View
{
    public class RegionCard : MonoBehaviour
    {
        private OrchestratorViewHandler handler;
        public GameObject[] vehicleAttributePrefab;

        public GameObject accessButton;
        public Point [] accessPoints;
        private List<IconScript> activeAccessRestrictions =  new List<IconScript>();

        public GameObject priorityButton;
        public Point[] priorityPoints;
        private List<IconScript> activePriorityRestrictions = new List<IconScript>();

        public GameObject[] priorityMarkers;
        //public Color regionColor;
        void Start()
        {
            accessButton.transform.position = accessPoints[0].GetPos();
            priorityButton.transform.position = priorityPoints[0].GetPos();
        }


        public void setHandler(OrchestratorViewHandler handler)
        {
            this.handler = handler;
        }



        public GameObject setIcon(int id, List<IconScript> activeRestricions, Point[] points, GameObject button )
        {
            if (id >= vehicleAttributePrefab.Length) { return null; } //non legal id
            int activeRestrictionsCount = activeRestricions.Count + 1;
            if (activeRestrictionsCount > 2) { return null; } //should not be possible

            if (activeRestrictionsCount == 2)
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

        public void addAccess()
        {
            this.handler.showAccessScreen(this);
        }

        public void setAccess(int id)
        {
            GameObject icon = setIcon(id, activeAccessRestrictions, accessPoints, accessButton);
            IconScript iconScript = icon.GetComponent<IconScript>();
            activeAccessRestrictions.Add(iconScript);
            iconScript.setTypeOfRestriction(Restriction.Access);
            iconScript.setDeleteButton(true);
            iconScript.setAttachedRegionCard(this);
          

            pri(activeAccessRestrictions);
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
            pri(activeAccessRestrictions);
            return true;
        }

        private void addPriorityMarker(int i,Point point, Vector3 dimentions)
        {
            GameObject priorityMarker = priorityMarkers[i];
            priorityMarker.transform.position = point.GetPos() + new Vector3(dimentions.x,0,0);
            priorityMarker.SetActive(true);

        }
        private void resetPriorityMarkers()
        {
            foreach (GameObject priorityMarker in priorityMarkers)
            {
                //priorityMarker.setActive(false);
                priorityMarker.SetActive(false);
            }
        }

        public void addPriority()
        {
            this.handler.showPriorityScreen(this);
        }

        public void setPriority(int id)
        {
            GameObject icon = setIcon(id, activePriorityRestrictions, priorityPoints, priorityButton);
            IconScript iconScript = icon.GetComponent<IconScript>();
            activePriorityRestrictions.Add(iconScript);
            iconScript.setTypeOfRestriction(Restriction.Priority);
            iconScript.setDeleteButton(true);
            iconScript.setAttachedRegionCard(this);
            int activePriorityRestrictionsCount = activePriorityRestrictions.Count;
            Debug.Log(activePriorityRestrictionsCount);
            addPriorityMarker(activePriorityRestrictionsCount-1, priorityPoints[activePriorityRestrictionsCount-1], iconScript.getDimentions());

            pri(activeAccessRestrictions);
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
                addPriorityMarker(i, priorityPoints[i], script.getDimentions());
            }
            pri(activePriorityRestrictions);
            return true;
        }

        public void pri(List<IconScript> activeRestrictions)
        {
            foreach (IconScript s in activeRestrictions)
            {
                Debug.Log(s);
            }
        }
        // Update is called once per frame
        void Update()
        {
        
        }
    }

}
