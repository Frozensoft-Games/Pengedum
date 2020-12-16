using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    public static InGameManager instance;

    public GameObject uI;

    void Awake()
    {
        instance = this;
    }

    public void PlayAnimation()
    {
        GameSaveManager.stopBalance = true;
        GameSaveManager.hasPlayed = true;
        uI.SetActive(true);
        StartCoroutine(Close());
    }

    IEnumerator Close()
    {
        yield return new WaitForSeconds(15);
        CloseBtn();
    }

    public void CloseBtn()
    {
        GameManager.instance.LoadCredits();
    }
}
