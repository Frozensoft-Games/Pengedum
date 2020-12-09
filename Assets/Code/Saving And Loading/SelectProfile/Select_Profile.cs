using Assets.Code.Saving___Loading.Profile_System.SelectProfile;
using UnityEngine;
using UnityEngine.UI;

public class Select_Profile : MonoBehaviour
{
    public void SelectProfile()
    {
        string selectedProfileID = this.GetComponent<SelectProfileData>().profileId;
        string selectedProfileName = this.GetComponent<SelectProfileData>().profileName;
        string selectedCompanyName = this.GetComponent<SelectProfileData>().companyName;
        string selectedFullProfileName = this.GetComponent<SelectProfileData>().fullProfileName;
        string selectedProfileImage = this.GetComponent<SelectProfileData>().profileImage;

        SelectedProfileManager.selectedProfile = CreateSelectedProfileObject(selectedProfileID, selectedProfileName, selectedFullProfileName, selectedCompanyName, selectedProfileImage);
    }

    // Used to easily create an SelectedProfile object
    public SelectedProfile CreateSelectedProfileObject(string selectedProfileId, string selectedProfile, string selectedFullProfileName, string selectedCompany, string selectedProfileImage)
    {
        SelectedProfile SelectedProfile = new SelectedProfile
        {
            profileId = selectedProfileId,
            profileName = selectedProfile,
            fullProfileName = selectedFullProfileName,
            companyName = selectedCompany,
            profileImage = selectedProfileImage
        };

        return SelectedProfile;
    }
}
