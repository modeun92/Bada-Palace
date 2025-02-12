using System;

namespace Assets.Scripts.Global
{
    public class GameConfigInterface
    {
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
                       (m_Setting.CurrentProgress.Level * m_Setting.ProfitScalePerLevel) +
                       (m_Setting.CurrentProgress.Stage / m_Setting.ProfitRatioPerStage),
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
        public GameProgress CurrentProgress //TO_FIX
        {
            get => m_Setting.CurrentProgress;
            set { lock (locker) { m_Setting.CurrentProgress = value; m_OnChanged(); } }
        }
        public GameProgress RecordProgress //TO_FIX
        {
            get => m_Setting.RecordProgress;
            set { lock (locker) { m_Setting.RecordProgress = value; m_OnChanged(); } }
        }
        public GameConfigInterface(GameConfig a_Setting, Action a_OnChanged)
        {
            m_Setting = a_Setting;
            m_OnChanged = a_OnChanged;
        }
        private Action m_OnChanged;
        private GameConfig m_Setting;
        private object locker = new object();
    }
    public enum ProfitReason { GameWon, Event, }
}
