using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : PersistentSingleton<LoadSceneManager>
{
    [SerializeField] private Animator m_Animator;

    private bool loading = false;
    public void LoadLevel(LevelSO _level)
    {
        if (loading) return;

        StartCoroutine(LoadAsyncScene(_level.levelName));
    }

    public IEnumerator LoadAsyncScene(string sceneName)
    {
        loading = true;

        m_Animator.SetTrigger("Start");
        
        yield return new WaitForSeconds(1f);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

        yield return new WaitUntil(() => asyncLoad.isDone);

        GameManager.Instance.SceneReady();

        m_Animator.SetTrigger("End");

        yield return new WaitForSeconds(1f);

        loading = false;

        
    }

    public IEnumerator UnloadAsyncScene(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.UnloadSceneAsync(sceneName);

        yield return new WaitUntil(() => asyncLoad.isDone);
    }
}
