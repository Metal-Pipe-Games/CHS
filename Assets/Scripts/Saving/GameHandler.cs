using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public static string saveName = "";
    public static Save save;
    public PlayerHealthController healthController;
    public ToolHandler toolHandler;
    public byte levelId;
    public byte savePoint;
    public EnemyController[] enemies;
    public static SaveState defaultSave;
    public static bool ready = false;
    // Start is called before the first frame update
    void Start()
    {
        SaveData data = Load();
        healthController.health = data.health;
        healthController.permDamage = data.scars;
    }

    static string FindAvailableName()
    {
        string path = Path.Combine(Application.persistentDataPath,"saves");

        string name = "";

        for(int i = 0; name == ""; i++)
        {
            string n = "floppydisk_" + i;
            if (!File.Exists(Path.Combine(path, n)))
            {
                name = n;
            }
        }

        return name;
    }

    string[] GetFiles()
    {
        var path = Path.Combine(Application.persistentDataPath, "saves");
        string[] files;
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        files = Directory.GetFiles(path);
        return files;
    }

    Save[] GetSaves()
    {
        return new Save[0];
    }

    public static void CreateSave()
    {
        string name = FindAvailableName();
        saveName = name;
        save = new Save()
        {
            Name=name,
            data=defaultSave.SaveData
        };
        SaveLoad.Save(defaultSave.SaveData, name);
    }

    public void Save()
    {
        SaveData data = new();
        data.health = healthController.health;
        data.scars = healthController.permDamage;

        data.Ammo = (byte)toolHandler.tools[Tools.Gun].AttackCount;
        data.TotalAmmo = (byte)toolHandler.tools[Tools.Gun].AttackCount;

        data.inventory = new ushort[0];
        data.level = levelId;
        data.savePoint = savePoint;
        data.levelFlags = 0;

        List<int> aliveEnemies = new();

        foreach(EnemyController eh in enemies)
        {
            if (eh.alive) aliveEnemies.Add(eh.enemyId);
        }

        data.aliveEnemies = aliveEnemies.ToArray();

        SaveLoad.Save(data, saveName);
    }

    public static SaveData Load()
    {
        SaveData data = SaveLoad.Load(saveName);

        if (data != null)
        {
            return data;
        }
        else
        {
            throw new System.Exception("Unable to load save");
        }
    }

    public static Save Load(string name)
    {
        saveName = name;
        var saveData = Load();
        save = new Save()
        {
            Name = name,
            data = saveData
        };
        return save;
    }
}
