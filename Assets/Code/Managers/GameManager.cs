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


    private void Awake()
    {
        instance = this;

        SceneManager.LoadSceneAsync((int) SceneIndexes.TITLE_SCREEN, LoadSceneMode.Additive);
    }

    private readonly List<AsyncOperation> scenesLoading = new List<AsyncOperation>();
    public void LoadGame()
    {
        backgroundImage.sprite = backgrounds[Random.Range(0, backgrounds.Length)];
        loadingScreen.gameObject.SetActive(true);

        StartCoroutine(GenerateTips());

        scenesLoading.Add(SceneManager.UnloadSceneAsync((int)SceneIndexes.TITLE_SCREEN));
        scenesLoading.Add(SceneManager.LoadSceneAsync((int) SceneIndexes.MAP, LoadSceneMode.Additive));

        StartCoroutine(GetSceneLoadProgress());
    }

    public void LoadMainMenu()
    {
        backgroundImage.sprite = backgrounds[Random.Range(0, backgrounds.Length)];
        loadingScreen.gameObject.SetActive(true);

        StartCoroutine(GenerateTips());

        scenesLoading.Add(SceneManager.UnloadSceneAsync((int)SceneIndexes.MAP));
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
