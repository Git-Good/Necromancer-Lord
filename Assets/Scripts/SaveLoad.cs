﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveLoad {

    public static List<GameMemento> savedGames = new List<GameMemento>();

    public static void Save()
    {
        SaveLoad.savedGames.Add(GameMemento.current);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/savedGames.gd");
        bf.Serialize(file, SaveLoad.savedGames);
        file.Close();
    }

    public static void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/savedGames.gd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedGames.gd", FileMode.Open);
            SaveLoad.savedGames = (List<GameMemento>)bf.Deserialize(file);
            file.Close();
        }
    }
}