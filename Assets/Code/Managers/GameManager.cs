using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    /* Related to Loading Screen */
    public GameObject loadingScreen;
    public Slider loadingBar;
    public TextMeshProUGUI loadingText;
    public TextMeshProUGUI loadingTipText;
    public CanvasGroup loadingTipTextAlphaCanvas;
    public string[] tips;
    /* /End Related to Loading Screen */

    /* Background Related Stuff */
    public Sprite[] backgrounds;
    public Image backgroundImage;
    /* /End Background Related Stuff */

    // For the first loading screen.
    private bool loadingScreenFirst = false;

    public GameObject logoCanvas;
    public GameObject fsgLogo;
    public GameObject hjelpeLinjenLogo;

    public float fsgrotationDegreesPerSecond = 45f;
    public float fsgrotationDegreesAmount = 90f;
    private float fsgtotalRotation = 0;

    public float hLrotationDegreesPerSecond = 45f;
    public float hLrotationDegreesAmount = 90f;
    private float hLtotalRotation = 0;

    private float degreesPerSecond = 60f;

    void FixedUpdate()
    {
        if (loadingScreenFirst) return;
        //if we haven't reached the desired rotation, swing
        if (Mathf.Abs(fsgtotalRotation) < Mathf.Abs(fsgrotationDegreesAmount))
            AnimateFsgLogo();
        else if (Mathf.Abs(hLtotalRotation) < Mathf.Abs(hLrotationDegreesAmount) && !(Mathf.Abs(fsgtotalRotation) < Mathf.Abs(fsgrotationDegreesAmount)))
        {
            fsgLogo.gameObject.SetActive(false);
            AnimateHjelpeLinjenLogo();
        }
        else
        {
            hjelpeLinjenLogo.gameObject.SetActive(false);
            logoCanvas.gameObject.SetActive(false);
            loadingScreenFirst = true;
            LoadMainMenu();
        }
    }

    void AnimateFsgLogo()
    {
        var currentAngle = fsgLogo.transform.rotation.eulerAngles.y;
        fsgLogo.transform.rotation =
            Quaternion.AngleAxis(currentAngle + (Time.deltaTime * degreesPerSecond), Vector3.up);
        fsgtotalRotation += Time.deltaTime * degreesPerSecond;
        fsgLogo.gameObject.SetActive(true);
    }

    private void AnimateHjelpeLinjenLogo()
    {
        var currentAngle = hjelpeLinjenLogo.transform.rotation.eulerAngles.y;
        hjelpeLinjenLogo.transform.rotation =
            Quaternion.AngleAxis(currentAngle + (Time.deltaTime * degreesPerSecond), Vector3.up);
        hLtotalRotation += Time.deltaTime * degreesPerSecond;
        hjelpeLinjenLogo.gameObject.SetActive(true);
    }


    private void Awake()
    {
        instance = this;

        if(loadingScreenFirst)
            SceneManager.LoadSceneAsync((int) SceneIndexes.TITLE_SCREEN, LoadSceneMode.Additive);
    }

    private readonly List<AsyncOperation> scenesLoading = new List<AsyncOperation>();
    public void LoadGame()
    {
        if (SceneManager.GetSceneByBuildIndex((int)SceneIndexes.MAP).isLoaded) return;
        backgroundImage.sprite = backgrounds[Random.Range(0, backgrounds.Length)];
        loadingScreen.gameObject.SetActive(true);

        StartCoroutine(GenerateTips());

        scenesLoading.Add(SceneManager.UnloadSceneAsync((int) SceneIndexes.PROFILES));
        scenesLoading.Add(SceneManager.LoadSceneAsync((int) SceneIndexes.MAP, LoadSceneMode.Additive));

        StartCoroutine(GetSceneLoadProgress());
    }

    public void LoadCredits()
    {
        if (SceneManager.GetSceneByBuildIndex((int)SceneIndexes.CREDITS).isLoaded) return;
        backgroundImage.sprite = backgrounds[Random.Range(0, backgrounds.Length)];
        loadingScreen.gameObject.SetActive(true);

        StartCoroutine(GenerateTips());

        if(SceneManager.GetSceneByBuildIndex((int)SceneIndexes.TITLE_SCREEN).isLoaded)
            scenesLoading.Add(SceneManager.UnloadSceneAsync((int)SceneIndexes.TITLE_SCREEN));
        if (SceneManager.GetSceneByBuildIndex((int)SceneIndexes.MAP).isLoaded)
            scenesLoading.Add(SceneManager.UnloadSceneAsync((int)SceneIndexes.MAP));

        scenesLoading.Add(SceneManager.LoadSceneAsync((int)SceneIndexes.CREDITS, LoadSceneMode.Additive));

        StartCoroutine(GetSceneLoadProgress());
    }

    public void LoadProfiles()
    {
        if (SceneManager.GetSceneByBuildIndex((int)SceneIndexes.PROFILES).isLoaded) return;

        backgroundImage.sprite = backgrounds[Random.Range(0, backgrounds.Length)];
        loadingScreen.gameObject.SetActive(true);

        StartCoroutine(GenerateTips());

        if (SceneManager.GetSceneByBuildIndex((int)SceneIndexes.TITLE_SCREEN).isLoaded)
            scenesLoading.Add(SceneManager.UnloadSceneAsync((int)SceneIndexes.TITLE_SCREEN));

        scenesLoading.Add(SceneManager.LoadSceneAsync((int)SceneIndexes.PROFILES, LoadSceneMode.Additive));

        StartCoroutine(GetSceneLoadProgress());
    }
    public void LoadMainMenu()
    {
        if (SceneManager.GetSceneByBuildIndex((int)SceneIndexes.TITLE_SCREEN).isLoaded) return;

        backgroundImage.sprite = backgrounds[Random.Range(0, backgrounds.Length)];
        loadingScreen.gameObject.SetActive(true);

        StartCoroutine(GenerateTips());

        if (SceneManager.GetSceneByBuildIndex((int)SceneIndexes.MAP).isLoaded)
            scenesLoading.Add(SceneManager.UnloadSceneAsync((int)SceneIndexes.MAP));

        if(SceneManager.GetSceneByBuildIndex((int)SceneIndexes.PROFILES).isLoaded)
            scenesLoading.Add(SceneManager.UnloadSceneAsync((int)SceneIndexes.PROFILES));

        if (SceneManager.GetSceneByBuildIndex((int)SceneIndexes.CREDITS).isLoaded)
            scenesLoading.Add(SceneManager.UnloadSceneAsync((int)SceneIndexes.CREDITS));

        scenesLoading.Add(SceneManager.LoadSceneAsync((int)SceneIndexes.TITLE_SCREEN, LoadSceneMode.Additive));

        StartCoroutine(GetSceneLoadProgress());
    }

    private float totalSceneProgress;
    public IEnumerator GetSceneLoadProgress()
    {
        for(int i= 0; i<scenesLoading.Count; i++)
        {
            while (!scenesLoading[i].isDone)
            {
                totalSceneProgress = 0;

                foreach (AsyncOperation operation in scenesLoading)
                {
                    totalSceneProgress += operation.progress;
                }

                totalSceneProgress = (totalSceneProgress / scenesLoading.Count) * 100f;

                loadingBar.value = Mathf.RoundToInt(totalSceneProgress);

                loadingText.text = $"Loading Environments: {totalSceneProgress}%";

                yield return null;
            }
        }

        loadingScreen.gameObject.SetActive(false);
    }

    public int tipCount;
    public IEnumerator GenerateTips()
    {
        tipCount = Random.Range(0, tips.Length);
        loadingTipText.text = tips[tipCount];
        while (loadingScreen.activeInHierarchy)
        {
            yield return new WaitForSeconds(3f);

            LeanTween.alphaCanvas(loadingTipTextAlphaCanvas, 0, 0.5f);

            tipCount++;
            if (tipCount >= tips.Length) tipCount = 0;

            loadingTipText.text = tips[tipCount];

            LeanTween.alphaCanvas(loadingTipTextAlphaCanvas, 1, 0.5f);
        }
    }
}