using UnityEngine;

[CreateAssetMenu(fileName = "SaveData", menuName = "CHS/SaveState", order = 1)]
public class SaveState: ScriptableObject
{
    public SaveData SaveData;
}
[System.Serializable]
public class SaveData
{
    [Header("Player Health")]
    public float health;
    public float scars;
    [Header("Player Tools")]
    public byte Ammo;
    public uint TotalAmmo;
    public ushort[] inventory;
    [Header("Level Data")]
    public byte level;
    public byte savePoint;
    public int levelFlags;
    public int[] aliveEnemies;
    [Header("Save Data")]
    public int playtime;
}
[System.Serializable]
public class Save
{
    public string Name;
    public SaveData data;
}