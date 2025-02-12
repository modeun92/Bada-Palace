using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public Image PlayingPanel;
    public Image PausePanel;
    public Image LosingPanel;
    public Image WinningPanel;
    public Image ConfigPanel;
    public Image AdminPanel;
    
    private Image[] mImages;
    private PanelType mPanels;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("UiManager initing...");
        mImages = new Image[]{ PlayingPanel, PausePanel, LosingPanel, WinningPanel, ConfigPanel, AdminPanel };
        GameTotalEventManager.Instance.OnGameStarted += () => { ActivateOnlyThisPanel(PanelType.PLAYING_PANEL); };
        GameTotalEventManager.Instance.OnGamePaused +=  () => { ActivateOnlyThisPanel(PanelType.PAUSE_PANEL); };
        GameTotalEventManager.Instance.OnGameResumed += () => { ActivateOnlyThisPanel(PanelType.PLAYING_PANEL); };
        GameTotalEventManager.Instance.OnGameWinning += () => { Debug.Log("WINNING SCREEN");  ActivateOnlyThisPanel(PanelType.WINNING_PANEL); };
        GameTotalEventManager.Instance.OnGameLosing +=  () => { ActivateOnlyThisPanel(PanelType.LOSING_PANEL); };
        GameTotalEventManager.Instance.OnGameEnded +=   () => { ActivateOnlyThisPanel(PanelType.NONE); };
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void ActivateConfig(bool aToggle)
    {
        float scalex = 0f;
        if (aToggle)
        {
            scalex = 1f;
        }
        ConfigPanel.GetComponent<Transform>().localScale = new Vector3(scalex, 1f, 1f);
    }
    private void ActivateOnlyThisPanel(PanelType aPanelType)
    {
        Debug.Log("SCREEN doing");
        int index = (int)aPanelType;
        for (int i = 0; i < mImages.Length; i++)
        {
            float scalex = index == i ? 1f : 0f;
            mImages[i].GetComponent<Transform>().localScale = new Vector3(scalex, 1f, 1f);
        }
        Debug.Log("SCREEN done");
    }
}
public enum PanelType { PLAYING_PANEL, PAUSE_PANEL, LOSING_PANEL, WINNING_PANEL, CONFIG_PANEL, ADMIN_PANEL, NONE }