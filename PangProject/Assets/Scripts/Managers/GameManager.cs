using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : PersistentSingleton<GameManager>
{
    [Header("Scenes")]
    [SerializeField] private LevelSO Menu;
    [SerializeField] private List<LevelSO> levels = new List<LevelSO>();

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI timer;
    [SerializeField] private TextMeshProUGUI score;

    private float points = 0;

    [HideInInspector] public UnityEvent timeFreeze = new UnityEvent();
    [HideInInspector] public UnityEvent timeUnFreeze = new UnityEvent();

    public void UpdateTimer(float _time)
    {
        float minutes = Mathf.FloorToInt(_time / 60);
        float seconds = Mathf.FloorToInt(_time % 60);
        float miliseconds = Mathf.FloorToInt((_time * 1000) % 1000);

        timer.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, miliseconds);
    }

    public void UpdateScore(float _points)
    {
        points += _points;
        score.text = string.Format("{0:00000}", points);
    }

    public void FreezeTime(float _time)
    {
        StopCoroutine(FreezeTimeCo(_time));
        StartCoroutine(FreezeTimeCo(_time));       
    }

    private void UnFreezeTime()
    {
        timeUnFreeze?.Invoke();
    }

    public IEnumerator FreezeTimeCo(float _time)
    {
        timeFreeze?.Invoke();

        while (_time > 0)
        {
            yield return new WaitForEndOfFrame();
            _time -= Time.deltaTime;
        }

        UnFreezeTime();
    }

    public void StartGame()
    {
        LoadSceneManager.Instance.LoadLevel(levels[0]);
    }

    public void LoadNextLevel()
    {
        LevelSO currentLevel = levels.Find(lvl => lvl.levelName == SceneManager.GetActiveScene().name);
        int levelIndex = levels.IndexOf(currentLevel);

        if (levelIndex + 1 < levels.Count)
            LoadSceneManager.Instance.LoadLevel(levels[levelIndex + 1]);
        else
            ReturnToMenu();
    }

    public void ReturnToMenu()
    {
        LoadSceneManager.Instance.LoadLevel(Menu);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SceneReady()
    {

    }

}
