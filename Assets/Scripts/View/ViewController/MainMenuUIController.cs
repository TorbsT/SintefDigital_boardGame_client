using Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace View
{
    internal class MainMenuUIController : MonoBehaviour
    {
        public static MainMenuUIController Instance { get; private set; }

        [SerializeField] private LobbySearchUIManager lobbySearchUIManager;
        [SerializeField] private InLobbyUIManager inLobbyUIManager;

        void Awake()
        {
            Instance = this;
            lobbySearchUIManager.gameObject.SetActive(true);
            inLobbyUIManager.gameObject.SetActive(false);
        }
        public void JoinLobby(int id)
        {
            GameStateSynchronizer.Instance.id
            inLobbyUIManager.gameObject.SetActive(true);
            lobbySearchUIManager.gameObject.SetActive(false);
        }
        public void BackToMainMenu()
        {
            inLobbyUIManager.gameObject.SetActive(false);
            lobbySearchUIManager.gameObject.SetActive(true);
        }
    }
}
