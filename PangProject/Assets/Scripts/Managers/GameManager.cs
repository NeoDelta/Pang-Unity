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
    [SerializeField] private Canvas hud;
    [SerializeField] private TextMeshProUGUI timer;
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private Canvas result;
    [SerializeField] private TextMeshProUGUI resultTime;
    [SerializeField] private TextMeshProUGUI resultScore;
    [SerializeField] private Canvas lose;

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
        points = 0;
        LoadSceneManager.Instance.LoadLevel(levels[0]);
    }

    public void ReloadLevel()
    {
        hud.enabled = false;

        LevelSO currentLevel = levels.Find(lvl => lvl.levelName == SceneManager.GetActiveScene().name);
        int levelIndex = levels.IndexOf(currentLevel);

        LoadSceneManager.Instance.LoadLevel(levels[levelIndex]);
    }

    public void LoadNextLevel()
    {
        hud.enabled = false;

        LevelSO currentLevel = levels.Find(lvl => lvl.levelName == SceneManager.GetActiveScene().name);
        int levelIndex = levels.IndexOf(currentLevel);

        if (levelIndex + 1 < levels.Count)
            LoadSceneManager.Instance.LoadLevel(levels[levelIndex + 1]);
        else
            ReturnToMenu();
    }

    public void OnWinLevel(float _time)
    {
        StartCoroutine(ResultScreenCo(_time, points));
    }

    private IEnumerator ResultScreenCo(float _time, float _score)
    {
        result.enabled = true;

        while (_time > 0)
        {
            yield return new WaitForEndOfFrame();
            _time -= Mathf.Max(0.25f + Time.deltaTime, 0f);
            _score += 5f;
            UpdateResultScreen(_time, _score);
        }

        points = _score;
        UpdateResultScreen(0, _score);

        yield return new WaitForSecondsRealtime(3f);

        LoadNextLevel();
    }

    public void OnLoseLevel()
    {
        if (lose.enabled) return;

        FreezeTime(4f);
        StartCoroutine(LoseScreenCo(3f));
    }
    private IEnumerator LoseScreenCo(float _time)
    {
        lose.enabled = true;

        yield return new WaitForSecondsRealtime(_time);

        ReturnToMenu();
    }

    private void UpdateResultScreen(float _time, float _points)
    {
        float minutes = Mathf.FloorToInt(_time / 60);
        float seconds = Mathf.FloorToInt(_time % 60);
        float miliseconds = Mathf.FloorToInt((_time * 1000) % 1000);

        resultTime.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, miliseconds);

        resultScore.text = string.Format("{0:00000}", _points);
    }

    public void ReturnToMenu()
    {
        points = 0;

        LoadSceneManager.Instance.LoadLevel(Menu);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SceneReady()
    {
        result.enabled = false;
        lose.enabled = false;
        hud.enabled = SceneManager.GetActiveScene().name != Menu.levelName;
    }

}
