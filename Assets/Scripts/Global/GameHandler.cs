using System;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Global
{
    public class GameHandler : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent OnWinning;
        [SerializeField]
        private UnityEvent OnPlaying;
        [SerializeField]
        private UnityEvent OnPausing;
        [SerializeField]
        private UnityEvent OnResuming;
        [SerializeField]
        private UnityEvent OnStopping;
        [SerializeField]
        private UnityEvent OnConfiguring;
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
        public void TellToStop()
        {
            OnStopping.Invoke();
        }
        public void TellToConfig()
        {
            OnConfiguring.Invoke();
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
}