using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public static string saveName = "0";
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

    public static void CreateSave(int count)
    {
        SaveLoad.Save(defaultSave.SaveData, "floppydisk_" + count);
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
        SaveData? data = SaveLoad.Load(saveName);

        if (data.HasValue)
        {
            return data.Value;
        }
        else
        {
            throw new System.Exception("Unable to load save");
        }
    }
}
