using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Network;


namespace View
{
    public class IconScript : MonoBehaviour
    {
        public int id;
        public GameObject closeButton; 
        private RegionCard currentRegionCard;
        private RectTransform rectTransform;

        private NetworkData.DistrictModifierType typeOfModifier;
        private int value = 0;


        void Awake()
        {
            closeButton = this.transform.Find("close").gameObject;
            rectTransform = GetComponent<RectTransform>();
        }

        public void setAttachedRegionCard(RegionCard regionCard)
        {
            currentRegionCard = regionCard;
        }
        public void setTypeOfModifier(NetworkData.DistrictModifierType restriction)
        {
            typeOfModifier = restriction;
        }

        public void setDeleteButton(bool boolean)
        {
            closeButton.SetActive(boolean);
        }


        public Vector3 getDimentions() //returns size adjusted to scale
        {
            return new Vector3(rectTransform.rect.width* transform.lossyScale.x, rectTransform.rect.height* transform.lossyScale.y, 0);
        }

        public void moveTo(Vector3 pos)
        {
            this.transform.position = pos;
        }

        public void removeOnServer() 
        {
            Debug.Log(typeOfModifier + " " + currentRegionCard);
            if (currentRegionCard == null) { return; } // if not attached to a RegionCard, should not be able to access
            switch (typeOfModifier)
            {
                case NetworkData.DistrictModifierType.Access:
                    currentRegionCard.removeAccessServer(this);
                    break;
                case NetworkData.DistrictModifierType.Priority:
                    currentRegionCard.removePriorityServer(this);
                    break;
                case NetworkData.DistrictModifierType.Toll:
                    currentRegionCard.removeTollServer(this);
                    break;
                default:
                    break;
            }
        }

        public void removeSelf() 
        {
            if (currentRegionCard == null) { return; } // if not attached to a RegionCard, should not be able to access
            switch (typeOfModifier)
            {
                case NetworkData.DistrictModifierType.Access:
                    currentRegionCard.removeAccess(this);
                    PoolManager.Instance.Enpool(gameObject);
                    break;
                case NetworkData.DistrictModifierType.Priority:
                    currentRegionCard.removePriority(this);
                    PoolManager.Instance.Enpool(gameObject); 
                    break;
                case NetworkData.DistrictModifierType.Toll:
                    currentRegionCard.removeToll(this);
                    break;
                default:
                    break;
            }
        }

        public int getValue()
        {
            return value;
        }
        public void setValue(int value)
        {
            this.value = value;
        }

        public int getId()
        {
            return id;
        }


    }

}
