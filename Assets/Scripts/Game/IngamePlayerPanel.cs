using TMPro;
using UnityEngine;
using Common;

namespace Game
{
    [ExecuteAlways]
    internal class IngamePlayerPanel : MonoBehaviour
    {
        [field: SerializeField] public PlayerOwned PlayerOwned { get; private set; }
        [field: SerializeField] public TextMeshProUGUI Text { get; private set; }
        [field: SerializeField] public GameObject Shade { get; private set; }

        private void OnValidate()
        {
            if (PlayerOwned == null)
                PlayerOwned = GetComponent<PlayerOwned>();
        }
    }
}
