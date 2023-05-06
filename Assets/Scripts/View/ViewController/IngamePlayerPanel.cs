using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace View
{
    [ExecuteAlways]
    internal class IngamePlayerPanel : MonoBehaviour
    {
        [field: SerializeField] public PlayerOwned PlayerOwned { get; private set; }
        [field: SerializeField] public TextMeshProUGUI Text { get; private set; }
        [field: SerializeField] public GameObject Shade { get; private set; }

        private void Awake()
        {
            
        }
        private void OnValidate()
        {
            if (PlayerOwned == null)
                PlayerOwned = GetComponent<PlayerOwned>();
        }
    }
}
