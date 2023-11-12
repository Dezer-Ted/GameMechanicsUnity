using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class HandleHighscores 
{
    string dataDirPath = "";
    string dataFileName = "";

   public HandleHighscores(string dataDirPath,string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }
    //Saves Highscores to file in the json format from a HighscoreList Obj
    public void Save(HighscoreList highscores)
    {
        string fullPath = Path.Combine(dataDirPath,dataFileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            string dataToStore = JsonConvert.SerializeObject(highscores);


            using (FileStream stream = new FileStream(fullPath,FileMode.Create))
            {
                using(StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch(Exception e)
        {
            Debug.LogError("Error occured when trying to save data to: " + fullPath +"\n"+e);
        }
    }
    //Loads Highscores from Json and deserializes it back to a HighscoreList
    public HighscoreList Load()
    {
        HighscoreList scores = new HighscoreList();
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        if(File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }
                scores = JsonConvert.DeserializeObject<HighscoreList>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to load data from: " + fullPath + "\n" + e);

            }
        }
        return scores;
    }
}
