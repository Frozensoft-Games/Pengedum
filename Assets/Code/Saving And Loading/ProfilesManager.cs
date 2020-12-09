using System;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Collections;
using Assets.Code.Saving___Loading.Profile_System.SelectProfile;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using Assets.Code.Extensions;
using UnityEditor;
using UnityEngine.Serialization;

namespace Assets.Code.Profile_System
{
    public class ProfilesManager : MonoBehaviour
    {
        // This is the value used to store the Main directory path
        private string mainDirectoryPath = "";

        private string profilesImages = "";

        // This is the value used to store the profiles.dat location
        private string profilesPath = "";

        // Input fields used to create the profile
        InputField profileName;

        // Showing all profiles UI in the profiles scroll view
        public GameObject profilesUi;

        // UI needed for Create Profile
        public GameObject createProfileCanvas;

        // UI needed for Edit Profile
        public GameObject editProfileCanvas;

        // When the Profiles Manager loads it sets it's values and it loads the profiles
        public async void Start()
        {
            SaveManagerEvents.current.OnProfileSaved += OnProfileSaved;
            SaveManagerEvents.current.OnProfileDeleted += OnProfileDeleted;
            SaveManagerEvents.current.OnProfileEdited += OnProfileEdited;
            SaveManagerEvents.current.OnCreateProfile += OnCreateProfile;
            SaveManagerEvents.current.OnAllProfilesSaved += OnAllProfilesSaved;
            SaveManagerEvents.current.OnAllProfilesLoaded += OnAllProfilesLoaded;
            SaveManagerEvents.current.OnProfileLoaded += OnProfileLoaded;

            //Sets the main path value for Profiles
            mainDirectoryPath = Application.dataPath + "Resources/Profiles/";

            //Sets the profiles.dat path value
            profilesPath = mainDirectoryPath + "/profiles.dat";

            // Sets the path for ProfileImages Folder
            profilesImages = Application.dataPath + "Resources/ProfileImages/";

            // Checks if the Profiles Folder Exists or not. If it doesn't exist it creates it.
            if (!Directory.Exists(mainDirectoryPath)) FileManagerExtension.CreateDirectory(mainDirectoryPath);

            if (!Directory.Exists(profilesImages)) FileManagerExtension.CreateDirectory(profilesImages);

            // Loads the event
            FileSystemEventsManager.FileSystemEvents(mainDirectoryPath);

            //Gets all Directories in the Profiles folder
            string[] profiles = FileManagerExtension.GetDirectories(mainDirectoryPath);

            // Shows the Create profile UI if player doesn't have any profiles
            if (!profiles.Any())
            {
                FirstTimePlaying();
                await SaveManager.SaveAllProfilesAsync(profilesPath);
                return;
            }

            await SaveManager.LoadAllProfilesAsync(profilesPath);

            for (int i = 0; i < profiles.Length; i++)
            {
                await LoadProfile(profiles.GetValue(i).ToString());
            }
        }

        private async Task LoadProfile(string profilePath)
        {
            string profileConfigPath = profilePath + "/config.dat";
            string profileName = profilePath.Split('/').Last();

            // Deletes Directory if config file doesn't exist
            if (!Directory.Exists(profilePath) || !File.Exists(profileConfigPath))
            {
                Debug.Log("Failed to load " + profileName + " could not locate needed information");
            }
            else if (!SaveManager.Profiles.Exists(p => p.profileId.ToString().Equals(profileName.Split(' ').Last())))
            {
                Debug.Log(profileName + " not found in profiles.dat file profile will not be loaded");
            }
            else
            {
                // Loads the profile and it's content
                ProfileData profile = await SaveManager.LoadProfileAsync(profileConfigPath, profileName, profileName.Split(' ').Last());

                if (profile != null)
                {
                    // Creates the stuff needed for the Profiles UI
                    GameObject ProfilesUIClone = Instantiate(profilesUi) as GameObject;
                    ProfilesUIClone.transform.SetParent(GameObject.Find("Profiles").transform.Find("Viewport").Find("Content").transform, false);
                    ProfilesUIClone.AddComponent<SelectProfileData>();
                    ProfilesUIClone.GetComponent<SelectProfileData>().profileName = profile.profileName;
                    ProfilesUIClone.GetComponent<SelectProfileData>().profileId = profile.profileId;
                    ProfilesUIClone.GetComponent<SelectProfileData>().fullProfileName = profile.fullProfileName;
                    ProfilesUIClone.transform.Find("ProfileInformation").transform.Find("ProfileName").GetComponentInChildren<Text>().text = profile.profileName;

                    if(ProfilesUIClone.transform.Find("ProfileImages").transform.Find(profile.profileImage) != null)
                    {
                        ProfilesUIClone.transform.Find("ProfileImages").transform.Find(profile.profileImage).gameObject.SetActive(true);
                    }

                    ProfilesUIClone.name = profile.fullProfileName;
                    ProfilesUIClone.tag = "ProfileUI(Clone)";
                    if (SaveManager.Profiles.Where(p => p.profileName.Equals(profile.profileName)).ToList().Count > 1)
                    {
                        ProfilesUIClone.transform.Find("ProfileInformation").gameObject.transform.Find("ProfileName").gameObject.SetActive(false);
                        ProfilesUIClone.transform.Find("ProfileInformation").transform.Find("FullProfileName").gameObject.SetActive(true);
                    }
                    if (profile.profileName.Equals("eiromplays", StringComparison.OrdinalIgnoreCase))
                    {
                        if(ColorUtility.TryParseHtmlString("#bf9b30", out Color color))
                        {
                            ProfilesUIClone.transform.Find("ProfileInformation").GetComponent<Image>().color = color;
                            ProfilesUIClone.transform.Find("ProfileImages").GetComponent<Image>().color = color;
                        }
                    }
                    ProfilesUIClone.SetActive(true);
                }
            }
        }

