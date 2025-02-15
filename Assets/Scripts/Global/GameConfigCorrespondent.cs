using Assets.Scripts.Maze;
using System;
using UnityEngine;

namespace Assets.Scripts.Global
{
    public class GameConfigCorrespondent
    {
        public GameConfigCorrespondent(ref GameConfig a_Setting, Action a_OnChanged)
        {
            m_Setting = a_Setting;
            m_OnChanged = a_OnChanged;
            //m_ProgressCorrespondent = new GameProgressCorrespondent(
            //    ref m_Setting.CurrentProgress, ref m_Setting.RecordProgress, ref m_Setting.MaximumProgress,
            //    () => { m_Setting.CurrentProgress = m_ProgressCorrespondent.CurrentProgress; m_OnChanged();
            //        Debug.Log("CONFIG"); });
        }
        public float Volume
        {
            get => m_Setting.Volume;
            set { lock (locker) { m_Setting.Volume = value; m_OnChanged(); } }
        }
        public float MusicVolume
        {
            get => m_Setting.MusicVolume;
            set { lock (locker) { m_Setting.MusicVolume = value; m_OnChanged(); } }
        }
        public bool IsRighthanded
        {
            get => m_Setting.IsRighthanded;
            set { lock (locker) { m_Setting.IsRighthanded = value; m_OnChanged(); } }
        }
        public int GoldAmount { get => m_Setting.GoldAmount; }
        public void AddGold(ProfitReason a_Reason)
        {
            lock (locker)
            {
                int l_Profit = a_Reason switch
                {
                    ProfitReason.GameWon =>
                       (m_Setting.CurrentProgress.Level * m_Setting.ProfitScaleProgress.Level) +
                       (m_Setting.CurrentProgress.Stage / m_Setting.ProfitScaleProgress.Stage),
                    ProfitReason.Event => m_Setting.ProfitPerEvent,
                    _ => 0
                };
                m_Setting.GoldAmount += l_Profit;
                m_OnChanged();
            }
        }
        public Item[] Items //TO_FIX
        {
            get => m_Setting.Items;
            set { lock (locker) { m_Setting.Items = value; m_OnChanged(); } }
        }
        //public GameProgressCorrespondent ProgressCorrespondent { get => m_ProgressCorrespondent; }

        //private GameProgressCorrespondent m_ProgressCorrespondent;
        private Action m_OnChanged;
        private GameConfig m_Setting;
        private object locker = new object();
    }
    public enum CompareType { Smaller = -1, Same = 0, Bigger = 1 }
    public enum ProfitReason { GameWon, Event, SELLING, BUYING }
}
