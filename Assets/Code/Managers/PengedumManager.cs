using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        GameManager.instance.LoadProfiles();
    }

    // Options for the game
    public void Options()
    {
        Debug.Log("Coming Soon!");
    }
}