        public void OnProfileSaved(ProfileData profile)
        {
            Debug.Log("Profile was saved: " + profile.fullProfileName);

            // Clears the old UI Elements and loads the new ones
            ClearProfilesUIScrollView();
            CancelEditProfile();
            CancelCreateProfile();
            Start();
        }

        public void OnProfileEdited(ProfileData editedProfile)
        {
            Debug.Log("Profile was edited: " + editedProfile.fullProfileName);
        }

        public void OnCreateProfile(string profileName, string fullProfileName, string profileId)
        {
            Debug.Log("Creating profile: " + fullProfileName);
        }

        public void OnProfileDeleted(SelectedProfile selectedProfile)
        {
            Debug.Log($"Profile {selectedProfile.fullProfileName} was deleted!");
            // Clears the old UI Elements and loads the new ones
            ClearProfilesUIScrollView();
            Start();
        }

        public void OnProfileLoaded(ProfileData loadedProfile)
        {
            Debug.Log("Profile was loaded: " + loadedProfile.fullProfileName);
        }

        public void OnAllProfilesSaved(List<ProfilesData> profiles)
        {
            foreach(ProfilesData profile in profiles)
            {
                Debug.Log("Profile: " + profile.fullProfileName + " was saved in profiles.dat");
            }
        }

        public void OnAllProfilesLoaded(List<ProfilesData> profiles)
        {
            foreach (ProfilesData profile in profiles)
            {
                Debug.Log("Profile: " + profile.fullProfileName + " was loaded from profiles.dat");
            }
        }

        // Clear the Profiles UI Scroll View
        public void ClearProfilesUIScrollView()
        {
            GameObject[] profilesToDelete = GameObject.FindGameObjectsWithTag("ProfileUI(Clone)");
            foreach (GameObject profile in profilesToDelete)
                GameObject.Destroy(profile);
        }

        // Clears the old UI Elements and loads the new ones
        public void ClearProfilesUI()
        {
            ClearProfilesUIScrollView();
            CancelEditProfile();
            Start();
        }

        // Hides the Create Profile Canvas
        public void CancelCreateProfile()
        {
            createProfileCanvas.SetActive(false);
            ClearInputFields();
        }

        // Hides the Edit Profile Canvas
        public void CancelEditProfile()
        {
            ClearInputFields();
            editProfileCanvas.SetActive(false);
            SelectedProfileManager.selectedProfile = null;
        }

        // Clears the Input fields
        public void ClearInputFields()
        {
            if(profileName != null) profileName.text = "";
        }

        // Shows the Create Profile Canvas
        public void NewProfile()
        {
            createProfileCanvas.SetActive(true);
            profileName = GameObject.Find("ProfileNameInput").GetComponent<InputField>();
        }

        // Shows the Create Profile Canvas
        public void FirstTimePlaying()
        {
            NewProfile();
            createProfileCanvas.transform.Find("CancelButton").gameObject.SetActive(false);
        }

        // Shows the Edit Profile Canvas
        public void EditProfileButton()
        {
            if (SelectedProfileManager.selectedProfile == null)
            {
                Debug.Log("You don't have any profile selected");
                return;
            }
            editProfileCanvas.SetActive(true);
            profileName = GameObject.Find("ProfileNameInput").GetComponent<InputField>();
            profileName.text = SelectedProfileManager.selectedProfile.profileName;
        }

