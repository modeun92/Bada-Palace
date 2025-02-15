using System;
using UnityEngine;
namespace Assets.Scripts.Global
{
    public class GameProgressCorrespondent
    {
        private GameProgress m_TemporaryProgress, m_CurrentProgress, m_RecordProgress, m_MaximumProgress;
        private Action m_OnChanged;
        private object locker = new object();
        private bool m_IsSimulLevelUpable, m_IsSimulLevelDownable, m_IsSimulStageUpable, m_IsSimulStageDownable;
        public GameProgress CurrentProgress
        {
            get
            {
                lock (locker) { return Clone(m_CurrentProgress); }
            }
            set
            {
                lock (locker)
                {
                    if (Compare(m_CurrentProgress, m_RecordProgress) is CompareType.Bigger)
                    {
                        m_CurrentProgress = m_RecordProgress;
                        return;
                    }
                    m_CurrentProgress = value;
                    m_OnChanged();
                }
            }
        }
        public GameProgress SimulationProgress { get { lock (locker) { return Clone(m_TemporaryProgress); } } }
        public GameProgress RecordProgress { get { lock (locker) { return Clone(m_RecordProgress); } } }
        public GameProgress MaximumProgress { get { lock (locker) { return Clone(m_MaximumProgress); } } }
        private GameProgress Clone(GameProgress a_Original)
        {
            var l_Clone = new GameProgress()
            {
                Level = a_Original.Level,
                Stage = a_Original.Stage
            };
            return l_Clone;
        }
        public GameProgressCorrespondent(
            ref GameProgress a_CurrentProgress, ref GameProgress a_RecordProgress, ref GameProgress a_MaximumProgress,
            Action a_OnChanged)
        {
            m_CurrentProgress = a_CurrentProgress;
            m_RecordProgress = a_RecordProgress;
            m_MaximumProgress = a_MaximumProgress;
            m_TemporaryProgress = new GameProgress() { 
                Level = m_CurrentProgress.Level, 
                Stage = m_CurrentProgress.Stage };
            m_OnChanged = a_OnChanged;
        }
        public void IncreaseStage()
        {
            lock (locker)
            {
                CompareType l_Compared = Compare(m_CurrentProgress, m_RecordProgress);
                if (l_Compared is CompareType.Same)
                {
                    IncreaseStage(ref m_RecordProgress);
                    m_CurrentProgress.Level = m_RecordProgress.Level;
                    m_CurrentProgress.Stage = m_RecordProgress.Stage;
                }
                else
                {
                    IncreaseStage(ref m_CurrentProgress);
                }
                m_OnChanged();
            }
        }
        private void IncreaseStage(ref GameProgress a_Progress)
        {
            a_Progress.Stage++;
            if (a_Progress.Level < m_MaximumProgress.Level)
            {
                if (a_Progress.Stage == m_MaximumProgress.Stage)
                {
                    a_Progress.Level++;
                    a_Progress.Stage = 1;
                }
            }
        }
        public void SimulateLevelDown(
            out bool a_IsLevelDownable, out bool a_IsLevelUpable,
            out bool a_IsStageDownable, out bool a_IsStageUpable)
        {
            if (m_IsSimulLevelDownable)
            {
                m_TemporaryProgress.Level--;
                GetSimulateionState(out a_IsLevelDownable, out a_IsLevelUpable,
                    out a_IsStageDownable, out a_IsStageUpable);
            }
            else
            {
                Return(out a_IsLevelDownable, out a_IsLevelUpable,
                    out a_IsStageDownable, out a_IsStageUpable);
            }
        }
        public void SimulateLevelUp(
            out bool a_IsLevelDownable, out bool a_IsLevelUpable,
            out bool a_IsStageDownable, out bool a_IsStageUpable)
        {
            if (m_IsSimulLevelUpable)
            {
                m_TemporaryProgress.Level++;
                if (m_TemporaryProgress.Level == m_RecordProgress.Level)
                {
                    if (m_TemporaryProgress.Stage > m_RecordProgress.Stage)
                    {
                        m_TemporaryProgress.Stage = m_RecordProgress.Stage;
                    }

                }
                GetSimulateionState(out a_IsLevelDownable, out a_IsLevelUpable,
                    out a_IsStageDownable, out a_IsStageUpable);
            }
            else
            {
                Return(out a_IsLevelDownable, out a_IsLevelUpable,
                    out a_IsStageDownable, out a_IsStageUpable);
            }
        }
        public void SimulateStageDown(
            out bool a_IsLevelDownable, out bool a_IsLevelUpable,
            out bool a_IsStageDownable, out bool a_IsStageUpable)
        {
            if (m_IsSimulStageDownable)
            {
                m_TemporaryProgress.Stage--;

                if (m_TemporaryProgress.Stage == 0)
                {
                    m_TemporaryProgress.Level--;
                    m_TemporaryProgress.Stage = m_MaximumProgress.Stage;
                }
                GetSimulateionState(out a_IsLevelDownable, out a_IsLevelUpable,
                    out a_IsStageDownable, out a_IsStageUpable);
            }
            else
            {
                Return(out a_IsLevelDownable, out a_IsLevelUpable,
                    out a_IsStageDownable, out a_IsStageUpable);
            }
        }
        public void SimulateStageUp(
            out bool a_IsLevelDownable, out bool a_IsLevelUpable,
            out bool a_IsStageDownable, out bool a_IsStageUpable)
        {
            if (m_IsSimulStageUpable)
            {
                m_TemporaryProgress.Stage++;
                if (m_TemporaryProgress.Stage > m_MaximumProgress.Stage)
                {
                    m_TemporaryProgress.Level++;
                    m_TemporaryProgress.Stage = 1;
                }
                GetSimulateionState(out a_IsLevelDownable, out a_IsLevelUpable,
                    out a_IsStageDownable, out a_IsStageUpable);
            }
            else
            {
                Return(out a_IsLevelDownable, out a_IsLevelUpable,
                    out a_IsStageDownable, out a_IsStageUpable);
            }
        }
        public bool ApplySimulation()
        {
            if (Compare(m_TemporaryProgress, m_CurrentProgress) is not CompareType.Same)
            {
                m_CurrentProgress.Level = m_TemporaryProgress.Level;
                m_CurrentProgress.Stage = m_TemporaryProgress.Stage;
                m_OnChanged();
                return true;
            }
            return false;
        }
        private CompareType Compare(GameProgress a, GameProgress b)
        {
            if (a.Level > b.Level)
            {
                return CompareType.Bigger;
            }
            else if (a.Level == b.Level)
            {
                if (a.Stage > b.Stage)
                {
                    return CompareType.Bigger;
                }
                else if (a.Stage == b.Stage)
                {
                    return CompareType.Same;
                }
            }
            return CompareType.Smaller;
        }
        public void GetSimulateionState(
            out bool a_IsLevelDownable, out bool a_IsLevelUpable,
            out bool a_IsStageDownable, out bool a_IsStageUpable)
        {
            GetSimulateionState();
            Return(out a_IsLevelDownable, out a_IsLevelUpable, out a_IsStageDownable, out a_IsStageUpable);
        }
        private void GetSimulateionState()
        {
            m_IsSimulLevelDownable = false;
            m_IsSimulLevelUpable = false;
            m_IsSimulStageDownable = false;
            m_IsSimulStageUpable = false;

            if (m_RecordProgress.Level > 1) // LEVEL Upable, Downable or Bidir
            {
                if (m_TemporaryProgress.Level == m_RecordProgress.Level) // LEVEL Downable 10??
                {
                    m_IsSimulLevelDownable = true;                                       //O   X   ?   ?
                    Debug.Log("m_IsLevelDownable = true");
                    // + StageDownable 101?
                    m_IsSimulStageDownable = true;                                       //O   X   O   ?
                    Debug.Log("m_IsStageDownable = true");
                    if (m_TemporaryProgress.Stage < m_RecordProgress.Stage)
                    {
                        m_IsSimulStageUpable = true;                                     //O   X   O   O//
                        Debug.Log("m_IsStageUpable = true");
                    }// else //1010                                                 //O   X   O   X//
                }
                else //current.Level < record.Level
                {
                    m_IsSimulLevelUpable = true;       //?   O   ?   ?
                    Debug.Log("m_IsLevelUpable = true");
                    m_IsSimulStageUpable = true; //01?1                              //?   O   ?   O
                    Debug.Log("m_IsStageUpable = true");
                    if (m_TemporaryProgress.Level > 1)  //1 < current.Level < record.Level // LEVEL Bidir //current.Level > record.Level
                    {
                        m_IsSimulLevelDownable = true;                                   //O   O   ?   ?
                        m_IsSimulStageDownable = true;
                        Debug.Log("m_IsLevelDownable = true");
                        Debug.Log("m_IsStageDownable = true");
                    }
                    else //current.Level == 1 && current.Level < record.Level LEVEL Bidir
                    {
                        if (m_TemporaryProgress.Stage > 1)
                        {
                            m_IsSimulStageDownable = true; //0111                        //X   O   O   O//
                            Debug.Log("m_IsStageDownable = true");
                        }//else                                                     //X   O   X   O//
                    }
                }
            }
            else  //LevelNone 00??                                                  //X   X   ?   ?
            {
                if (m_RecordProgress.Stage > 1) //record.Stage > 1 00??
                {
                    if (m_TemporaryProgress.Stage == 1) // StageUpable 0001
                    {
                        m_IsSimulStageUpable = true;                                     //X   X   X   O//
                        Debug.Log("m_IsStageUpable = true");
                    }
                    else
                    {
                        m_IsSimulStageDownable = true; //+StageBidir                     //X   X   O   ?
                        Debug.Log("m_IsStageDownable = true");

                        if (m_TemporaryProgress.Stage < m_RecordProgress.Stage) // StageBidir 0011
                        {
                            m_IsSimulStageUpable = true;                                 //X   X   O   O//
                            Debug.Log("m_IsStageUpable = true");
                        }//else                                                     //X   X   O   X//
                    }
                }//else                                                             //X   X   X   X//
            }

            Debug.Log($"LD: {m_IsSimulLevelDownable}, LU: {m_IsSimulLevelUpable}, SD: {m_IsSimulStageDownable}, SU: {m_IsSimulStageUpable}");
        }
        private void Return(
            out bool a_IsLevelDownable, out bool a_IsLevelUpable,
            out bool a_IsStageDownable, out bool a_IsStageUpable)
        {
            a_IsLevelDownable = m_IsSimulLevelDownable;
            a_IsLevelUpable = m_IsSimulLevelUpable;
            a_IsStageDownable = m_IsSimulStageDownable;
            a_IsStageUpable = m_IsSimulStageUpable;
        }
    }
}
