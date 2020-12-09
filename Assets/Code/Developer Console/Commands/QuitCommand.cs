using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quit Command", menuName = "Utilities/DeveloperConsole/Commands/Quit Command")]
public class QuitCommand : ConsoleCommand
{
    public override bool Process(string[] args)
    {
        Debug.Log("Qutting: All unsaved progress will be lost.");

        Application.Quit();

        return true;
    }
}