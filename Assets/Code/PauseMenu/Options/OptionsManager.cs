using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Code.Saving___Loading.Profile_System.SelectProfile;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Audio;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    public Toggle fullscreenToggle;
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown textureQualityDropdown;
    public TMP_Dropdown antialiasingDropdown;
    public TMP_Dropdown vSyncDropdown;
    public Slider volumeSlider;

    public Resolution[] resolutions;
    public string[] qualitySettings;

    public GameSettings gameSettings;

    public AudioMixer audioMixer;

    void OnEnable()
    {
        SaveManagerEvents.current.OnLoadOptions += OnLoadOptions;
        SaveManagerEvents.current.OnSaveOptions += OnSaveOptions;

        gameSettings = new GameSettings();

        fullscreenToggle.onValueChanged.AddListener(delegate { OnFullscreenToggle(); });
        resolutionDropdown.onValueChanged.AddListener(delegate { OnResolutionChange(); });
        textureQualityDropdown.onValueChanged.AddListener(delegate { OnTextureQualityChange(); });
        antialiasingDropdown.onValueChanged.AddListener(delegate { OnAntialiasingChange(); });
        vSyncDropdown.onValueChanged.AddListener(delegate { OnVSyncChange(); });
        volumeSlider.onValueChanged.AddListener(delegate { OnVolumeChange(); });

        // Add all resolutions to resolution Dropdown
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        foreach (var resolution in resolutions.Reverse())
        {
            resolutionDropdown.options.Add(new TMP_Dropdown.OptionData(resolution.ToString()));
        }

        // Add all quality settings to textureQuality Dropdown
        qualitySettings = QualitySettings.names;
        textureQualityDropdown.ClearOptions();
        foreach (var quality in qualitySettings.Reverse())
        {
            textureQualityDropdown.options.Add(new TMP_Dropdown.OptionData(quality));
        }

        LoadSettings();
    }

    public void OnFullscreenToggle()
    {
        gameSettings.fullscreen = Screen.fullScreen = fullscreenToggle.isOn;
        SaveSettings();
    }

    public void OnResolutionChange()
    {
        Screen.SetResolution(resolutions[resolutionDropdown.value].width, resolutions[resolutionDropdown.value].height, Screen.fullScreenMode);
        gameSettings.resolutionIndex = resolutionDropdown.value;
    }

    public void OnTextureQualityChange()
    {
        QualitySettings.masterTextureLimit = gameSettings.textureQuality = textureQualityDropdown.value;
        SaveSettings();
    }

    public void OnAntialiasingChange()
    {
        QualitySettings.antiAliasing = gameSettings.antialiasing = (int) Mathf.Pow(2f, antialiasingDropdown.value);
    }

    public void OnVSyncChange()
    {
        QualitySettings.vSyncCount = gameSettings.vSync = vSyncDropdown.value;
    }

    public void OnVolumeChange()
    {
        audioMixer.SetFloat("volume", volumeSlider.value);
        gameSettings.volume = volumeSlider.value;
    }

    public async void SaveSettings()
    {
        await SaveManager.SaveOptionsAsync(gameSettings);
    }

    public async void LoadSettings()
    {
        gameSettings =  await SaveManager.LoadOptionsAsync() ?? new GameSettings {volume = 1f,fullscreen = true, textureQuality = 0,resolutionIndex = 0};
        volumeSlider.value = gameSettings.volume;
        antialiasingDropdown.value = gameSettings.antialiasing;
        vSyncDropdown.value = gameSettings.vSync;
        textureQualityDropdown.value = gameSettings.textureQuality;
        resolutionDropdown.value = gameSettings.resolutionIndex;
        fullscreenToggle.isOn = gameSettings.fullscreen;
        Screen.fullScreen = gameSettings.fullscreen;
    }

    public void ApplyChanges()
    {
        SaveSettings();
    }

    public void OnLoadOptions(GameSettings gameSettings, SelectedProfile selectedProfile)
    {
        Debug.Log($"Loaded game settings for {selectedProfile.profileName}");
    }

    public void OnSaveOptions(GameSettings gameSettings, SelectedProfile selectedProfile)
    {
        Debug.Log($"Saved game settings for {selectedProfile.profileName}");
    }
}
