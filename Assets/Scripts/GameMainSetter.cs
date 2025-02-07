using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using Assets.Scripts;
public delegate void OnSettingChanged();
public delegate object GetValue();
public delegate void SetValue(object value);
public enum ProgressCompareState
{
    LEVEL_ONE_STAGE_ONE = 00, LEVEL_ONE_STAGE_MIN, LEVEL_ONE_STAGE_MID, LEVEL_ONE_STAGE_MAX, //레벨은 모두 해제
    LEVEL_MIN_STAGE_ONE = 10, LEVEL_MIN_STAGE_MIN, LEVEL_MIN_STAGE_MID, LEVEL_MIN_STAGE_MAX, //레벨은 감소 해제 //stage 증가 해제 없음 (x 10)
    LEVEL_MID_STAGE_ONE = 20, LEVEL_MID_STAGE_MIN, LEVEL_MID_STAGE_MID, LEVEL_MID_STAGE_MAX, //레벨은 모두 설정 //stage 감소 증가 해제 없음 (x 21)
    LEVEL_MAX_STAGE_ONE = 30, LEVEL_MAX_STAGE_MIN, LEVEL_MAX_STAGE_MID, LEVEL_MAX_STAGE_MAX  //레벨은 증가 해제 //stage 감소 해제 없음 (x 31)
}
public class GameMainSettingManager
{
    private static string dir = Application.persistentDataPath + "/Setup";
    private static readonly string CONFIG = "config.badapalace";
    private static string path = dir + "/setting.json";
    private static GameConfig setting;
    private static OnSettingChanged settingChanged = () =>
    {
        Debug.Log("settingChanged called." + setting == null);
        var jsonString = JsonUtility.ToJson(setting);
        Debug.Log("json file: " + jsonString);
        //MakeDirs();
        //using (StreamWriter writer = new StreamWriter(File.Create(path)))
        //{
        //    writer.WriteLine(jsonString);
        //    writer.Flush();
        //}
        PlayerPrefs.SetString(CONFIG, jsonString);
        Debug.Log("writing done.");
    };
    public static void GetValue<T>(ConfigParameter aConfigParameter, out T aValue)
    {
        if (setting == null)
        {
            StartGameMainSetter();
        }
        aValue = setting.GetParameter<T>(aConfigParameter);
    }
    public static void SetValue<T>(ConfigParameter aConfigParameter, T aValue)
    {
        if (setting == null)
        {
            StartGameMainSetter();
        }
        setting.SetParameter<T>(aConfigParameter, aValue);
    }
    //public static void GetProgressStatic(out GameProgress current, out GameRecordProgress max)
    //{
    //    if (setting == null)
    //    {
    //        StartGameMainSetter();
    //    }
    //    current = setting.CurrentProgress;
    //    max = setting.RecordProgress;
    //}
    //public static void SetProgressStatic(int aLevel, int aStage)
    //{
    //    if (setting == null)
    //    {
    //        StartGameMainSetter();
    //    }
    //    SetProgress(aLevel, aStage);
    //    settingChanged();
    //}
    //public static void GetGoldAmountStatic(out int aGoldAmount)
    //{
    //    if (setting == null)
    //    {
    //        StartGameMainSetter();
    //    }
    //    aGoldAmount = setting.GoldAmount;
    //}
    //public static void SetGoldAmountStatic(int aGoldAmount)
    //{
    //    if (setting == null)
    //    {
    //        StartGameMainSetter();
    //    }
    //    setting.GoldAmount = aGoldAmount;
    //    settingChanged();
    //}
    //public static void GetItemsStatic(out Item[] aItems)
    //{
    //    if (setting == null)
    //    {
    //        StartGameMainSetter();
    //    }
    //    aItems = setting.Items;
    //}
    //public static void SetItemsStatic(Item[] aItems)
    //{
    //    if (setting == null)
    //    {
    //        StartGameMainSetter();
    //    }
    //    setting.Items = aItems;
    //    settingChanged();
    //}
    //public static void GetUserHandStatic(out bool aIsRighthanded)
    //{
    //    if (setting == null)
    //    {
    //        StartGameMainSetter();
    //    }
    //    aIsRighthanded = setting.IsRighthanded;
    //}
    //public static void SetUserHandStatic(bool aIsRighthanded)
    //{
    //    if (setting == null)
    //    {
    //        StartGameMainSetter();
    //    }
    //    setting.IsRighthanded = aIsRighthanded;
    //    settingChanged();
    //}
    //public static void GetVolumeStatic(out float aVolume)
    //{
    //    if (setting == null)
    //    {
    //        StartGameMainSetter();
    //    }
    //    aVolume = setting.Volume;
    //}
    //public static void SetVolumeStatic(float aVolume)
    //{
    //    if (setting == null)
    //    {
    //        StartGameMainSetter();
    //    }
    //    setting.Volume = aVolume;
    //    settingChanged();
    //}
    public static void StartGameMainSetter()
    {
        GetGameMainSetting();
    }
    private static void GetGameMainSetting()
    {
        var data = ReadData();
        try
        {
            Debug.Log("Deserializing.");
            setting = JsonUtility.FromJson<GameConfig>(data);
            Debug.Log("Deserialization succeeded." + data);
        }
        catch (Exception)
        { 
            Debug.Log("Deserialization failed.");
            setting = JsonUtility.FromJson<GameConfig>("{}");
        }
    }
    private static string ReadData()
    {
        string data = "{}";
        if (PlayerPrefs.HasKey(CONFIG))
        {
            data = PlayerPrefs.GetString(CONFIG, data);
        }
        //if (File.Exists(path)) //있으면 읽어오기
        //{
        //    Debug.Log("I found the setting file.");
        //    using (StreamReader reader = new StreamReader(path))
        //    {
        //        Debug.Log("I'm reading the setting file.");
        //        data = reader.ReadToEnd();
        //        Debug.Log("I've brought the setting information.");
        //    }
        //}
        //else //setting 클래스 만들고
        //{
        //    Debug.Log("there is no setting file.");
        //}
        return data;
    }
    private static void MakeDirs()
    {
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
    }
    //////////////////////////////////////////////////////////////////////
    //public static void SetProgress(int level, int stage)
    //{
    //    setting.CurrentProgress.SetProgress(level, stage);
    //    if (setting.RecordProgress.IsWithinMax(setting.CurrentProgress) == -1)
    //    {
    //        setting.RecordProgress.SetProgress(level, stage);
    //    }
    //    settingChanged();
    //}
    public static void Reset()
    {
        setting.CurrentProgress = new GameProgress();
        setting.RecordProgress = new GameRecordProgress();
        setting.Volume = 0.8f;
        settingChanged();
    }
}
[Serializable]
public class GameRecordProgress : GameProgress
{
    private int MaxAmount { get { return (Level * 10000) + Stage; } }
    public int IsWithinMax(GameProgress currentProgress)
    {
        return CompareTo(currentProgress.Level, currentProgress.Stage);
    }
    public int CompareTo(int aLevel, int aStage)
    {
        int currentProgressAmount = (aLevel * 10000) + aStage;
        return MaxAmount.CompareTo(currentProgressAmount);
    }
    public ProgressCompareState GetProgressState(int aLevel, int aStage)
    {
        if (aLevel > 1)
        {
            if (aLevel < Level)
            {
                return ProgressCompareState.LEVEL_MID_STAGE_MID;
            }
            else
            {
                if (aStage < Stage)
                {
                    return ProgressCompareState.LEVEL_MAX_STAGE_MID;
                }
                else
                {
                    return ProgressCompareState.LEVEL_MAX_STAGE_MAX;
                }
            }
        }
        else
        {
            if (Level > 1)
            {
                if (aStage > 1)
                {
                    return ProgressCompareState.LEVEL_MIN_STAGE_MID;
                }
                else
                {
                    return ProgressCompareState.LEVEL_MIN_STAGE_MIN;
                }
            }
            else
            {
                if (aStage > 1)
                {
                    if (aStage < Stage)
                    {
                        return ProgressCompareState.LEVEL_ONE_STAGE_MID;

                    }
                    else
                    {
                        return ProgressCompareState.LEVEL_ONE_STAGE_MAX;

                    }
                }
                else
                {
                    if (Stage > 1)
                    {
                        return ProgressCompareState.LEVEL_ONE_STAGE_MIN;
                    }
                    else
                    {
                        return ProgressCompareState.LEVEL_ONE_STAGE_ONE;
                    }
                }
            }
        }
    }
}
[Serializable]
public class GameProgress
{
    public int Level = 1;
    public int Stage = 1;
    public void SetProgress(int aLevel, int aStage)
    {
        Level = aLevel;
        Stage = aStage;
    }
    public GameProgress()
    {

    }
    public GameProgress(int aLevel, int aStage)
    {
        SetProgress(aLevel, aStage);
    }
}

