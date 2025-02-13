using Assets.Scripts.Global;
using UnityEngine;
using UnityEngine.UI;
public class GameProgressUpdater : MonoBehaviour
{
    public Button LevelUpButton;
    public Button LevelDownButton;
    public Button StageUpButton;
    public Button StageDownButton;

    public Text LevelText;
    public Text StageText;

    private bool m_IsLevelUpable, m_IsLevelDownable, m_IsStageUpable, m_IsStageDownable;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("level:" + ConfigManager.Setting.ProgressCorrespondent.CurrentProgress.Level + 
            ",stage:"+ ConfigManager.Setting.ProgressCorrespondent.CurrentProgress.Stage);
        UiUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LevelDown()
    {
        ConfigManager.Setting.ProgressCorrespondent.LevelDown(
            out m_IsLevelDownable, out m_IsLevelUpable,
            out m_IsStageDownable, out m_IsStageUpable);
        UiUpdate();
    }
    public void LevelUp()
    {
        ConfigManager.Setting.ProgressCorrespondent.LevelUp(
            out m_IsLevelDownable, out m_IsLevelUpable,
            out m_IsStageDownable, out m_IsStageUpable);
        UiUpdate();
    }
    public void StageDown()
    {
        ConfigManager.Setting.ProgressCorrespondent.StageDown(
            out m_IsLevelDownable, out m_IsLevelUpable,
            out m_IsStageDownable, out m_IsStageUpable);
        UiUpdate();
    }
    public void StageUp()
    {
        ConfigManager.Setting.ProgressCorrespondent.StageUp(
            out m_IsLevelDownable, out m_IsLevelUpable,
            out m_IsStageDownable, out m_IsStageUpable);
        UiUpdate();
    }
    private void UiUpdate()
    {
        LevelDownButton.interactable = m_IsLevelDownable;
        LevelUpButton.interactable = m_IsLevelUpable;
        StageDownButton.interactable = m_IsStageDownable;
        StageUpButton.interactable = m_IsStageUpable;
       
        LevelText.text = "Level " + ConfigManager.Setting.ProgressCorrespondent.CurrentProgress.Level;
        StageText.text = "Stage " + ConfigManager.Setting.ProgressCorrespondent.CurrentProgress.Stage;
    }
}
