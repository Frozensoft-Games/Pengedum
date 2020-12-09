using Assets.Code.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileImagesListControl : MonoBehaviour
{
    [SerializeField]
    private GameObject optionTemplate;

    private List<ProfileImagesListOption> profileImages = new List<ProfileImagesListOption>();

    public List<GameObject> options = new List<GameObject>();

    bool isOpen = false;

    public ProfileImageListSelectedOption selectedOption;

    public async void GenerateList()
    {
        if (isOpen)
        {
            if (options.Count > 0)
            {
                options.ForEach(option => Destroy(option.gameObject));
                options.Clear();
            }
            isOpen = false;
            return;
        }

        foreach (ProfileImagesListOption profileImage in profileImages)
        {
            if(profileImage.optionName != null && profileImage.optionSprite != null) AddOption(profileImage.optionName, profileImage.optionSprite);
        }

        //AsyncHelperExtensions.LoadResourcesProfileImagesScrollBar(this.GetComponent<ProfileImagesListControl>());
        //await AsyncHelperExtensions.LoadProfileImagesScrollBar(this.GetComponent<ProfileImagesListControl>());
        isOpen = true;
    }

    public void AddOption(string optionName, Sprite optionSprite)
    {
        GameObject option = Instantiate(optionTemplate) as GameObject;
        option.name = optionName;
        option.GetComponent<ProfileImagesListOptionButton>().SetText(optionName);
        option.GetComponent<ProfileImagesListOptionButton>().SetImage(optionSprite);
        option.transform.SetParent(optionTemplate.transform.parent, false);
        option.SetActive(true);
        options.Add(option);
    }

    public async void SelectOption(ProfileImageListSelectedOption selectedOption)
    {
        //await AsyncHelperExtensions.LoadProfileImagesScrollBarSelected(this.GetComponent<ProfileImagesListControl>(), selectedOption);
        this.selectedOption = selectedOption;
        GenerateList();
    }
}
