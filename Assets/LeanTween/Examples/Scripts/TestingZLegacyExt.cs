using UnityEngine;
using System.Collections;
using System;
using System.Threading;
using DentedPixel;


public class TestingZLegacyExt : MonoBehaviour {

	public AnimationCurve customAnimationCurve;
    public Transform pt1;
    public Transform pt2;
    public Transform pt3;
    public Transform pt4;
    public Transform pt5;

    public delegate void NextFunc();
    private int exampleIter = 0;
    private readonly string[] exampleFunctions = new string[] { /**/"updateValue3Example", "loopTestClamp", "loopTestPingPong", "moveOnACurveExample", "customTweenExample", "moveExample", "rotateExample", "scaleExample", "updateValueExample", "delayedCallExample", "alphaExample", "moveLocalExample", "rotateAroundExample", "colorExample" };
    public bool useEstimatedTime = true;
    private Transform ltLogo;
    private TimingType timingType = TimingType.SteadyNormalTime;
    private int descrTimeScaleChangeId;
    private Vector3 origin;

    public enum TimingType
    {
        SteadyNormalTime,
        IgnoreTimeScale,
        HalfTimeScale,
        VariableTimeScale,
        Length
    }

    void Awake()
    {
        // LeanTween.init(3200); // This line is optional. Here you can specify the maximum number of tweens you will use (the default is 400).  This must be called before any use of LeanTween is made for it to be effective.
    }

    void Start()
    {
        ltLogo = GameObject.Find("LeanTweenLogo").transform;
        LeanTween.delayedCall(1f, CycleThroughExamples);
        origin = ltLogo.position;

        //      alphaExample();
    }

    void PauseNow()
    {
        Time.timeScale = 0f;
        Debug.Log("pausing");
    }

    void OnGUI()
    {
        string label = useEstimatedTime ? "useEstimatedTime" : "timeScale:" + Time.timeScale;
        GUI.Label(new Rect(0.03f * Screen.width, 0.03f * Screen.height, 0.5f * Screen.width, 0.3f * Screen.height), label);
    }

    void EndlessCallback()
    {
        Debug.Log("endless");
    }

    void CycleThroughExamples()
    {
        if (exampleIter == 0)
        {
            int iter = (int)timingType + 1;
            if (iter > (int)TimingType.Length)
                iter = 0;
            timingType = (TimingType)iter;
            useEstimatedTime = timingType == TimingType.IgnoreTimeScale;
            Time.timeScale = useEstimatedTime ? 0 : 1f; // pause the Time Scale to show the effectiveness of the useEstimatedTime feature (this is very usefull with Pause Screens)
            if (timingType == TimingType.HalfTimeScale)
                Time.timeScale = 0.5f;

            if (timingType == TimingType.VariableTimeScale)
            {
				descrTimeScaleChangeId = gameObject.LeanValue(0.01f, 10.0f, 3f).setOnUpdate((float val) => {
                    //Debug.Log("timeScale val:"+val);
                    Time.timeScale = val;
                }).setEase(LeanTweenType.easeInQuad).setUseEstimatedTime(true).setRepeat(-1).id;
            }
            else
            {
                Debug.Log("cancel variable time");
                LeanTween.cancel(descrTimeScaleChangeId);
            }
        }
        gameObject.BroadcastMessage(exampleFunctions[exampleIter]);

        // Debug.Log("cycleThroughExamples time:"+Time.time + " useEstimatedTime:"+useEstimatedTime);
        float delayTime = 1.1f;
		gameObject.LeanDelayedCall( delayTime, CycleThroughExamples).setUseEstimatedTime(useEstimatedTime);

        exampleIter = exampleIter + 1 >= exampleFunctions.Length ? 0 : exampleIter + 1;
    }

    public void UpdateValue3Example()
    {
        Debug.Log("updateValue3Example Time:" + Time.time);
		gameObject.LeanValue( UpdateValue3ExampleCallback, new Vector3(0.0f, 270.0f, 0.0f), new Vector3(30.0f, 270.0f, 180f), 0.5f).setEase(LeanTweenType.easeInBounce).setRepeat(2).setLoopPingPong().setOnUpdateVector3(UpdateValue3ExampleUpdate).setUseEstimatedTime(useEstimatedTime);
    }

    public void UpdateValue3ExampleUpdate(Vector3 val)
    {
        //Debug.Log("val:"+val+" obj:"+obj);
    }

    public void UpdateValue3ExampleCallback(Vector3 val)
    {
        ltLogo.transform.eulerAngles = val;
        // Debug.Log("updateValue3ExampleCallback:"+val);
    }

    public void LoopTestClamp()
    {
        Debug.Log("loopTestClamp Time:" + Time.time);
		Transform cube1 = GameObject.Find("Cube1").transform;
        cube1.localScale = new Vector3(1.0f, 1.0f, 1.0f);
		cube1.LeanScaleZ( 4.0f, 1.0f).setEase(LeanTweenType.easeOutElastic).setRepeat(7).setLoopClamp().setUseEstimatedTime(useEstimatedTime);//
    }

    public void LoopTestPingPong()
    {
        Debug.Log("loopTestPingPong Time:" + Time.time);
		Transform cube2 = GameObject.Find("Cube2").transform;
        cube2.localScale = new Vector3(1.0f, 1.0f, 1.0f);
		cube2.LeanScaleY( 4.0f, 1.0f).setEase(LeanTweenType.easeOutQuad).setLoopPingPong(4).setUseEstimatedTime(useEstimatedTime);
        //LeanTween.scaleY( cube2, 4.0f, 1.0f, LeanTween.options().setEaseOutQuad().setRepeat(8).setLoopPingPong().setUseEstimatedTime(useEstimatedTime) );
    }

