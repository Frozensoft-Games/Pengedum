using System.Collections;
using System.Collections.Generic;
using System.IO;
using Assets.Code.Extensions;
using Assets.Code.Saving___Loading.Profile_System.SelectProfile;
using UnityEngine;

public class ScreenshotHandler : MonoBehaviour
{
    private static ScreenshotHandler instance;

    private Camera myCamera;
    private bool takeScreenshotOnNextFrame;

    private void Awake()
    {
        instance = this;
        myCamera = gameObject.GetComponent<Camera>();
    }

    private async void OnPostRender()
    {
        if (SelectedProfileManager.selectedProfile == null) return;

        if (!takeScreenshotOnNextFrame) return;
        takeScreenshotOnNextFrame = false;
        RenderTexture renderTexture = myCamera.targetTexture;

        Texture2D renderResult = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
        Rect rect = new Rect(0,0,renderTexture.width,renderTexture.height);
        renderResult.ReadPixels(rect,0,0);

        byte[] byteArray = renderResult.EncodeToPNG();
        string path = Path.Combine(Application.persistentDataPath, "Profiles", SelectedProfileManager.selectedProfile.fullProfileName, "ProfilePicture.png");
        await AsyncHelperExtensions.WriteBytesAsync(path, byteArray);
        Debug.Log("Saved Profile Picture.");

        RenderTexture.ReleaseTemporary(renderTexture);
        myCamera.targetTexture = null;
    }

    private void TakeScreenshot(int width, int height)
    {
        myCamera.targetTexture = RenderTexture.GetTemporary(width, height, 16);
        takeScreenshotOnNextFrame = true;
    }

    public static void TakeScreenshot_Static(int width, int height)
    {
        instance.TakeScreenshot(width, height);
    }
}
