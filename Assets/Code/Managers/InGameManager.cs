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

    public GameObject moneyUi;

    public GameObject closeBtn;

    void Awake()
    {
        instance = this;
    }

    public void PlayAnimation()
    {
        GameSaveManager.hasPlayed = true;
        uI.SetActive(true);
        moneyUi.SetActive(false);
        GameSaveManager.instance.Save();
        StartCoroutine(Close());
        StartCoroutine(ShowCloseBtn());
    }

    IEnumerator Close()
    {
        yield return new WaitForSeconds(15);
        CloseBtn();
    }

    IEnumerator ShowCloseBtn()
    {
        closeBtn.SetActive(false);
        yield return new WaitForSeconds(5);
        closeBtn.SetActive(true);
    }

    public void CloseBtn()
    {
        GameManager.instance.LoadCredits();
    }
}
