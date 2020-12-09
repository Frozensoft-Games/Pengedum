using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Security.Permissions;

public class FileSystemEventsManager : MonoBehaviour
{
    private readonly static string MAIN_DIRECTORY_PATH = Application.persistentDataPath + "/Profiles/";

    public static void FileSystemEvents(string path)
    {
        // Create a new FileSystemWatcher and set its properties.
        FileSystemWatcher fileSystemWatcher = new FileSystemWatcher();
        fileSystemWatcher.Path = path;
        fileSystemWatcher.Created += FileSystemWatcher_Created;
        fileSystemWatcher.Renamed += FileSystemWatcher_Renamed;
        fileSystemWatcher.Deleted += FileSystemWatcher_Deleted;
        fileSystemWatcher.EnableRaisingEvents = true;
    }

    private static void FileSystemWatcher_Created(object sender, FileSystemEventArgs e)
    {
        FileInfo info = new FileInfo(e.FullPath);
        //Debug.LogFormat("File created: {0}", info.Name);
    }

    private static void FileSystemWatcher_Renamed(object sender, FileSystemEventArgs e)
    {
        FileInfo info = new FileInfo(e.FullPath);
        //Debug.LogFormat("File renamed: {0}", info.Name);
    }

    private static void FileSystemWatcher_Deleted(object sender, FileSystemEventArgs e)
    {
        FileInfo info = new FileInfo(e.FullPath);
        //Debug.LogFormat("File deleted: {0}", info.Name);
    }
}
