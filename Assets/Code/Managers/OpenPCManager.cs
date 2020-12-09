using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenPCManager : MonoBehaviour
{
    public GameObject pcUI;
    public GameObject screenLight;

    public static bool isOpen = false;

    void Start()
    {
        pcUI.SetActive(false);
        screenLight.SetActive(false);
    }

    void OnTriggerEnter(Collider col)
    {
        if (!col.tag.Equals("player", StringComparison.OrdinalIgnoreCase)) return;
        pcUI.SetActive(true);
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
        pcUI.SetActive(false);
        screenLight.SetActive(true);
        ScreenshotHandler.TakeScreenshot_Static(1920,1080);
    }

    public void No()
    {
        pcUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        isOpen = false;
    }
}
