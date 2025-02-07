using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class DestinationManager : MonoBehaviour
{
    public OnDestinated OnDestinated { get { return mOnDestinated; } set { mOnDestinated = value; } }
    [SerializeField]
    private OnDestinated mOnDestinated = new OnDestinated();
    private AudioSource mAudioSource;
    // Start is called before the first frame update
    void Start()
    {
        mAudioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameTotalEventManager.Instance.GameState == GameState.PLAYING && collision.gameObject.tag == "Player")
        {
            mAudioSource.Play();
            mOnDestinated.Invoke();
        }
        
    }
}
[Serializable]
public class OnDestinated : UnityEvent { }
