using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class GameAdmin : MonoBehaviour
{
    private List<string> mDebugLines;
    private bool IsAdminModeOn = false;
    private static GameAdmin instance = null;
    void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    public static GameAdmin Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }
    public void ActivateAdminMode(bool aToggle)
    {
        IsAdminModeOn = aToggle;
        if (IsAdminModeOn)
        {
            mDebugLines = new List<string>();
        }
        else
        {
            mDebugLines.Clear();
            mDebugLines = null;
        }
    }
    public void Log(string aMessage)
    {
        if (IsAdminModeOn)
        {
            mDebugLines.Add(aMessage);
        }
    }
    public void ExecuteCommand(string aCommandLine)
    {
        if (IsAdminModeOn)
        {
            mDebugLines.Add(aCommandLine);
        }
    }
    private void ParseCommand(string aCommandLine)
    {
        if (aCommandLine.ElementAt(0) == '\\')
        {
            aCommandLine.Remove(0);
            var elements = aCommandLine.Split(' ');
            if (elements.Length >= 1)
            {
                var commandType = elements[0];
                if (commandType.Equals("exit"))
                {

                }
                else if (commandType.Equals("change"))
                {

                }
                else if (commandType.Equals("turn"))
                {

                }
            }
        }

    }
}
