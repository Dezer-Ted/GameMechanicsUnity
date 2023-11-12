using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //Singleton Class
    private int score;
    private int oldScore;
    private float timer;
    private bool timerIsRunning = false;
    Coroutine counter;
    [SerializeField]
    GameObject scoreText;
    [SerializeField]
    float countDuration;
    [SerializeField]
    GameObject gameOverUI;
    [SerializeField]
    GameObject saveNameUI;
    [SerializeField]
    TMP_InputField nameInput;
    [SerializeField]
    TextMeshProUGUI timerText;
    [SerializeField]
    List<TextMeshProUGUI> scores;
    [SerializeField]
    GameObject scoreBoard;
    [SerializeField]
    AudioSource gameOverSound;
    [SerializeField]
    AudioSource buttonClickSound;
    public bool GamePaused {  get; set; }
    private static GameManager instance = null;
    public static GameManager Instance { get { return instance; } private set { instance = value; } }
    public int Score {
        get { return score; }
        set 
        {
            
            oldScore = score;
            score = value;
            if (counter != null)
                StopCoroutine(counter);
            counter = StartCoroutine(CountTo());
        }
    }
    void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        oldScore = score;
        Time.timeScale = 1;
        GameManager.Instance.GamePaused = false;
        timer = 0;
    }
    private void Update()
    {
        UpdateTimer();
    }
    //smooth number climbing animation for score
    IEnumerator CountTo()
    {
        var rate = Mathf.Abs(oldScore - score) / countDuration;
        while(score != oldScore)
        {
            oldScore = (int)Mathf.MoveTowards(oldScore, score, rate * Time.deltaTime);
            scoreText.GetComponent<TextMeshProUGUI>().text = "Score: "+ oldScore.ToString();
            yield return null;
        }
    }
    
    public void GameOver()
    {
        gameOverUI.SetActive(true);
        saveNameUI.SetActive(true);
        gameOverSound.Play();
        Time.timeScale = 0;
        GameManager.Instance.GamePaused = true;

    }
    public void RestartScene()
    {
        buttonClickSound.Play();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void ExitGame()
    {
        buttonClickSound.Play();
        Application.Quit();
    }
    //Saves name from Input field and enables highscores
    public void SaveName()
    {
        buttonClickSound.Play();
        string name = nameInput.text;
        saveNameUI.SetActive(false);
        Highscore score = new Highscore(name, Score, timer);
        PersistentDataHolder.Instance.SaveHighscores(score);
        ShowHighscores();
    }
    public void StartTimer()
    {
        timerIsRunning = true;
    }
    public void StopTimer()
    {
        timerIsRunning = false;
    }
    
    void UpdateTimer()
    {
        if (!timerIsRunning)
            return;
        timer += Time.deltaTime;
        
        timerText.text = TimerToString(timer);
    }
    //Creates a list of Highscores in the scoreboard of the game
    //Format: {Index}. {Name} {Score} {scorePerMin}
    void ShowHighscores()
    {
        scoreBoard.SetActive(true);
        float scorePerMinute;
        for (int index = 0; index < scores.Count; ++index)
        {
            string output = $"{index+1}. ";
            output += PersistentDataHolder.Instance.highscores.List[index].Name + "\t";
            output+= PersistentDataHolder.Instance.highscores.List[index].Score + "\t";
            if(PersistentDataHolder.Instance.highscores.List[index].Score !=0 )
            {
                scorePerMinute = PersistentDataHolder.Instance.highscores.List[index].Score / (timer / 60);
                output += scorePerMinute + " Pts/min";
            }
            else
            {
                output += "N/A";
            }
            
            scores[index].text = output;

        }
    }
    //Converts elapsed Seconds in to timer That is Always in MM:SS format and fills empty spaces with 0
    string TimerToString(float time)
    {
        int minutes = (int)time / 60;
        int seconds = (int)time - (minutes * 60);
        string output = "";
        if (minutes < 10)
        {
            output += "0";
        }
        output += minutes + ":";
        if (seconds < 10)
        {
            output += "0";
        }
        output += seconds;
        return output;
    }
}
