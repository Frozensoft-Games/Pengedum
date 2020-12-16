using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PengedumManager : MonoBehaviour
{
    void Start()
    {
        if (GameManager.instance == null) 
            SceneManager.LoadSceneAsync((int)SceneIndexes.MANAGER);
    }

    // Quit the game
    public void Quit()
    {
        Application.Quit();
    }

    // Play the game
    public void Play()
    {
        GameManager.instance.LoadProfiles();
    }

    // Credits
    public void Credits()
    {
        GameManager.instance.LoadCredits();
    }
}
