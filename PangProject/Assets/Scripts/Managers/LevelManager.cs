using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

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
        StartCoroutine(TimerCo());
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
        player.GetComponent<PlayerInput>().enabled = false;
        StopTimer();
        Debug.Log(timeInSeconds);
        GameManager.Instance.OnWinLevel(timeInSeconds);
    }


    public void StopTimer()
    {
        StopCoroutine(TimerCo());
    }

    private IEnumerator TimerCo()
    {
        GameManager.Instance.UpdateTimer(timeInSeconds);

        while (timeInSeconds > 0)
        {
            yield return new WaitForEndOfFrame();
            timeInSeconds -= Time.deltaTime;
            GameManager.Instance.UpdateTimer(timeInSeconds);
        }
       
        OnLevelLose();
    }

    public void OnLevelLose()
    {
        player.GetComponent<PlayerInput>().enabled = false;
        GameManager.Instance.ReturnToMenu();
    }
}
