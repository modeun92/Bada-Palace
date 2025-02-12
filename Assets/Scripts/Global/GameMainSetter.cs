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
}

