using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : PersistentSingleton<LoadSceneManager>
{
    [SerializeField] private Animator m_Animator;

    public void LoadLevel(LevelSO _level)
    {
        if (SceneManager.GetSceneByName(_level.levelName) == null) return;

        StartCoroutine(LoadAsyncScene(_level.levelName));
    }

    public IEnumerator LoadAsyncScene(string sceneName)
    {
        m_Animator.SetTrigger("Start");
        
        yield return new WaitForSeconds(1f);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

        yield return new WaitUntil(() => asyncLoad.isDone);

        m_Animator.SetTrigger("End");
    }

    public IEnumerator UnloadAsyncScene(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.UnloadSceneAsync(sceneName);

        yield return new WaitUntil(() => asyncLoad.isDone);
    }
}
