using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorManager : MonoBehaviour
{
    public GameObject doorUI;

    public static bool isOpen = false;

    void Start()
    {
        doorUI.SetActive(false);
    }

    void OnTriggerEnter(Collider col)
    {
        if (!col.tag.Equals("player", StringComparison.OrdinalIgnoreCase)) return;
        doorUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        isOpen = true;
    }

    void OnTriggerExit(Collider col)
    {
        if (!col.tag.Equals("player", StringComparison.OrdinalIgnoreCase)) return;
        No();
    }

    public void Yes()
    {
        GameManager.instance.LoadMainMenu();
    }

    public void No()
    {
        doorUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        isOpen = false;
    }
}
