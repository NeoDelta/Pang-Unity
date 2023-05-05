using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void OnStartGame()
    {
        GameManager.Instance.StartGame();
    }

    public void OnQuitGame()
    {
        GameManager.Instance.QuitGame(); ;
    }
}
