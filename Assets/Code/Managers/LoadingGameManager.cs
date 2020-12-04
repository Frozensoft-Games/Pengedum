using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingGameManager : MonoBehaviour
{
    public GameObject FsgLogo;

    public float rotationDegreesPerSecond = 45f;
    public float rotationDegreesAmount = 90f;
    private float totalRotation = 0;

    private float degreesPerSecond = 60f;

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if we haven't reached the desired rotation, swing

        if (Mathf.Abs(totalRotation) < Mathf.Abs(rotationDegreesAmount))
            SwingOpen();
    }

    void SwingOpen()
    {
        float currentAngle = FsgLogo.transform.rotation.eulerAngles.y;
        FsgLogo.transform.rotation =
            Quaternion.AngleAxis(currentAngle + (Time.deltaTime * degreesPerSecond), Vector3.up);
        totalRotation += Time.deltaTime * degreesPerSecond;
        FsgLogo.gameObject.SetActive(true);
    }
}
