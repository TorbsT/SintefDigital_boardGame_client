using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroUIManager : MonoBehaviour
{
    [SerializeField] private string mainMenuScene = "MainMenu";
    public void Login()
    {
        SceneManager.LoadSceneAsync(mainMenuScene);
    }
}
