using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;



namespace View
{

    public class IconScript : MonoBehaviour
    {
        public int id;
        public GameObject closeButton; 
        private RegionCard currentRegionCard;
        private RectTransform rectTransform;

        private Restriction typeOfRestriction;
        // Start is called before the first frame update


        void Awake()
        {
            closeButton = this.transform.Find("close").gameObject;
            rectTransform = GetComponent<RectTransform>();
        }

        void Start()
        {
           
         
        }
        public void setAttachedRegionCard(RegionCard regionCard)
        {
            currentRegionCard = regionCard;
        }
        public void setTypeOfRestriction(Restriction restriction)
        {
            typeOfRestriction = restriction;
        }

        public void setDeleteButton(bool boolean)
        {

            closeButton.SetActive(boolean);
            //transform.localScale = new Vector2 (10, 10);b
            float width = rectTransform.rect.width;
            float height = rectTransform.rect.height;
            closeButton.transform.position = transform.position + new Vector3(width/2, height/2,0);

        }


        public Vector3 getDimentions()
        {
            return new Vector3(rectTransform.rect.width, rectTransform.rect.height,0);
        }

        public void moveTo(Vector3 pos)
        {
            this.transform.position = pos;
        }

        public void removeSelf() //if 0 remove access, if 1 remove 
        {
            if (currentRegionCard == null) { return; } // if not attached to a RegionCard, should not be able to access

            switch (typeOfRestriction)
            {
                case Restriction.Access:
                    if (!currentRegionCard.removeAccess(this)) { return; }
                    break;
                case Restriction.Priority:
                    if (!currentRegionCard.removePriority(this)) { return; }
                    break;
                default:
                    break;
            }
            Destroy(gameObject);
        }

 
    }

}
