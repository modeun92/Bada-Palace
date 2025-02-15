using System;

namespace Assets.Scripts.Global
{
    [Serializable]
    public class GameConfig
    {
        public string Version = "3.1";
        public float Volume = -5f;
        public float MusicVolume = -5f;
        public bool IsRighthanded = true;
        public int GoldAmount = 0;

        public int MaxStagePerLevel = 20;

        public int ProfitPerEvent = 5;

        public Item[] Items = null;
        public GameProgress CurrentProgress = new GameProgress();
        public GameProgress RecordProgress = new GameProgress();
        public GameProgress ProfitScaleProgress = new GameProgress() { Level = 3, Stage = 7 };
        public GameProgress MaximumProgress = new GameProgress() { Level = 31, Stage = 20 };
    }
}
