using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using Assets.Scripts;
using Assets.Scripts.Global;
using UnityEditor.SceneManagement;

public delegate void OnSettingChanged();
public delegate object GetValue();
public delegate void SetValue(object value);
internal enum ProgressControllerState
{
    None, Upable, Downable, Bidir,

    LevelNone_StageNone = 0,    LevelNone_StageUpable,      LevelNone_StageDownable,        LevelNone_StageBidir,
    LevelUpable_StageNone,      LevelUpable_StageUpable,    LevelUpable_StageDownable,      LevelUpable_StageBidir,
    LevelDownable_StageNone,    LevelDownable_StageUpable,  LevelDownable_StageDownable,    LevelDownable_StageBidir,
    LevelBidir_StageNone,       LevelBidir_StageUpable,     LevelBidir_StageDownable,       LevelBidir_StageBidir
}