        // This function is used to create a new profile
        public async void CreateProfile()
        {
            // Creates the unique profile ID
            string profileId = $"{GUID.Generate()}{GUID.Generate()}{GUID.Generate()}";

            if (string.IsNullOrEmpty(profileName.text))
            {
                Debug.Log("Profile name can't be empty or null");
                return;
            }

            // Checks if the Profile already exists or not.
            while (Directory.Exists(mainDirectoryPath + profileName.text + " " + profileId) || Directory.Exists($"{mainDirectoryPath}{profileName.text}"))
            {
                profileId = $"{profileId}{GUID.Generate()}";
            }

            string fullProfileName = $"{profileName.text} {profileId}";

            // Sets the paths needed for the profile
            string profileSavePath = mainDirectoryPath + fullProfileName + "/Save/";
            string profileSaveFilePath = Path.Combine(profileSavePath + "data.dat");
            string profileConfigPath = mainDirectoryPath + fullProfileName + "/config.dat";
            string profileSettingsPath = mainDirectoryPath + fullProfileName + "/Settings.dat";


            if (!Directory.Exists(mainDirectoryPath)) FileManagerExtension.CreateDirectory(mainDirectoryPath);

            FileManagerExtension.CreateDirectory(profileSavePath);

            FileManagerExtension.CreateFile(profileSaveFilePath);

            FileManagerExtension.CreateFile(profileSettingsPath);

            SaveManagerEvents.current.CreateProfile(profileName.text, fullProfileName, profileId);

            if (SelectedProfileManager.selectedProfile == null)
            {
                Debug.Log("Failed to create profile reason unknown");
                return;
            }

            // Creates and saves the profile.
            await SaveManager.SaveProfileAsync(profileConfigPath, SelectedProfileManager.selectedProfile.profileImage, profileName.text, profileId, fullProfileName);
        }

        // This function is used to Edit an already existing profile
        public async void EditProfile()
        {
            if (SelectedProfileManager.selectedProfile == null)
            {
                Debug.Log("You don't have any profile selected");
                return;
            }

            if (string.IsNullOrEmpty(this.profileName.text))
            {
                Debug.Log("Profile name can't be empty or null");
                return;
            }

            string profileId = SelectedProfileManager.selectedProfile.profileId;
            string profileName = this.profileName.text;
            string fullProfileName = $"{this.profileName.text} {profileId}";

            // Checks if the Profile Name is the same
            if (fullProfileName.Equals(SelectedProfileManager.selectedProfile.fullProfileName))
            {
                Debug.Log("You didn't change the profile name");
                profileName = SelectedProfileManager.selectedProfile.profileName;
            }
            else
            {
                FileManagerExtension.MoveDirectory(mainDirectoryPath + SelectedProfileManager.selectedProfile.fullProfileName, mainDirectoryPath + fullProfileName);
            }

            // Sets the paths needed for the profile
            string profileSavePath = mainDirectoryPath + fullProfileName + "/Save/";
            string profileSaveFilePath = Path.Combine(profileSavePath + "data.dat");
            string profileConfigPath = mainDirectoryPath + fullProfileName + "/config.dat";
            string profileSettingsPath = mainDirectoryPath + fullProfileName + "/Settings.dat";


            if (!Directory.Exists(mainDirectoryPath)) FileManagerExtension.CreateDirectory(mainDirectoryPath);

            if (!Directory.Exists(profileSavePath)) FileManagerExtension.CreateDirectory(profileSavePath);

            if (!File.Exists(profileSaveFilePath)) FileManagerExtension.CreateFile(profileSaveFilePath);

            if (!File.Exists(profileSettingsPath)) FileManagerExtension.CreateFile(profileSettingsPath);


            // Deletes the old Profiles value from profiles.dat
            SaveManager.Profiles.Remove(SaveManager.Profiles.FirstOrDefault(p => p.fullProfileName.Equals(SelectedProfileManager.selectedProfile.fullProfileName)));
            await SaveManager.SaveAllProfilesAsync(profilesPath);

            if (SelectedProfileManager.selectedProfile == null)
            {
                Debug.Log("Failed to create profile reason unknown");
                return;
            }

            string profileImage = SelectedProfileManager.selectedProfile.profileImage;

            // Creates and saves the profile.
            await SaveManager.SaveProfileAsync(profileConfigPath, profileImage, profileName, profileId, fullProfileName);

            SaveManagerEvents.current.ProfileEdited(SaveManager.CreateProfile(profileId, profileImage, profileName, fullProfileName, false));

            SelectedProfileManager.selectedProfile = null;
        }

        // Shows the Delete Profile Canvas
        public async void DeleteProfile()
        {
            if (SelectedProfileManager.selectedProfile == null)
            {
                Debug.Log("You don't have any profile selected");
                return;
            }

            FileManagerExtension.DeleteDirectory(mainDirectoryPath + SelectedProfileManager.selectedProfile.fullProfileName);
            SaveManager.Profiles.Remove(SaveManager.Profiles.FirstOrDefault(p => p.fullProfileName.Equals(SelectedProfileManager.selectedProfile.fullProfileName)));
            await SaveManager.SaveAllProfilesAsync(profilesPath);

            SaveManagerEvents.current.ProfileDeleted(SelectedProfileManager.selectedProfile);
        }
    }
}
