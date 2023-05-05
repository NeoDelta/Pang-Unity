using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField, Min(60)] private float timeInSeconds = 60f;

    [SerializeField] private List<Ball> balls = new List<Ball>();
    [SerializeField] private PlayerController player;

    private void Start()
    {
        StartTimer();
    }

    public void StartTimer()
    {
        StartCoroutine(TimerCo(timeInSeconds));
    }

    public void SetPlayer(PlayerController _player)
    {
        player = _player;
    }
    public void AddBall(Ball _ball)
    {
        if(!balls.Contains(_ball))
            balls.Add(_ball);
    }

    public void RemoveBall(Ball _ball)
    {
        balls.Remove(_ball);

        CheckLevelComplete();
    }

    private void CheckLevelComplete()
    {
        if (balls.Count != 0) return;

        //All balls destroyed = WIN
        GameManager.Instance.LoadNextLevel();
    }

    private IEnumerator TimerCo(float _time)
    {
        float remainingTime = _time;
        GameManager.Instance.UpdateTimer(remainingTime);

        while (remainingTime > 0)
        {
            yield return new WaitForEndOfFrame();
            remainingTime -= Time.deltaTime;
            GameManager.Instance.UpdateTimer(remainingTime);
        }
    }
}
