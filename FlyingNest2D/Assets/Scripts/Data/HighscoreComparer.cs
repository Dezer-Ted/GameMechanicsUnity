using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighscoreComparer : IComparer
{
    //enables Array.Sort with Highscores
    public int Compare(object x, object y)
    {
        return (new CaseInsensitiveComparer()).Compare( ((Highscore)x).Score, ((Highscore)y).Score);
    }
}
