using Assets.Code.Saving___Loading.Profile_System.SelectProfile;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManagerEvents : MonoBehaviour
{
    public static SaveManagerEvents current;
    void Awake()
    {
        current = this;
    }

    public event Action<string, string, string> OnCreateProfile;
    public void CreateProfile(string profileName, string fullProfileName, string profileId)
    {
        OnCreateProfile?.Invoke(profileName, fullProfileName, profileId);
    }

    public event Action<ProfileData> OnProfileSaved;
    public void ProfileSaved(ProfileData createdProfile)
    {
        OnProfileSaved?.Invoke(createdProfile);
    }
    public event Action<ProfileData> OnProfileLoaded;
    public void ProfileLoaded(ProfileData loadedProfile)
    {
        OnProfileLoaded?.Invoke(loadedProfile);
    }

    public event Action<SelectedProfile> OnProfileDeleted;
    public void ProfileDeleted(SelectedProfile deletedProfile)
    {
        OnProfileDeleted?.Invoke(deletedProfile);
    }

    public event Action<ProfileData> OnProfileEdited;
    public void ProfileEdited(ProfileData editedProfile)
    {
        OnProfileEdited?.Invoke(editedProfile);
    }

    public event Action<List<ProfilesData>> OnAllProfilesSaved;
    public void AllProfilesSaved(List<ProfilesData> profilesData)
    {
        OnAllProfilesSaved?.Invoke(profilesData);
    }
    public event Action<List<ProfilesData>> OnAllProfilesLoaded;
    public void AllProfilesLoaded(List<ProfilesData> profilesData)
    {
        OnAllProfilesLoaded?.Invoke(profilesData);
    }

    public event Action<GameData, SelectedProfile> OnLoadGame;
    public void LoadGame(GameData gameData, SelectedProfile selectedProfile)
    {
        OnLoadGame?.Invoke(gameData, selectedProfile);
    }

    public event Action<GameData, SelectedProfile> OnSaveGame;
    public void SaveGame(GameData gameData, SelectedProfile selectedProfile)
    {
        OnSaveGame?.Invoke(gameData, selectedProfile);
    }


    public event Action<GameSettings, SelectedProfile> OnLoadOptions;
    public void LoadOptions(GameSettings gameSettings, SelectedProfile selectedProfile)
    {
        OnLoadOptions?.Invoke(gameSettings, selectedProfile);
    }

    public event Action<GameSettings, SelectedProfile> OnSaveOptions;
    public void SaveOptions(GameSettings gameSettings, SelectedProfile selectedProfile)
    {
        OnSaveOptions?.Invoke(gameSettings, selectedProfile);
    }
}