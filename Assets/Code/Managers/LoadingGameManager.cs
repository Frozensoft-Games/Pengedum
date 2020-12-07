using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingGameManager : MonoBehaviour
{
    public GameObject FsgLogo;
    public GameObject HjelpeLinjenLogo;

    public float FsgrotationDegreesPerSecond = 45f;
    public float FsgrotationDegreesAmount = 90f;
    private float FsgtotalRotation = 0;

    public float HLrotationDegreesPerSecond = 45f;
    public float HLrotationDegreesAmount = 90f;
    private float HLtotalRotation = 0;

    private float degreesPerSecond = 60f;

    void FixedUpdate()
    {
        //if we haven't reached the desired rotation, swing
        if (Mathf.Abs(FsgtotalRotation) < Mathf.Abs(FsgrotationDegreesAmount))
            AnimateFsgLogo();
        else if (Mathf.Abs(HLtotalRotation) < Mathf.Abs(HLrotationDegreesAmount) && !(Mathf.Abs(FsgtotalRotation) < Mathf.Abs(FsgrotationDegreesAmount)))
        {
            FsgLogo.gameObject.SetActive(false);
            AnimateHjelpeLinjenLogo();
        }
        else
        {
            HjelpeLinjenLogo.gameObject.SetActive(false);
            SceneManager.LoadSceneAsync(1);
        }
    }

    void AnimateFsgLogo()
    {
        var currentAngle = FsgLogo.transform.rotation.eulerAngles.y;
        FsgLogo.transform.rotation =
            Quaternion.AngleAxis(currentAngle + (Time.deltaTime * degreesPerSecond), Vector3.up);
        FsgtotalRotation += Time.deltaTime * degreesPerSecond;
        FsgLogo.gameObject.SetActive(true);
    }

    private void AnimateHjelpeLinjenLogo()
    {
        var currentAngle = HjelpeLinjenLogo.transform.rotation.eulerAngles.y;
        HjelpeLinjenLogo.transform.rotation =
            Quaternion.AngleAxis(currentAngle + (Time.deltaTime * degreesPerSecond), Vector3.up);
        HLtotalRotation += Time.deltaTime * degreesPerSecond;
        HjelpeLinjenLogo.gameObject.SetActive(true);
    }
}
