using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;

public class GameSave
{
    public static GameSave Single { get; } = new GameSave();
    public GameData gameData = new GameData();


    public void LoadGameData()
    {
        string path = Application.dataPath + "\\GameData.xml";
        XmlSerializer xmlSerializer = new XmlSerializer(gameData.GetType());
        if (!File.Exists(path))
            return;
        using (FileStream fileStream = File.Open(path, FileMode.Open))
        {
            gameData = xmlSerializer.Deserialize(fileStream) as GameData;
        }
    }

    public void SaveGameData()
    {
        string path = Application.dataPath + "\\GameData.xml";
        XmlSerializer xmlSerializer = new XmlSerializer(gameData.GetType());
        using (FileStream fileStream = File.Open(path, FileMode.CreateNew))
        {
            xmlSerializer.Serialize(fileStream, gameData);
        }
    }

}
