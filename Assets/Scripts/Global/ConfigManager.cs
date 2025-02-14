using UnityEngine;

namespace Assets.Scripts.Global
{
    public class ConfigManager
    {
        private const string CONFIG = "config.badapalace";

        public static GameConfigCorrespondent Setting
        {
            get
            {
                lock (_locker)
                {
                    if (_instance == null) { _instance = new ConfigManager(); }
                }
                return _instance.m_ConfigCorrespondent;
            }
        }
        public static GameProgressCorrespondent Prog
        {
            get
            {
                lock (_locker)
                {
                    if (_instance == null) { _instance = new ConfigManager(); }
                }
                return _instance.m_ProgressCorrespondent;
            }
        }

        private static ConfigManager _instance;
        private static object _locker = new object();

        private GameConfigCorrespondent m_ConfigCorrespondent;
        private GameProgressCorrespondent m_ProgressCorrespondent;
        private GameConfig m_Setting;
        private ConfigManager()
        {
            LoadSetting();
            _instance = this;
        }
        private void SaveSetting()
        {
            Debug.Log("SaveSetting...");
            var l_JsonString = JsonUtility.ToJson(m_Setting);
            Debug.Log("json file: " + l_JsonString);
            PlayerPrefs.SetString(CONFIG, l_JsonString);
            Debug.Log("SaveSetting done.");
        }
        private void LoadSetting()
        {
            Debug.Log("LoadSetting...");
            if (PlayerPrefs.HasKey(CONFIG))
            {
                var l_JsonString = PlayerPrefs.GetString(CONFIG);
                Debug.Log($"CONFIG exists in PlayerPrefs.\n{l_JsonString}");
                m_Setting = JsonUtility.FromJson<GameConfig>(l_JsonString);
            }
            else
            {
                Debug.Log("CONFIG doesn't exist in PlayerPrefs.");
                Reset();
            }
            m_ConfigCorrespondent = new GameConfigCorrespondent(ref m_Setting, SaveSetting);
            m_ProgressCorrespondent = new GameProgressCorrespondent(
                ref m_Setting.CurrentProgress, ref m_Setting.RecordProgress, ref m_Setting.MaximumProgress, SaveSetting);
            Debug.Log("LoadSetting done.");
        }
        private void Reset()
        {
            m_Setting = new GameConfig();
            m_Setting.CurrentProgress = new GameProgress();
            m_Setting.RecordProgress = new GameProgress();
            SaveSetting();
            m_ConfigCorrespondent = new GameConfigCorrespondent(ref m_Setting, SaveSetting);
        }
    }
}
