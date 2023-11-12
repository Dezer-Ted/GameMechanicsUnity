using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighscoreList
{
    //dataclass for easier serialization to Json
    //public to use Unitys build in serializer to json

    public Highscore[] List = new Highscore[5];

   public HighscoreList() 
   {
        for (int i = 0; i <List.Length; i++)
        {
            List[i] = new Highscore();
        }
   }
   public void Addhighscore(Highscore newScore)
    {
        if(newScore.Score > List[4].Score)
        {
            List[4] = newScore;
            Sort();
        }
    }
    //Sorts List Highest -> Lowest
    public void Sort()
    {
        Array.Sort(List, new HighscoreComparer());
        Array.Reverse(List);
    }
}
