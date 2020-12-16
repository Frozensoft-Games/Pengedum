using System;
using System.Collections.Generic;
using UnityEngine;

// This class is used when saving one and one profile
[Serializable]
public class GameData
{
    // Value to see if you have played before or not.
    public bool hasPlayed;
    // Balance
    public int balance;
    // Player Position
    public Vector3 playerPosition;
    // Player Rotation
    public Quaternion playerRotation;
    // Camera Position
    public Vector3 cameraPosition;
    // Camera Rotation
    public Quaternion cameraRotation;

    // In game
    public List<InGameData> questions;
}