using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenPCManager : MonoBehaviour
{
    public GameObject pcUi;
    public GameObject screenLight;
    public GameObject pcMenu;

    public GameObject goodChoice;

    public static bool isOpen;

    void Start()
    {
        isOpen = false;
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
        if (pcMenu.gameObject.activeInHierarchy)
            StartCoroutine(OpenGoodChoice());

        pcUi.SetActive(false);
        screenLight.SetActive(false);
        pcMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        isOpen = false;
    }

    public void PlayOnPc()
    {
        InGameManager.instance.PlayAnimation();
    }

    IEnumerator OpenGoodChoice()
    {
        goodChoice.SetActive(true);
        yield return new WaitForSeconds(2);
        goodChoice.SetActive(false);
    }
}
