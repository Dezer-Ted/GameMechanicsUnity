using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentDataHolder : MonoBehaviour
{
    private static PersistentDataHolder instance = null;

    public static PersistentDataHolder Instance { get { return instance; } private set { instance = value; } }
    public bool StaminaTutorial { get; set; }
    public bool ScoreTutorial { get; set; }
    public bool CompassTutorial { get; set; }
    public bool EnemyTutorial { get; set; }
    public bool FoodTutorial { get; set; }
    private HandleHighscores handler; 
    public HighscoreList highscores { get; set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
            handler = new HandleHighscores(Application.persistentDataPath, "Highscores");
            highscores = handler.Load();
        }
    }
    public void SaveHighscores(Highscore score)
    {
        highscores.Addhighscore(score);
        handler.Save(highscores);
    }
}
