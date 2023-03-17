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
        //SceneManager.LoadSceneAsync(mainMenuScene);
        RestAPI.Instance.CreateUniquePlayerId(
                (response) =>
                {
                    NetworkData.Instance.UniqueID = response;
                    NetworkData.Instance.Name = nameInput.text;
                    SceneManager.LoadSceneAsync(mainMenuScene);
                    RestAPI.Instance.DebugPlayerCount(
                        (success) => { Debug.Log(success); }, 
                        (failure) => { });
                },
                (failure) => { }
            );
    }
}
