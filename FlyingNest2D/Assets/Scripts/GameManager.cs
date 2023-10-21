using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    private int score;
    [SerializeField]
    GameObject scoreText;

    private static GameManager instance = null;
    public static GameManager Instance { get { return instance; } private set { instance = value; } }
    public int Score {
        get { return score; }
        set 
        {
            score = value;
            scoreText.GetComponent<TextMeshProUGUI>().text = "Score: " + score.ToString();
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
