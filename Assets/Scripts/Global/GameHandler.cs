using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameHandler : MonoBehaviour
{
    public OnWinning OnWinning;
    public OnPlaying OnPlaying;
    public OnPausing OnPausing;
    public OnResuming OnResuming;
    public Target target;
    // Start is called before the first frame update
    void Start()
    {
        OnPlaying.Invoke();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void TellToWin()
    {
        OnWinning.Invoke();
    }
    public void TellToPlay()
    {
        OnPlaying.Invoke();
    }
    public void TellToResume()
    {
        OnResuming.Invoke();
    }
    public void TellToPause()
    {
        OnPausing.Invoke();
    }
}
[Serializable]
public class OnWinning : UnityEvent { }
[Serializable]
public class OnPausing : UnityEvent { }
[Serializable]
public class OnPlaying : UnityEvent { }
[Serializable]
public class OnResuming : UnityEvent { }