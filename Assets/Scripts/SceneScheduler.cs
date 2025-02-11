using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneScheduler : MonoBehaviour
{
    public Object PlayScene;
    public void LoadPlayScene()
    {
        SceneManager.LoadScene(PlayScene.name, LoadSceneMode.Single);
    }
}
