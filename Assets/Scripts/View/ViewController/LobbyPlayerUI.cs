using TMPro;
using UnityEngine;

namespace View
{
    internal class LobbyPlayerUI : MonoBehaviour
    {
        public string Name { get => nameField.text; set => nameField.text = value; }
        public string Role { get => roleField.text; set => roleField.text = value; }
        public string Host { get => hostField.text; set => hostField.text = value; }

        [SerializeField] private TextMeshProUGUI nameField;
        [SerializeField] private TextMeshProUGUI roleField;
        [SerializeField] private TextMeshProUGUI hostField;
    }
}