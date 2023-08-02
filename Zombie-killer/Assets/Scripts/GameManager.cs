using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [HideInInspector ]public int score;
    [SerializeField] private TMP_Text scoreText;
    public Slider _playerHP;
    public TMP_Text timerText;
    private PlayerHealthSystem _playerHealthSystem;
    private float currentTime;
    
    [Header("Pause menu")]
    public GameObject pauseMenu;
    public TMP_Text pauseScoreText;
    public TMP_Text pauseTimerText;
    public static bool isPaused;


    private void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        _playerHealthSystem = player.GetComponent<PlayerHealthSystem>();
        _playerHealthSystem.playerHitEvent.AddListener(ShowPlayerHP);
        _playerHealthSystem.playerDeathEvent.AddListener(PauseGame);
        pauseMenu.SetActive(false);
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        ShowKillText();
        
        currentTime += Time.deltaTime;
        UpdateTimerText();
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    private void ShowKillText()
    {
        scoreText.text = $"Kills: {score}";
    }
    
    private void ShowPlayerHP()
    {
        _playerHP.value = _playerHealthSystem.health;
    }
    
    private void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);
        string timeString = string.Format("{0:00}:{1:00}", minutes, seconds);
        timerText.text = timeString;
    }
    
    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        pauseScoreText.text = $"Kills: {score}";
        pauseTimerText.text = timerText.text;
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        timerText.enabled = false;
        scoreText.enabled = false;
        _playerHP.gameObject.SetActive(false);
    }

    void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        timerText.enabled = true;
        scoreText.enabled = true;
        _playerHP.gameObject.SetActive(true);
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
    
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
