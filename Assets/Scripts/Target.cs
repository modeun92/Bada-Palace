using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Target : MonoBehaviour
{
    public OnArrived OnArrived { get { return m_OnArrived; } set { m_OnArrived = value; } }
    [SerializeField]
    private OnArrived m_OnArrived = new OnArrived();
    private AudioSource mAudioSource;
    private bool m_IsArrivaable;
    // Start is called before the first frame update
    void Start()
    {
        mAudioSource = GetComponent<AudioSource>();
    }
    public void SetArrivable(bool a_IsArrivable)
    {
        m_IsArrivaable = a_IsArrivable;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            mAudioSource.Play();
            m_OnArrived.Invoke();
            Debug.Log("TOUCHED");
        }
    }
}
[Serializable]
public class OnArrived : UnityEvent { }
