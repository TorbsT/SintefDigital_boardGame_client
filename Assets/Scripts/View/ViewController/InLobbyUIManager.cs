using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace View
{
    public class InLobbyUIManager : MonoBehaviour
    {
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private string gameScene;
        public void Show(int lobbyId)
        {
            GetComponent<UIListHandler>().Clear();
            for (int i = 0; i < UnityEngine.Random.Range(0, 6); i++)
            {
                string playerName = $"Per{i}";
                string roleName = "player";
                bool host = false;
                if (i == 0)
                {
                    roleName = "orchestrator";
                    host = true;
                }
                AddPlayer(playerName, roleName, host);
            }
        }
        public void StartGame()
        {
            SceneManager.LoadSceneAsync(gameScene);
        }
        private void AddPlayer(string playerName, string roleName, bool host)
        {
            GameObject panel = PoolManager.Instance.Depool(playerPrefab);
            LobbyPlayerUI player = panel.GetComponent<LobbyPlayerUI>();
            player.Name = playerName;
            player.Role = roleName;
            if (host) player.Host = "Host";
            else player.Host = "";
            GetComponent<UIListHandler>().AddItem(panel);
        }
    }
}