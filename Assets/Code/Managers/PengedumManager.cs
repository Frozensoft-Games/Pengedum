using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PengedumManager : MonoBehaviour
{
    // Quit the game
    public void Quit()
    {
        Application.Quit();
    }

    // Play the game
    public void Play()
    {
        GameManager.instance.LoadGame();
    }

    // Options for the game
    public void Options()
    {
        Debug.Log("You are currently editing the settings. GG!");
    }
}
