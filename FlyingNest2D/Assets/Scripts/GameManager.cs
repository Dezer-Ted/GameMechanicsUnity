using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    private int score;
    private int oldScore;
    Coroutine counter;
    [SerializeField]
    GameObject scoreText;
    [SerializeField]
    float countDuration;

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
    }

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
}
