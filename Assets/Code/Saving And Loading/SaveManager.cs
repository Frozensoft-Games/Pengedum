using Assets.Code.Extensions;
using Assets.Code.Profile_System;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Assets.Code.Saving___Loading.Profile_System.SelectProfile;
using UnityEngine;

[Serializable]
public class ProfilesListWrapper
{
    public List<ProfilesData> Profiles;
}

public class SaveManager : MonoBehaviour
{

    // The encryption key for profiles.dat
    static readonly string JsonEncryptedKeyProfiles = "E!5Lasr**L;SX{h4976+?mD<hS(.>/.!";

    // This is the value used to store the Main directory path
    static readonly string MainDirectoryPath = Path.Combine(Application.persistentDataPath, "Profiles");

    // This is the value used to store the profiles.dat location
    static readonly string ProfilesPath = Path.Combine(MainDirectoryPath, "profiles.dat");

    // Profiles List
    public static List<ProfilesData> profiles = new List<ProfilesData>();

    // Used to save the specified profile.
    
    public static void SaveProfile(string path, string profileImage, string profileName, string profileId, string fullProfileName)
    {
        ProfileData profile = CreateProfile(profileId, profileImage, profileName, fullProfileName, false);

        if (profile == null) return;

        string json = JsonUtility.ToJson(profile);

        // Creates the unique Encryption key per profile
        string JSON_ENCRYPTED_KEY = fullProfileName + "*},+;))%.<'){>')']$-$-`=([{+\'" + fullProfileName + "&@";
        // If the key is shorter then 32 characters it will make it longer
        if (JSON_ENCRYPTED_KEY.Length < 33) while (JSON_ENCRYPTED_KEY.Length < 33) { JSON_ENCRYPTED_KEY = fullProfileName + "*},+;))%.<'){>')']$-$-`=([{+\'" + fullProfileName + "&@" + fullProfileName; }
        // If the value is longer then 32 characters it will make it 32 characters
        if (JSON_ENCRYPTED_KEY.Length > 32) JSON_ENCRYPTED_KEY = StringExtensions.Truncate(JSON_ENCRYPTED_KEY, 32);

        // Encryption process
        Rijndael crypto = new Rijndael();
        byte[] soup = crypto.Encrypt(json, JSON_ENCRYPTED_KEY);
        File.WriteAllBytes(path, soup);

        ProfilesData profiles = CreateProfilesData(profileId, profileName, fullProfileName);

        LoadAllProfiles(ProfilesPath);

        SaveManager.profiles.Add(profiles);

        SaveAllProfiles(ProfilesPath);
        SaveManagerEvents.current.ProfileSaved(profile);
    }

    public static async Task SaveProfileAsync(string path, string profileImage, string profileName, string profileId, string fullProfileName)
    {
        ProfileData profile = CreateProfile(profileId, profileImage, profileName, fullProfileName, false);

        if (profile == null) return;

        string json = JsonUtility.ToJson(profile);

        // Creates the unique Encryption key per profile
        string jsonEncryptedKey = $"{fullProfileName}BXRB98y-h^4^.ct^]~8|Cmn5([]+/+{fullProfileName}@&";
        // If the key is shorter then 32 characters it will make it longer
        if (jsonEncryptedKey.Length < 33) while (jsonEncryptedKey.Length < 33) { jsonEncryptedKey = $"{fullProfileName}BXRB98y-h^4^.ct^]~8|Cmn5([-+/{fullProfileName}@&{fullProfileName}"; }
        // If the value is longer then 32 characters it will make it 32 characters
        if (jsonEncryptedKey.Length > 32) jsonEncryptedKey = jsonEncryptedKey.Truncate(32);

        // Encryption process
        Rijndael crypto = new Rijndael();
        byte[] soup = crypto.Encrypt(json, jsonEncryptedKey);
        await AsyncHelperExtensions.WriteBytesAsync(path, soup);

        ProfilesData profiles = CreateProfilesData(profileId, profileName, fullProfileName);

        await LoadAllProfilesAsync(ProfilesPath);

        SaveManager.profiles.Add(profiles);

        await SaveAllProfilesAsync(ProfilesPath);

        SaveManagerEvents.current.ProfileSaved(profile);

    }

