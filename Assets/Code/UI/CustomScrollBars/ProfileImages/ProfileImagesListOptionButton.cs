using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileImagesListOptionButton : MonoBehaviour
{
    [SerializeField]
    private Text optionName;

    [SerializeField]
    private RawImage optionRawImage;

    public ProfileImagesListControl profileImagesControl;

    public void SetText(string optionName)
    {
        if (optionName != null) this.optionName.text = optionName;
    }

    public void SetImage(Sprite optionSprite)
    {
        if(optionSprite != null) this.optionRawImage.texture = optionSprite.texture;
    }

    public void OptionSelected()
    {
        profileImagesControl.SelectOption(CreateSelectedProfileImageOption(optionName.text, optionRawImage));
    }

    public ProfileImageListSelectedOption CreateSelectedProfileImageOption(string optionName, RawImage optionRawImage)
    {
        ProfileImageListSelectedOption profileImagesListOption = new ProfileImageListSelectedOption()
        {
            selectedOptionName = optionName,
            selectedOptionRawImage = optionRawImage
        };
        return profileImagesListOption;
    }
}
