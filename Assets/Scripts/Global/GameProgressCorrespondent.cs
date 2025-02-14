using System;

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
                if (m_CurrentProgress.Stage == m_MaximumProgress.Stage)
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
            a_IsLevelDownable = false;
            a_IsLevelUpable = false;
            a_IsStageDownable = false;
            a_IsStageUpable = false;

            if (m_RecordProgress.Level > 1) // LEVEL Upable, Downable or Bidir
            {
                if (m_CurrentProgress.Level == m_RecordProgress.Level) // LEVEL Downable 10??
                {
                    a_IsLevelDownable = true;                                       //O   X   ?   ?
                                                 // + StageDownable 101?
                    a_IsStageDownable = true;                                       //O   X   O   ?
                    if (m_CurrentProgress.Stage < m_RecordProgress.Stage)
                    {
                        a_IsStageUpable = true;                                     //O   X   O   O//
                    }// else //1010                                                 //O   X   O   X//
                }
                else //current.Level < record.Level
                {
                    a_IsLevelUpable = true;       //?   O   ?   ?
                    if (m_CurrentProgress.Level > 1)  //1 < current.Level < record.Level // LEVEL Bidir //current.Level > record.Level
                    {
                        a_IsLevelDownable = true;                                   //O   O   ?   ?
                        a_IsStageDownable = true;
                        a_IsStageUpable = true;                                     //O   O   O   O//
                    }
                    else //current.Level == 1 && current.Level < record.Level LEVEL Bidir
                    {
                        a_IsStageUpable = true; //01?1                              //X   O   ?   O
                        if (m_CurrentProgress.Stage > 1)
                        {
                            a_IsStageDownable = true; //0111                        //X   O   O   O//
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
                        a_IsStageUpable = true;                                     //X   X   X   O//
                    }
                    else
                    {
                        a_IsStageDownable = true; //+StageBidir                     //X   X   O   ?

                        if (m_CurrentProgress.Stage < m_RecordProgress.Stage) // StageBidir 0011
                        {
                            a_IsStageUpable = true;                                 //X   X   O   O//
                        }//else                                                     //X   X   O   X//
                    }
                }//else                                                             //X   X   X   X//
            }
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
            GetControllerState(out m_IsLevelDownable, out m_IsLevelUpable, out m_IsStageDownable, out m_IsStageUpable);
            Return(out a_IsLevelDownable, out a_IsLevelUpable, out a_IsStageDownable, out a_IsStageUpable);
        }
    }
}
