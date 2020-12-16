using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenPCManager : MonoBehaviour
{
    public static OpenPCManager instance;

    public GameObject pcUi;
    public GameObject screenLight;
    public GameObject pcMenu;

    public static bool isOpen = false;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        pcUi.SetActive(false);
        screenLight.SetActive(false);
    }

    void OnTriggerEnter(Collider col)
    {
        if (!col.tag.Equals("player", StringComparison.OrdinalIgnoreCase)) return;
        pcUi.SetActive(true);
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
        pcUi.SetActive(false);
        screenLight.SetActive(true);
        pcMenu.SetActive(true);
    }

    public void No()
    {
        pcUi.SetActive(false);
        screenLight.SetActive(false);
        pcMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        isOpen = false;
    }
}
