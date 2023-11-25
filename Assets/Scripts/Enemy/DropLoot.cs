using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropLoot : MonoBehaviour
{
    public Loot[] lootables;

    public Loot? GetLoot()
    {
        Loot[] tempLootables = new Loot[lootables.Length];
        lootables.CopyTo(tempLootables, 0);
        Shuffle(tempLootables);

        Loot? chosenLoot = null;
        foreach (Loot loot in tempLootables)
        {
            if (Random.value < loot.chance)
            {
                chosenLoot = loot;
                break;
            }
        }
        return chosenLoot;
    }

    public void CreateLoot()
    {
        var t = GetLoot();

        if (t.HasValue)
        {
            var loot = t.Value;
            var g = Instantiate(loot.loot);
            g.GetComponent<Interactable>().item = g;
            g.transform.SetPositionAndRotation(transform.position, transform.rotation);
            g.SetActive(true);
        }
    }

    void Shuffle(Loot[] deck)
    {
        for (int i = 0; i < deck.Length; i++)
        {
            Loot temp = deck[i];
            int randomIndex = Random.Range(i, deck.Length);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
    }
}

[System.Serializable]
public struct Loot
{
    [Range(0,1)]
    public float chance;
    public GameObject loot;
}