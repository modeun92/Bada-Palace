using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainUi : MonoBehaviour
{
    // Start is called before the first frame update
    private string tempScene;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GoToScene(string nextScene)
    {
        if (nextScene == "EXIT")
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
        }
        tempScene = nextScene;
        Invoke("LoadScene", 0.5f);
    }
    private void LoadScene()
    {
        StartCoroutine("LoadMyAsyncScene");
    }
    private IEnumerator LoadMyAsyncScene()
    {
        // AsyncOperation을 통해 Scene Load 정도를 알 수 있다.
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(tempScene);
        // Scene을 불러오는 것이 완료되면, AsyncOperation은 isDone 상태가 된다.
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
