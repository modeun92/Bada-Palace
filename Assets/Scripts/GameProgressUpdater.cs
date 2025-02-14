using Assets.Scripts.Global;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class GameProgressUpdater : MonoBehaviour
{
    public Button LevelDownButton;
    public Button LevelUpButton;
    public TextMeshProUGUI LevelText;

    public Button StageDownButton;
    public Button StageUpButton;
    public TextMeshProUGUI StageText;

    private bool m_IsLevelUpable, m_IsLevelDownable, m_IsStageUpable, m_IsStageDownable;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("level:" + ConfigManager.Prog.CurrentProgress.Level + 
            ",stage:"+ ConfigManager.Prog.CurrentProgress.Stage);
        ConfigManager.Prog.GetControllerState(
            out m_IsLevelDownable, out m_IsLevelUpable,
            out m_IsStageDownable, out m_IsStageUpable);
        UiUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LevelDown()
    {
        Debug.Log("LevelDown");
        ConfigManager.Prog.LevelDown(
            out m_IsLevelDownable, out m_IsLevelUpable,
            out m_IsStageDownable, out m_IsStageUpable);
        UiUpdate();
    }
    public void LevelUp()
    {
        Debug.Log("LevelUp");
        ConfigManager.Prog.LevelUp(
            out m_IsLevelDownable, out m_IsLevelUpable,
            out m_IsStageDownable, out m_IsStageUpable);
        UiUpdate();
    }
    public void StageDown()
    {
        Debug.Log("StageDown");
        ConfigManager.Prog.StageDown(
            out m_IsLevelDownable, out m_IsLevelUpable,
            out m_IsStageDownable, out m_IsStageUpable);
        UiUpdate();
    }
    public void StageUp()
    {
        Debug.Log("StageUp");
        ConfigManager.Prog.StageUp(
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

        Debug.Log($"LD: {m_IsLevelDownable}, LU: {m_IsLevelUpable}, SD: {m_IsStageDownable}, SU: {m_IsStageUpable}");
        LevelText.text = ConfigManager.Prog.CurrentProgress.Level.ToString();
        StageText.text = ConfigManager.Prog.CurrentProgress.Stage.ToString();
    }
}
