using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;
using System.Text;

public class DataAppLauncher : MonoBehaviour
{
    private static string shortcutPath;
    private static string spaceCheck;
    public static void LaunchApplication(string budgetFile)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(Environment.GetFolderPath(Environment.SpecialFolder.Programs));
        sb.Append("\\");
        sb.Append("Excel2Text");
        sb.Append("\\");
        sb.Append("Excel2Text.appref-ms ");
        shortcutPath = sb.ToString();
        ProcessStartInfo startInfo = new ProcessStartInfo();
        startInfo.CreateNoWindow = false;
        startInfo.UseShellExecute = true;
        startInfo.WindowStyle = ProcessWindowStyle.Minimized;
        spaceCheck = Application.streamingAssetsPath+","+budgetFile;
        spaceCheck = spaceCheck.Replace(" ", "<MakeASpace>");
        startInfo.Arguments = spaceCheck;
        startInfo.FileName = shortcutPath;
        Process.Start(startInfo);
    }
    //private void go()
    //{
    //    StartCoroutine(checkAppInstallation());
    //}
    //private static IEnumerator checkAppInstallation()
    //{
    //}
}
