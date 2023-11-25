using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveLoad
{
    public static void Save(SaveData data,string saveName)
    {
        BinaryFormatter formatter = new();
        string path = Application.persistentDataPath + saveName + ".save";

        FileStream stream = new(path, FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static SaveData? Load(string saveName)
    {
        string path = Application.persistentDataPath + saveName + ".save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new();
            FileStream stream = new(path, FileMode.Open);

            SaveData? data = formatter.Deserialize(stream) as SaveData?;

            if (data.HasValue)
            {
                return data.Value;
            }
            else return null;
        }
        return null;
    }

    public static void Delete(string saveName)
    {
        string path = Application.persistentDataPath + saveName + ".save";

        File.Delete(path);
    }
}
