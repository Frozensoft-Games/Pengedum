using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsManager : MonoBehaviour
{
    void Start()
    {
        if (GameManager.instance == null)
            SceneManager.LoadSceneAsync((int)SceneIndexes.MANAGER);
    }

    public void Back()
    {
        GameManager.instance.LoadMainMenu();
    }

    public void GetHelp()
    {
        Application.OpenURL("https://hjelpelinjen.no/");
    }
}
