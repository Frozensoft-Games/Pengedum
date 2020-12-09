using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class is used when saving one and one profile
[Serializable]
public class ProfileData
{
    // A unique GUID which is generated when profile is created
    public string profileId;
    // Used to set profile image value
    public string profileImage;
    // This value is used to store the Profile name
    public string profileName;
    // This value stores the full profile name including the profile ID 
    // For verification purposes only
    public string fullProfileName;
    // This value is used so the game knows if it should show the tutorial or not
    public bool isTutorialComplete;
}

// This is the class used when storing the profiles in profiles.dat
[Serializable]
public class ProfilesData
{
    // A unique GUID which is generated when profile is created
    public string profileId;
    // This value is used to store the Profile name
    public string profileName;
    // Full profile name including ID
    public string fullProfileName;
}
