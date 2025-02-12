using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts;
using Assets.Scripts.Maze;
public class GameProgressUpdateInterface : MonoBehaviour
{
    public Button lvlIncr;
    public Button lvlDecr;
    public Button stgIncr;
    public Button stgDecr;

    public Text lvlText;
    public Text stgText;

    private int level;
    private int stage;

    private GameProgress current;
    private GameRecordProgress record;
    // Start is called before the first frame update
    void Start()
    {
        //GameMainSettingManager.GetProgressStatic(out current, out max);
        ConfigManager.GetValue(ConfigParameter.CURRENT_PROGRESS, out current);
        ConfigManager.GetValue(ConfigParameter.RECORD_PROGRESS, out record);
        level = current.Level;
        stage = current.Stage;
        Debug.Log("level:" + level + ",stage:"+stage);
        CheckGameMatchRange();
        UiUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Apply()
    {
        ConfigManager.SetValue(ConfigParameter.CURRENT_PROGRESS, new GameProgress(level, stage));
    }
    public void SetProgress(string message)
    {
        string[] messageTiles = message.Split(':');
        string type = messageTiles[0];
        string direction = messageTiles[1];
        if (type == "level")
        {
            if (direction == "up")
            {
                level++;
            }
            else if (direction == "down")
            {
                level--;
            }
        }
        else if (type == "stage")
        {
            if (direction == "up")
            {
                stage++;
            }
            else if (direction == "down")
            {
                stage--;
            }
        }

        CheckGameMatchRange();

        UiUpdate();
    }
    private void UiUpdate()
    {
        var state = record.GetProgressState(level, stage);
        string[] stateMessages = state.ToString().Split('_');
        string levelState = stateMessages[1];
        if (levelState == "ONE")
        {
            lvlDecr.interactable = false;
            lvlIncr.interactable = false;
        }
        else if (levelState == "MIN")
        {
            lvlDecr.interactable = false;
            lvlIncr.interactable = true;
        }
        else if (levelState == "MID")
        {
            lvlDecr.interactable = true;
            lvlIncr.interactable = true;
        }
        else if (levelState == "MAX")
        {
            lvlDecr.interactable = true;
            lvlIncr.interactable = false;
        }

        string stageState = stateMessages[3];
        if (stageState == "ONE")
        {
            stgDecr.interactable = false;
            stgIncr.interactable = false;
        }
        else if (stageState == "MIN")
        {
            stgDecr.interactable = false;
            stgIncr.interactable = true;
        }
        else if (stageState == "MID")
        {
            stgDecr.interactable = true;
            stgIncr.interactable = true;
        }
        else if (stageState == "MAX")
        {
            stgDecr.interactable = true;
            stgIncr.interactable = false;
        }

        lvlText.text = "Level " + level;
        stgText.text = "Stage " + stage;
    }
    private void CheckGameMatchRange()
    {
        if (stage < 1)
        {
            level--;
            if (level < 1)
            {
                level = 1;
                stage = 1;
            }
            else
            {
                stage = MazeConstructor.MAXIMUM_STAGE;
            }
        }
        else if (level < MazeConstructor.MAXIMUM_LEVEL && stage > MazeConstructor.MAXIMUM_STAGE)
        {
            stage = 1;
            level++;
        }

        if (level < 1)
        {
            level = 1;
        }
        else if (level > MazeConstructor.MAXIMUM_LEVEL)
        {
            level = MazeConstructor.MAXIMUM_LEVEL;
        }

        if (record.CompareTo(level, stage) == -1)
        {
            level = record.Level;
            stage = record.Stage;
        }
    }
}