    // Used to load the specified profile.
    public static ProfileData LoadProfile(string path, string profileName, string profileId)
    {
        // This is the encrypted input from the file
        byte[] soupBackIn = File.ReadAllBytes(path);

        // Creates the unique Encryption key per profile
        string jsonEncryptedKey = profileName + $"BXRB98y-h^4^.ct^]~8|Cmn5([]+/+{profileName}@&";
        // If the key is shorter then 32 characters it will make it longer
        if (jsonEncryptedKey.Length < 33) while (jsonEncryptedKey.Length < 33) { jsonEncryptedKey = $"{profileName}BXRB98y-h^4^.ct^]~8|Cmn5([-+/{profileName}@&{profileName}"; }
        // If the value is longer then 32 characters it will make it 32 characters
        if (jsonEncryptedKey.Length > 32) jsonEncryptedKey = jsonEncryptedKey.Truncate(32);

        // Decrypting process
        Rijndael crypto = new Rijndael();
        string jsonFromFile = crypto.Decrypt(soupBackIn, jsonEncryptedKey);

        // Creates a ProfileData object with the information from the json file
        ProfileData profile = JsonUtility.FromJson<ProfileData>(jsonFromFile);

        // Checks if the decrypted Profile Data is null or not
        if (profile == null)
        {
            Debug.Log("Failed to load profile");
            return null;
        }
        if(profile.fullProfileName != profileName)
        {
            Debug.Log("Profile Name's are not matching");
            return null;
        }
        if (profile.profileId != profileId)
        {
            Debug.Log("Profile ID's are not matching");
            return null;
        }

        SaveManagerEvents.current.ProfileLoaded(profile);

        return profile;
    }

    public static async Task<ProfileData> LoadProfileAsync(string path, string profileName, string profileId)
    {
        // This is the encrypted input from the file
        byte[] soupBackIn = await AsyncHelperExtensions.ReadBytesAsync(path);

        // Creates the unique Encryption key per profile
        string jsonEncryptedKey = $"{profileName}BXRB98y-h^4^.ct^]~8|Cmn5([]+/+{profileName}@&";
        // If the key is shorter then 32 characters it will make it longer
        if (jsonEncryptedKey.Length < 33) while (jsonEncryptedKey.Length < 33) { jsonEncryptedKey = $"{profileName}BXRB98y-h^4^.ct^]~8|Cmn5([-+/{profileName}@&{profileName}"; }
        // If the value is longer then 32 characters it will make it 32 characters
        if (jsonEncryptedKey.Length > 32) jsonEncryptedKey = jsonEncryptedKey.Truncate(32);

        // Decrypting process
        Rijndael crypto = new Rijndael();
        string jsonFromFile = crypto.Decrypt(soupBackIn, jsonEncryptedKey);

        // Creates a ProfileData object with the information from the json file
        ProfileData profile = JsonUtility.FromJson<ProfileData>(jsonFromFile);

        // Checks if the decrypted Profile Data is null or not
        if (profile == null)
        {
            Debug.Log("Failed to load profile");
            return null;
        }
        if (profile.fullProfileName != profileName)
        {
            Debug.Log("Profile Name's are not matching");
            return null;
        }
        if (profile.profileId != profileId)
        {
            Debug.Log("Profile ID's are not matching");
            return null;
        }

        SaveManagerEvents.current.ProfileLoaded(profile);

        return profile;
    }

    // Load all profiles from profiles.dat 
    public static void LoadAllProfiles(string path)
    {
        // This is the encrypted input from the file
        byte[] soupBackIn = File.ReadAllBytes(path);

        // Decrypting process
        Rijndael crypto = new Rijndael();
        string jsonFromFile = crypto.Decrypt(soupBackIn, JsonEncryptedKeyProfiles);

        ProfilesListWrapper profiles = JsonUtility.FromJson<ProfilesListWrapper>(jsonFromFile);

        SaveManager.profiles = profiles == null ? new List<ProfilesData>() : profiles.Profiles;

        SaveManagerEvents.current.AllProfilesLoaded(SaveManager.profiles);
    }

    public static async Task LoadAllProfilesAsync(string path)
    {
        // This is the encrypted input from the file
        byte[] soupBackIn = await AsyncHelperExtensions.ReadBytesAsync(path);
        // Decrypting process
        Rijndael crypto = new Rijndael();
        string jsonFromFile = crypto.Decrypt(soupBackIn, JsonEncryptedKeyProfiles);

        ProfilesListWrapper profiles = JsonUtility.FromJson<ProfilesListWrapper>(jsonFromFile);

        SaveManager.profiles = profiles == null ? new List<ProfilesData>() : profiles.Profiles;

        SaveManagerEvents.current.AllProfilesLoaded(SaveManager.profiles);
    }

    // Save all profiles to profiles.dat
    public static void SaveAllProfiles(string path)
    {
        ProfilesListWrapper profiles = new ProfilesListWrapper() { Profiles = SaveManager.profiles };

        string json = JsonUtility.ToJson(profiles);

        // Encrypting process
        Rijndael crypto = new Rijndael();
        byte[] soup = crypto.Encrypt(json, JsonEncryptedKeyProfiles);

        File.WriteAllBytes(path, soup);

        SaveManagerEvents.current.AllProfilesSaved(SaveManager.profiles);
    }

