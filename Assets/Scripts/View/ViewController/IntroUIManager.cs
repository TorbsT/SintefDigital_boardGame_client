using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Network;

public class IntroUIManager : MonoBehaviour
{
    [SerializeField] private string mainMenuScene = "MainMenu";
    [SerializeField] private TextMeshProUGUI nameInput;
    public void Login()
    {
        RestAPI.Instance.CreateUniquePlayerId(
                (response) =>
                {
                    NetworkData.Instance.Me = new NetworkData.Player
                    {
                        connected_game_id = null,
                        in_game_id = NetworkData.InGameID.Undecided.ToString(),
                        unique_id = response,  // integer
                        name = nameInput.text,
                        position = null
                    };
                    SceneManager.LoadSceneAsync(mainMenuScene);
                    RestAPI.Instance.DebugPlayerCount(
                        (success) => { Debug.Log(success); }, 
                        (failure) => { });
                },
                (failure) => { }
            );
    }
}
