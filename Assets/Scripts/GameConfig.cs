using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public enum ConfigParameter { VERSION, VOLUME, MUSIC_VOLUME, IS_RIGHTHANDED, GOLD_AMOUNT, ITEMS, CURRENT_PROGRESS, RECORD_PROGRESS }
    [Serializable]
    class GameConfig
    {
        public string Version = "3.0";
        public float Volume = -5f;
        public float MusicVolume = -5f;
        public bool IsRighthanded = true;
        public int GoldAmount = 0;
        public Item[] Items = null;
        public GameProgress CurrentProgress = new GameProgress();
        public GameRecordProgress RecordProgress = new GameRecordProgress();
        private Dictionary<ConfigParameter, GetValue> mParametersGetters = null;
        private Dictionary<ConfigParameter, SetValue> mParametersSetters = null;
        public GameConfig()
        {
            mParametersGetters = new Dictionary<ConfigParameter, GetValue>();
            mParametersGetters.Add(ConfigParameter.VERSION, () => { return Version; });
            mParametersGetters.Add(ConfigParameter.VOLUME, () => { return Volume; });
            mParametersGetters.Add(ConfigParameter.MUSIC_VOLUME, () => { return MusicVolume; });
            mParametersGetters.Add(ConfigParameter.IS_RIGHTHANDED, () => { return IsRighthanded; });
            mParametersGetters.Add(ConfigParameter.GOLD_AMOUNT, () => { return GoldAmount; });
            mParametersGetters.Add(ConfigParameter.ITEMS, () => { return Items; });
            mParametersGetters.Add(ConfigParameter.CURRENT_PROGRESS, () => { return CurrentProgress; });
            mParametersGetters.Add(ConfigParameter.RECORD_PROGRESS, () => { return RecordProgress; });

            mParametersSetters = new Dictionary<ConfigParameter, SetValue>();
            mParametersSetters.Add(ConfigParameter.VERSION, (value) => { Version = (string)value; });
            mParametersSetters.Add(ConfigParameter.VOLUME, (value) => { Volume = (float)value; });
            mParametersSetters.Add(ConfigParameter.MUSIC_VOLUME, (value) => { MusicVolume = (float)value; });
            mParametersSetters.Add(ConfigParameter.IS_RIGHTHANDED, (value) => { IsRighthanded = (bool)value; });
            mParametersSetters.Add(ConfigParameter.GOLD_AMOUNT, (value) => { GoldAmount = (int)value; });
            mParametersSetters.Add(ConfigParameter.ITEMS, (value) => { Items = (Item[])value; });
            mParametersSetters.Add(ConfigParameter.CURRENT_PROGRESS, (value) => { CurrentProgress = (GameProgress)value; });
            mParametersSetters.Add(ConfigParameter.RECORD_PROGRESS, (value) => { RecordProgress = (GameRecordProgress)value; });
        }
        public T GetParameter<T>(ConfigParameter parameter)
        {
            GetValue valueGetter = null;
            mParametersGetters.TryGetValue(parameter, out valueGetter);
            return (T)valueGetter();
        }
        public void SetParameter<T>(ConfigParameter parameter, T value)
        {
            SetValue valueSetter = null;
            mParametersSetters.TryGetValue(parameter, out valueSetter);
            valueSetter(value);
        }
    }
}