    public static async Task SaveAllProfilesAsync(string path)
    {
        ProfilesListWrapper profiles = new ProfilesListWrapper() { Profiles = SaveManager.profiles };

        string json = JsonUtility.ToJson(profiles);

        // Encrypting process
        Rijndael crypto = new Rijndael();
        byte[] soup = crypto.Encrypt(json, JsonEncryptedKeyProfiles);

        await AsyncHelperExtensions.WriteBytesAsync(path, soup);

        SaveManagerEvents.current.AllProfilesSaved(SaveManager.profiles);
    }

    // Save Game
    public static async Task SaveGameAsync(bool hasPlayed, int balance, Vector3 playerPosition, Quaternion playerRotation, Vector3 cameraPosition, Quaternion cameraRotation)
    {
        if (SelectedProfileManager.selectedProfile == null) return;

        string fullProfileName = SelectedProfileManager.selectedProfile.fullProfileName;

        GameData gameData = CreateGameData(hasPlayed, balance, playerPosition, playerRotation, cameraPosition, cameraRotation);

        if (gameData == null) return;

        string path = Path.Combine(MainDirectoryPath, fullProfileName, "Save", "data.dat");
        string dirPath = Path.Combine(MainDirectoryPath, fullProfileName, "Save");

        if (!Directory.Exists(dirPath))
            FileManagerExtension.CreateDirectory(dirPath);

        string json = JsonUtility.ToJson(gameData);

        // Creates the unique Encryption key per profile
        string jsonEncryptedKey = "|9{Ajia:p,g<ae&)9KsLy7;<t9G5sJ>G";
        // If the value is longer then 32 characters it will make it 32 characters
        if (jsonEncryptedKey.Length > 32) jsonEncryptedKey = jsonEncryptedKey.Truncate(32);

        // Encryption process
        Rijndael crypto = new Rijndael();
        byte[] soup = crypto.Encrypt(json, jsonEncryptedKey);
        await AsyncHelperExtensions.WriteBytesAsync(path, soup);

        SaveManagerEvents.current.SaveGame(gameData, SelectedProfileManager.selectedProfile);
    }

    // Load Game
    public static async Task<GameData> LoadGameAsync()
    {
        if (SelectedProfileManager.selectedProfile == null) return null;

        string fullProfileName = SelectedProfileManager.selectedProfile.fullProfileName;

        string path = Path.Combine(MainDirectoryPath, fullProfileName, "Save", "data.dat");

        if (!File.Exists(path)) return null;

        // This is the encrypted input from the file
        byte[] soupBackIn = await AsyncHelperExtensions.ReadBytesAsync(path);

        // Creates the unique Encryption key per profile
        string jsonEncryptedKey = "|9{Ajia:p,g<ae&)9KsLy7;<t9G5sJ>G";
        // If the value is longer then 32 characters it will make it 32 characters
        if (jsonEncryptedKey.Length > 32) jsonEncryptedKey = jsonEncryptedKey.Truncate(32);

        // Decrypting process
        Rijndael crypto = new Rijndael();
        string jsonFromFile = crypto.Decrypt(soupBackIn, jsonEncryptedKey);

        // Creates a ProfileData object with the information from the json file
        GameData gameData = JsonUtility.FromJson<GameData>(jsonFromFile);

        // Checks if the decrypted Profile Data is null or not
        if (gameData == null)
        {
            Debug.Log("Failed to load game data.");
            return null;
        }

        SaveManagerEvents.current.LoadGame(gameData, SelectedProfileManager.selectedProfile);

        return gameData;
    }

    // Used to easily create a new ProfileData object
    public static ProfileData CreateProfile(string profileId, string profileImage, string profileName, string fullProfileName, bool isTutorialComplete)
    {
        ProfileData profile = new ProfileData
        {
            profileId = profileId,
            profileImage = profileImage,
            profileName = profileName,
            fullProfileName = fullProfileName,
            isTutorialComplete = isTutorialComplete
        };
        return profile;
    }

    // Used to easily create a new ProfilesData object
    static ProfilesData CreateProfilesData(string profileId, string profileName, string fullProfileName)
    {
        ProfilesData profile = new ProfilesData
        {
            profileId = profileId,
            profileName = profileName,
            fullProfileName = fullProfileName
        };
        return profile;
    }

    // Used to easily create a new GameData object
    static GameData CreateGameData(bool hasPlayed, int balance, Vector3 playerPosition, Quaternion playerRotation, Vector3 cameraPosition, Quaternion cameraRotation)
    {
        GameData gameData = new GameData
        {
            hasPlayed =  hasPlayed,
            balance = balance,
            playerPosition = playerPosition,
            playerRotation = playerRotation,
            cameraPosition = cameraPosition,
            cameraRotation = cameraRotation
        };
        return gameData;
    }
}
