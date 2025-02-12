using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameTotalEventManager : MonoBehaviour
{
    public GameState GameState = GameState.PAUSED;

    public OnGameStarted OnGameStarted;
    public OnGamePaused OnGamePaused;
    public OnGameResumed OnGameResumed;
    public OnGameWinning OnGameWinning;
    public OnGameLosing OnGameLosing;
    public OnGameEnded OnGameEnded;

    private static GameTotalEventManager instance = null;
    void Awake()
    {
        OnGameStarted = () => { GameState = GameState.PLAYING; };
        OnGamePaused = () => { GameState = GameState.PAUSED; };
        OnGameResumed = () => { GameState = GameState.PLAYING; };
        OnGameWinning = () => { SetPause(); Debug.Log("WON so PAUSED"); };
        OnGameLosing = () => { GameState = GameState.PAUSED; };
        OnGameEnded = () => { GameState = GameState.PAUSED; };

        instance = this;
        DontDestroyOnLoad(this.gameObject);
        StartGame();
    }
    public static GameTotalEventManager Instance
    {
        get
        {
            return instance;
        }
    }
    private void SetPause()
    {
        GameState = GameState.PAUSED;
    }
    public void StartGame()
    {
        OnGameStarted();
        Time.timeScale = 1;
    }
    public void PauseGame()
    {
        OnGamePaused();
        Time.timeScale = 0;
    }
    public void ResumeGame()
    {
        OnGameResumed();
        Time.timeScale = 1;
    }
    public void WinGame()
    {
        Debug.Log("Game WON!");
        OnGameWinning();
    }
    public void LoseGame()
    {
        OnGameLosing();
    }
    public void EndGame()
    {
        OnGameEnded();
    }
    
}
public enum GameState { PLAYING, PAUSED };
public delegate void OnGameStarted();
public delegate void OnGamePaused();
public delegate void OnGameResumed();
public delegate void OnGameWinning();
public delegate void OnGameLosing();
public delegate void OnGameEnded();