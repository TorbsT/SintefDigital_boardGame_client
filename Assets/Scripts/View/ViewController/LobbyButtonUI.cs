using Network;
using TMPro;
using UnityEngine;

namespace View
{
    internal class LobbyButtonUI : MonoBehaviour
    {
        public string Name { get => nameField.text; set => nameField.text = value; }
        public string Quantity { get => quantityField.text; set => quantityField.text = value; }
        [field: SerializeField] public int LobbyId { get; set; }

        [SerializeField] private TextMeshProUGUI nameField;
        [SerializeField] private TextMeshProUGUI quantityField;

        public void Click()
        {
            NetworkData.Instance.CurrentGameState.id = LobbyId;
            MainMenuUIController.Instance.JoinLobby();
        }
    }
}