using TMPro;
using UnityEngine;

namespace Pregame
{
    internal class LobbyPlayerUI : MonoBehaviour
    {
        public string Name { get => nameField.text; set => nameField.text = value; }
        public string Role { get => roleField.text; set => roleField.text = value; }
        public string Me { get => meField.text; set => meField.text = value; }

        [SerializeField] private TextMeshProUGUI nameField;
        [SerializeField] private TextMeshProUGUI roleField;
        [SerializeField] private TextMeshProUGUI meField;
    }
}