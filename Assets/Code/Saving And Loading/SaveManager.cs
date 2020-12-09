using Assets.Code.Extensions;
using Assets.Code.Profile_System;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class ProfilesListWrapper
{
    public List<ProfilesData> Profiles;
}

public class SaveManager : MonoBehaviour
{

    // The encryption key for profiles.dat
    static readonly string JSON_ENCRYPTED_KEY_PROFILES = "e<goe451t_@&mG&1M-Qpx,CIp'_rci$i";

    // This is the value used to store the Main directory path
    static readonly string MAIN_DIRECTORY_PATH = Application.persistentDataPath + "/Profiles/";

    // This is the value used to store the profiles.dat location
    static readonly string PROFILES_PATH = MAIN_DIRECTORY_PATH + "/profiles.dat";

    // Profiles List
    public static List<ProfilesData> Profiles = new List<ProfilesData>();

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

        LoadAllProfiles(PROFILES_PATH);

        Profiles.Add(profiles);

        SaveAllProfiles(PROFILES_PATH);
        SaveManagerEvents.current.ProfileSaved(profile);
    }

    public static async Task SaveProfileAsync(string path, string profileImage, string profileName, string profileId, string fullProfileName)
    {
        ProfileData profile = CreateProfile(profileId, profileImage, profileName, fullProfileName, false);

        if (profile == null) return;

        string json = JsonUtility.ToJson(profile);

        // Creates the unique Encryption key per profile
        string jsonEncryptedKey = profileName + $"BXRB98y-h^4^.ct^]~8|Cmn5([]+/+{profileName}@&";
        // If the key is shorter then 32 characters it will make it longer
        if (jsonEncryptedKey.Length < 33) while (jsonEncryptedKey.Length < 33) { jsonEncryptedKey = $"{profileName}BXRB98y-h^4^.ct^]~8|Cmn5([-+/{profileName}@&{profileName}"; }
        // If the value is longer then 32 characters it will make it 32 characters
        if (jsonEncryptedKey.Length > 32) jsonEncryptedKey = jsonEncryptedKey.Truncate(32);

        // Encryption process
        Rijndael crypto = new Rijndael();
        byte[] soup = crypto.Encrypt(json, jsonEncryptedKey);
        await AsyncHelperExtensions.WriteBytesAsync(path, soup);

        ProfilesData profiles = CreateProfilesData(profileId, profileName, fullProfileName);

        await LoadAllProfilesAsync(PROFILES_PATH);

        Profiles.Add(profiles);

        await SaveAllProfilesAsync(PROFILES_PATH);

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
        string jsonFromFile = crypto.Decrypt(soupBackIn, JSON_ENCRYPTED_KEY_PROFILES);

        ProfilesListWrapper profiles = JsonUtility.FromJson<ProfilesListWrapper>(jsonFromFile);

        Profiles = profiles == null ? new List<ProfilesData>() : profiles.Profiles;

        SaveManagerEvents.current.AllProfilesLoaded(Profiles);
    }

    public static async Task LoadAllProfilesAsync(string path)
    {
        // This is the encrypted input from the file
        byte[] soupBackIn = await AsyncHelperExtensions.ReadBytesAsync(path);
        // Decrypting process
        Rijndael crypto = new Rijndael();
        string jsonFromFile = crypto.Decrypt(soupBackIn, JSON_ENCRYPTED_KEY_PROFILES);

        ProfilesListWrapper profiles = JsonUtility.FromJson<ProfilesListWrapper>(jsonFromFile);

        Profiles = profiles == null ? new List<ProfilesData>() : profiles.Profiles;

        SaveManagerEvents.current.AllProfilesLoaded(Profiles);
    }

    // Save all profiles to profiles.dat
    public static void SaveAllProfiles(string path)
    {
        ProfilesListWrapper profiles = new ProfilesListWrapper() { Profiles = Profiles };

        string json = JsonUtility.ToJson(profiles);

        // Encrypting process
        Rijndael crypto = new Rijndael();
        byte[] soup = crypto.Encrypt(json, JSON_ENCRYPTED_KEY_PROFILES);

        File.WriteAllBytes(path, soup);

        SaveManagerEvents.current.AllProfilesSaved(Profiles);
    }

    public static async Task SaveAllProfilesAsync(string path)
    {
        ProfilesListWrapper profiles = new ProfilesListWrapper() { Profiles = Profiles };

        string json = JsonUtility.ToJson(profiles);

        // Encrypting process
        Rijndael crypto = new Rijndael();
        byte[] soup = crypto.Encrypt(json, JSON_ENCRYPTED_KEY_PROFILES);

        await AsyncHelperExtensions.WriteBytesAsync(path, soup);

        SaveManagerEvents.current.AllProfilesSaved(Profiles);
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
}
