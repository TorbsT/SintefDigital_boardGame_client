using UnityEngine;
using UnityEngine.UI;


namespace Game.OrchestratorView
{
    public class PriorityMarker : MonoBehaviour
    {
        public Text priorityText;
        public void setPriority(int prioity)
        {
            if (prioity <= 0) return;
            priorityText.text = "+" + prioity;
        }
        
        public void SetActive(bool boolean)
        {
            gameObject.SetActive(boolean);
        }
    }
}