    public void ColorExample()
    {
        GameObject lChar = GameObject.Find("LCharacter");
		lChar.LeanColor( new Color(1.0f, 0.0f, 0.0f, 0.5f), 0.5f).setEase(LeanTweenType.easeOutBounce).setRepeat(2).setLoopPingPong().setUseEstimatedTime(useEstimatedTime);
    }

    public void CustomTweenExample()
    {
        Debug.Log("customTweenExample starting pos:" + ltLogo.position + " origin:" + origin);

		ltLogo.LeanMoveX(-10.0f, 0.5f).setEase(customAnimationCurve).setUseEstimatedTime(useEstimatedTime);
		ltLogo.LeanMoveX( 0.0f, 0.5f).setEase(customAnimationCurve).setDelay(0.5f).setUseEstimatedTime(useEstimatedTime);
    }

    public void MoveExample()
    {
        Debug.Log("moveExample");

		ltLogo.LeanMove(new Vector3(-2f, -1f, 0f), 0.5f).setUseEstimatedTime(useEstimatedTime);
		ltLogo.LeanMove(origin, 0.5f).setDelay(0.5f).setUseEstimatedTime(useEstimatedTime);
    }

    public void RotateExample()
    {
        Debug.Log("rotateExample");

        Hashtable returnParam = new Hashtable {{"yo", 5.0}};

        ltLogo.LeanRotate(new Vector3(0f, 360f, 0f), 1f).setEase(LeanTweenType.easeOutQuad).setOnComplete(RotateFinished).setOnCompleteParam(returnParam).setOnUpdate(RotateOnUpdate).setUseEstimatedTime(useEstimatedTime);
    }

    public static void RotateOnUpdate(float val)
    {
        //Debug.Log("rotating val:"+val);
    }

    public void RotateFinished(object hash)
    {
        Hashtable h = hash as Hashtable;
        Debug.Log("rotateFinished hash:" + h["yo"]);
    }

    public void ScaleExample()
    {
        Debug.Log("scaleExample");

        Vector3 currentScale = ltLogo.localScale;
		ltLogo.LeanScale(new Vector3(currentScale.x + 0.2f, currentScale.y + 0.2f, currentScale.z + 0.2f), 1f).setEase(LeanTweenType.easeOutBounce).setUseEstimatedTime(useEstimatedTime);
    }

    public void UpdateValueExample()
    {
        Debug.Log("updateValueExample");
        Hashtable pass = new Hashtable {{"message", "hi"}};
        gameObject.LeanValue( (Action<float, object>)UpdateValueExampleCallback, ltLogo.eulerAngles.y, 270f, 1f).setEase(LeanTweenType.easeOutElastic).setOnUpdateParam(pass).setUseEstimatedTime(useEstimatedTime);
    }

    public void UpdateValueExampleCallback(float val, object hash)
    {
        // Hashtable h = hash as Hashtable;
        // Debug.Log("message:"+h["message"]+" val:"+val);
        Vector3 tmp = ltLogo.eulerAngles;
        tmp.y = val;
        ltLogo.transform.eulerAngles = tmp;
    }

    public void DelayedCallExample()
    {
        Debug.Log("delayedCallExample");

        LeanTween.delayedCall(0.5f, DelayedCallExampleCallback).setUseEstimatedTime(useEstimatedTime);
    }

    public void DelayedCallExampleCallback()
    {
        Debug.Log("Delayed function was called");
        Vector3 currentScale = ltLogo.localScale;

		ltLogo.LeanScale( new Vector3(currentScale.x - 0.2f, currentScale.y - 0.2f, currentScale.z - 0.2f), 0.5f).setEase(LeanTweenType.easeInOutCirc).setUseEstimatedTime(useEstimatedTime);
    }

    public void AlphaExample()
    {
        Debug.Log("alphaExample");

        GameObject lChar = GameObject.Find("LCharacter");
		lChar.LeanAlpha( 0.0f, 0.5f).setUseEstimatedTime(useEstimatedTime);
		lChar.LeanAlpha( 1.0f, 0.5f).setDelay(0.5f).setUseEstimatedTime(useEstimatedTime);
    }

    public void MoveLocalExample()
    {
        Debug.Log("moveLocalExample");

        GameObject lChar = GameObject.Find("LCharacter");
        Vector3 origPos = lChar.transform.localPosition;
		lChar.LeanMoveLocal( new Vector3(0.0f, 2.0f, 0.0f), 0.5f).setUseEstimatedTime(useEstimatedTime);
		lChar.LeanMoveLocal( origPos, 0.5f).setDelay(0.5f).setUseEstimatedTime(useEstimatedTime);
    }

    public void RotateAroundExample()
    {
        Debug.Log("rotateAroundExample");

        GameObject lChar = GameObject.Find("LCharacter");
		lChar.LeanRotateAround(Vector3.up, 360.0f, 1.0f).setUseEstimatedTime(useEstimatedTime);
    }

    public void LoopPause()
    {
        GameObject cube1 = GameObject.Find("Cube1");
		cube1.LeanPause();
    }

    public void LoopResume()
    {
        GameObject cube1 = GameObject.Find("Cube1");
		cube1.LeanResume();
    }

    public void PunchTest()
    {
		ltLogo.LeanMoveX(7.0f, 1.0f).setEase(LeanTweenType.punch).setUseEstimatedTime(useEstimatedTime);
    }
}
