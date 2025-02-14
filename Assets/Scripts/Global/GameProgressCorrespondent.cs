using System;
using UnityEngine;
namespace Assets.Scripts.Global
{
    public class GameProgressCorrespondent
    {
        private GameProgress m_CurrentProgress, m_RecordProgress, m_MaximumProgress;
        private Action m_OnChanged;
        private object locker = new object();
        private bool m_IsLevelUpable, m_IsLevelDownable, m_IsStageUpable, m_IsStageDownable;
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
                    if (CompareCurrentToRecord() is CompareType.Bigger)
                    {
                        m_CurrentProgress = m_RecordProgress;
                        return;
                    }
                    m_CurrentProgress = value;
                    m_OnChanged();
                }
            }
        }
        public GameProgress RecordProgress
        {
            get
            {
                lock (locker) { return Clone(m_RecordProgress); }
            }
        }
        public GameProgress MaximumProgress
        {
            get
            {
                lock (locker) { return Clone(m_MaximumProgress); }
            }
        }
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
            m_OnChanged = a_OnChanged;
        }
        public void IncreaseStage()
        {
            lock (locker)
            {
                CompareType l_Compared = CompareCurrentToRecord();
                if (l_Compared == CompareType.Same)
                {
                    m_RecordProgress.Stage++;
                    if (m_RecordProgress.Level < m_MaximumProgress.Level)
                    {
                        if (m_RecordProgress.Stage > m_MaximumProgress.Stage)
                        {
                            m_RecordProgress.Stage = 1;
                            m_RecordProgress.Level++;
                        }
                    }
                    m_CurrentProgress.Level = m_RecordProgress.Level;
                    m_CurrentProgress.Stage = m_RecordProgress.Stage;
                }
                else
                {
                    m_CurrentProgress.Stage++;
                    if (m_CurrentProgress.Level < m_MaximumProgress.Level)
                    {
                        if (m_CurrentProgress.Stage == m_MaximumProgress.Stage)
                        {
                            m_CurrentProgress.Level++;
                            m_CurrentProgress.Stage = 1;
                        }
                    }
                }
                m_OnChanged();
            }
        }
        public void LevelDown(
            out bool a_IsLevelDownable, out bool a_IsLevelUpable,
            out bool a_IsStageDownable, out bool a_IsStageUpable)
        {
            if (m_IsLevelDownable)
            {
                m_CurrentProgress.Level--;
                ApplyAndReturn(out a_IsLevelDownable, out a_IsLevelUpable,
                    out a_IsStageDownable, out a_IsStageUpable);
            }
            else
            {
                Return(out a_IsLevelDownable, out a_IsLevelUpable,
                    out a_IsStageDownable, out a_IsStageUpable);
            }
        }
        public void LevelUp(
            out bool a_IsLevelDownable, out bool a_IsLevelUpable,
            out bool a_IsStageDownable, out bool a_IsStageUpable)
        {
            if (m_IsLevelUpable)
            {
                m_CurrentProgress.Level++;
                if (m_CurrentProgress.Level == m_RecordProgress.Level)
                {
                    if (m_CurrentProgress.Stage > m_RecordProgress.Stage)
                    {
                        m_CurrentProgress.Stage = m_RecordProgress.Stage;
                    }

                }
                ApplyAndReturn(out a_IsLevelDownable, out a_IsLevelUpable,
                    out a_IsStageDownable, out a_IsStageUpable);
            }
            else
            {
                Return(out a_IsLevelDownable, out a_IsLevelUpable,
                    out a_IsStageDownable, out a_IsStageUpable);
            }
        }
        public void StageDown(
            out bool a_IsLevelDownable, out bool a_IsLevelUpable,
            out bool a_IsStageDownable, out bool a_IsStageUpable)
        {
            if (m_IsStageDownable)
            {
                m_CurrentProgress.Stage--;

                if (m_CurrentProgress.Stage == 0)
                {
                    m_CurrentProgress.Level--;
                    m_CurrentProgress.Stage = m_MaximumProgress.Stage;
                }
                ApplyAndReturn(out a_IsLevelDownable, out a_IsLevelUpable,
                    out a_IsStageDownable, out a_IsStageUpable);
            }
            else
            {
                Return(out a_IsLevelDownable, out a_IsLevelUpable,
                    out a_IsStageDownable, out a_IsStageUpable);
            }
        }
        public void StageUp(
            out bool a_IsLevelDownable, out bool a_IsLevelUpable,
            out bool a_IsStageDownable, out bool a_IsStageUpable)
        {
            if (m_IsStageUpable)
            {
                m_CurrentProgress.Stage++;
                if (m_CurrentProgress.Stage > m_MaximumProgress.Stage)
                {
                    m_CurrentProgress.Level++;
                    m_CurrentProgress.Stage = 1;
                }
                ApplyAndReturn(out a_IsLevelDownable, out a_IsLevelUpable,
                    out a_IsStageDownable, out a_IsStageUpable);
            }
            else
            {
                Return(out a_IsLevelDownable, out a_IsLevelUpable,
                    out a_IsStageDownable, out a_IsStageUpable);
            }
        }
        private CompareType CompareCurrentToRecord()
        {
            if (m_CurrentProgress.Level > m_RecordProgress.Level)
            {
                return CompareType.Bigger;
            }
            else if (m_CurrentProgress.Level == m_RecordProgress.Level)
            {
                if (m_CurrentProgress.Stage > m_RecordProgress.Stage)
                {
                    return CompareType.Bigger;
                }
                else if (m_CurrentProgress.Stage == m_RecordProgress.Stage)
                {
                    return CompareType.Same;
                }
            }
            return CompareType.Smaller;
        }
        public void GetControllerState(
            out bool a_IsLevelDownable, out bool a_IsLevelUpable,
            out bool a_IsStageDownable, out bool a_IsStageUpable)
        {
            GetControllerState();
            Return(out a_IsLevelDownable, out a_IsLevelUpable, out a_IsStageDownable, out a_IsStageUpable);
        }
        private void GetControllerState()
        {
            m_IsLevelDownable = false;
            m_IsLevelUpable = false;
            m_IsStageDownable = false;
            m_IsStageUpable = false;

            if (m_RecordProgress.Level > 1) // LEVEL Upable, Downable or Bidir
            {
                if (m_CurrentProgress.Level == m_RecordProgress.Level) // LEVEL Downable 10??
                {
                    m_IsLevelDownable = true;                                       //O   X   ?   ?
                    Debug.Log("m_IsLevelDownable = true");
                    // + StageDownable 101?
                    m_IsStageDownable = true;                                       //O   X   O   ?
                    Debug.Log("m_IsStageDownable = true");
                    if (m_CurrentProgress.Stage < m_RecordProgress.Stage)
                    {
                        m_IsStageUpable = true;                                     //O   X   O   O//
                        Debug.Log("m_IsStageUpable = true");
                    }// else //1010                                                 //O   X   O   X//
                }
                else //current.Level < record.Level
                {
                    m_IsLevelUpable = true;       //?   O   ?   ?
                    Debug.Log("m_IsLevelUpable = true");
                    m_IsStageUpable = true; //01?1                              //?   O   ?   O
                    Debug.Log("m_IsStageUpable = true");
                    if (m_CurrentProgress.Level > 1)  //1 < current.Level < record.Level // LEVEL Bidir //current.Level > record.Level
                    {
                        m_IsLevelDownable = true;                                   //O   O   ?   ?
                        m_IsStageDownable = true;
                        Debug.Log("m_IsLevelDownable = true");
                        Debug.Log("m_IsStageDownable = true");
                    }
                    else //current.Level == 1 && current.Level < record.Level LEVEL Bidir
                    {
                        if (m_CurrentProgress.Stage > 1)
                        {
                            m_IsStageDownable = true; //0111                        //X   O   O   O//
                            Debug.Log("m_IsStageDownable = true");
                        }//else                                                     //X   O   X   O//
                    }
                }
            }
            else  //LevelNone 00??                                                  //X   X   ?   ?
            {
                if (m_RecordProgress.Stage > 1) //record.Stage > 1 00??
                {
                    if (m_CurrentProgress.Stage == 1) // StageUpable 0001
                    {
                        m_IsStageUpable = true;                                     //X   X   X   O//
                        Debug.Log("m_IsStageUpable = true");
                    }
                    else
                    {
                        m_IsStageDownable = true; //+StageBidir                     //X   X   O   ?
                        Debug.Log("m_IsStageDownable = true");

                        if (m_CurrentProgress.Stage < m_RecordProgress.Stage) // StageBidir 0011
                        {
                            m_IsStageUpable = true;                                 //X   X   O   O//
                            Debug.Log("m_IsStageUpable = true");
                        }//else                                                     //X   X   O   X//
                    }
                }//else                                                             //X   X   X   X//
            }

            Debug.Log($"LD: {m_IsLevelDownable}, LU: {m_IsLevelUpable}, SD: {m_IsStageDownable}, SU: {m_IsStageUpable}");
        }
        private void Return(
            out bool a_IsLevelDownable, out bool a_IsLevelUpable,
            out bool a_IsStageDownable, out bool a_IsStageUpable)
        {
            a_IsLevelDownable = m_IsLevelDownable;
            a_IsLevelUpable = m_IsLevelUpable;
            a_IsStageDownable = m_IsStageDownable;
            a_IsStageUpable = m_IsStageUpable;
        }
        private void ApplyAndReturn(
            out bool a_IsLevelDownable, out bool a_IsLevelUpable,
            out bool a_IsStageDownable, out bool a_IsStageUpable)
        {
            m_OnChanged();
            GetControllerState();
            Return(out a_IsLevelDownable, out a_IsLevelUpable, out a_IsStageDownable, out a_IsStageUpable);
        }
    }
}
