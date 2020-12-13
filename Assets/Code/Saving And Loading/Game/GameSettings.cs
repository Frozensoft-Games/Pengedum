using System;
using UnityEngine;

// This class is used when saving one and one profile
[Serializable]
public class GameSettings
{
    // Bool for fullscreen
    public bool fullscreen;

    // Index for Quality/Texture  Quality
    public int textureQuality;

    // Index for Anti aliasing
    public int antialiasing;
    
    // Index for vSync
    public int vSync;

    // Index for Resolution
    public int resolutionIndex;

    // Volume
    public float volume;
}