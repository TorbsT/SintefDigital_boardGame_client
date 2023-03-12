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
        private int activeAccessRestrictions = 0;


        //public Color regionColor;
        void Start()
        {
            accessButton.transform.position = accessPoints[0].GetPos();
            //PoolManager.Instance.Enpool(buttonPrefab);
        }


        public void setHandler(OrchestratorViewHandler handler)
        {
            this.handler = handler;
        }

        public void addAccess()
        {
            this.handler.showAccessScreen(this);
        }

        public void setAccess(int id)
        {
            if (id >= vehicleAttributePrefab.Length) { return; } //non legal id
            if (activeAccessRestrictions >= 2) { return; } //should not be possible
            activeAccessRestrictions++;
            if (activeAccessRestrictions == 2)
            {
                accessButton.SetActive(false);
            } else
            {
                accessButton.transform.position = accessPoints[1].GetPos();
            }

            Instantiate(vehicleAttributePrefab[id], accessPoints[activeAccessRestrictions-1].GetPos(), Quaternion.identity,this.transform);
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }

}
