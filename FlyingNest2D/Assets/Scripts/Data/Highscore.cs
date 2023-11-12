using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highscore 
{
    //public to use Unitys build in serializer to json
    public string Name;
    public int Score;
    public float Time; 
    public Highscore() 
    { 
        Name = string.Empty;
        Score = 0;
        Time = 0;
    }
    public Highscore(string name, int score, float time)
    {
        Name = name;
        Score = score;
        Time = time;
    }
}
