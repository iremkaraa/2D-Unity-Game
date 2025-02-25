using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Slider _timeSlider;
    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private GameObject _pauseCanvas;
    [SerializeField] private GameObject _loseCanvas;
    [SerializeField] private GameObject _winCanvas;
    
    [Space]
    [Header("Level Configs")]
    [SerializeField] private GameObject _collectiblesContainer;
    [SerializeField] private int _timeInSeconds;
    [SerializeField] private string _nextLevelName;
    [SerializeField] private float _timerIncreaseCount = 2.5f;
    
    private float _remainingTime;
    private int _neededScore;
    private int _currentScore;
    
    private bool _isGameOver = false;
    
    public static event Action OnGameLoseEvent;
    public static event Action OnGameWinEvent;

    private void Awake()
    {
        _isGameOver = false;
        Time.timeScale = 1;
        _neededScore = _collectiblesContainer.transform.childCount;
        _timeSlider.maxValue = _timeInSeconds;
        _remainingTime = _timeInSeconds;
        UpdateScoreUI();
        UpdateTimeUI();
        StartCoroutine(TimerCoroutine());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && _remainingTime > 0 && !_isGameOver)
        {
            OnPauseScreen();
        }
    }


    private void OnEnable()
    {
        Player.OnAppleCollected += UpdateScore;
        Player.OnAppleCollected += CheckScore;
        Player.OnTimeCollected += IncreaseTime;
        OnGameWinEvent += OnGameWin;
        OnGameLoseEvent += OnGameLose;
        
    }

    private void OnDisable()
    {
        Player.OnAppleCollected -= UpdateScore;
        Player.OnTimeCollected -= IncreaseTime;
        Player.OnAppleCollected -= CheckScore;
        OnGameWinEvent -= OnGameWin;
        OnGameLoseEvent -= OnGameLose;
    }

    private void CheckScore()
    {
        if(_currentScore >= _neededScore)
            OnGameWinEvent?.Invoke();
    }

    private IEnumerator TimerCoroutine()
    {
        while (_remainingTime >= 0)
        {
            yield return 0;
            _remainingTime -= Time.deltaTime;
            UpdateTimeUI();
        }
        if (_currentScore < _neededScore)
        {
            OnGameLoseEvent?.Invoke();
        }
    }
    private void UpdateScore()
    {
        _currentScore += 1;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        _scoreText.text = _currentScore.ToString() + "/" + _neededScore.ToString();
    }

    private void IncreaseTime()
    {
        _remainingTime += _timerIncreaseCount;
        _remainingTime = Mathf.Clamp(_remainingTime, 0, _timeInSeconds);
    }
    
    private void UpdateTimeUI()
    {
        // Yüzdelik değeri doğru hesaplayarak string'e çeviriyoruz
        float percentage = (_remainingTime / (float)_timeInSeconds) * 100;
        percentage = Mathf.Clamp(percentage, 0, 100);
        _timeText.text = "%" + percentage.ToString("F0"); // Tam sayı kısmını göstermek için "F0" formatı kullanılır
        _timeSlider.value = _remainingTime; // Slider değeri güncellenir
    }

    public void OnPauseScreen()
    {
        _pauseCanvas.SetActive(!_pauseCanvas.activeInHierarchy);
        Time.timeScale = _pauseCanvas.activeInHierarchy ? 0 : 1;
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("StartScene");
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnGameWin()
    {
        _isGameOver = true;
        Debug.Log("Game Win");
        _winCanvas.SetActive(true);
        Time.timeScale = 0;
        
    }


    private void OnGameLose()
    {
        _isGameOver = true;
        Debug.Log("Game Lose");
        _loseCanvas.SetActive(true);
        Time.timeScale = 0;
    }
    
    
}
