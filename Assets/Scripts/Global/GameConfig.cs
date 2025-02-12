using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
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

        public int ProfitScalePerLevel = 3;
        public int ProfitRatioPerStage = 7;
        public int ProfitPerEvent = 5;

        public Item[] Items = null;
        public GameProgress CurrentProgress = new GameProgress();
        public GameProgress RecordProgress = new GameProgress();
    }
}
