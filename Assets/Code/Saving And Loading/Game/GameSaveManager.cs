using Assets.Code.Saving___Loading.Profile_System.SelectProfile;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSaveManager : MonoBehaviour
{
    public static GameSaveManager instance;

    public GameObject autoSave;

    public GameObject player;
    public GameObject playerCamera;

    public static bool hasPlayed;

    public static int balance;

    public static bool reload = false;

    void Awake()
    {
        instance = this;
    }

    async void Start()
    {
        if (SelectedProfileManager.selectedProfile is null)
        {
            Debug.Log("Failed to load game.");
            if (GameManager.instance == null)
                SceneManager.LoadSceneAsync((int)SceneIndexes.MANAGER);

            Cursor.lockState = CursorLockMode.None;
            return;
        }

        SaveManagerEvents.current.OnSaveGame += OnSaveGame;
        SaveManagerEvents.current.OnLoadGame += OnLoadGame;

        await Load();
    }

    async Task Load()
    {
        var gameData = await SaveManager.LoadGameAsync();
        if (gameData == null)
        {
            Debug.Log("Failed to load game data.");

            await SaveManager.SaveGameAsync(false, 1000, player.transform.position, player.transform.rotation, playerCamera.transform.position,playerCamera.transform.rotation);

            await Load();
            return;
        }

        player.transform.SetPositionAndRotation(gameData.playerPosition, gameData.playerRotation);
        playerCamera.transform.rotation = gameData.cameraRotation;
        balance = gameData.balance;
        hasPlayed = gameData.hasPlayed;

        Debug.Log($"Balance: {balance} hasPlayed: {hasPlayed}");

        EnableAutoSave(10);
    }

    public async void Save()
    {
        await SaveManager.SaveGameAsync(hasPlayed, balance, player.transform.position, player.transform.rotation, playerCamera.transform.position, playerCamera.transform.rotation);
    }

    void EnableAutoSave(int time = 3)
    {
        StartCoroutine(AutoSave(time));
    }

    IEnumerator AutoSave(int time)
    {
        while (!reload)
        {
            yield return new WaitForSeconds(time);
            StartCoroutine(AutoSaveBox());
            Save();
            ScreenshotHandler.TakeScreenshot_Static(225,225);
        }
    }

    IEnumerator AutoSaveBox()
    {
        autoSave.gameObject.SetActive(true);
        yield return new WaitForSeconds(5);
        autoSave.gameObject.SetActive(false);
    }

    private void OnSaveGame(GameData gameData, SelectedProfile selectedProfile)
    {
        Debug.Log($"Successfully Saved Game for: {selectedProfile.profileName}");
    }

    private void OnLoadGame(GameData gameData, SelectedProfile selectedProfile)
    {
        Debug.Log($"Successfully Loaded Game for: {selectedProfile.profileName}");
    }
}
