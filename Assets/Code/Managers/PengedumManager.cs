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
        Debug.Log("You are currently playing the game, GG!");
    }

    // Options for the game
    public void Options()
    {
        Debug.Log("You are currently editing the settings. GG!");
    }
}
