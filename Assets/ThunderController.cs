using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderController : MonoBehaviour
{
    public float minDelay = 30;
    public float maxDelay = 100;
    public Material skyBoxMaterial;

    float currentDelay;
    float t = 0;
    float t2 = 0;

    public Thunder[] thunders;
    bool currentlyPlaying = false;

    AmbientSoundPlayer player;

    Thunder activeThunder;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<AmbientSoundPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!currentlyPlaying) {
            t += Time.deltaTime;
            if (t >= currentDelay)
            {
                t = 0;
                activeThunder = thunders[Random.Range(0, thunders.Length - 1)];
                currentlyPlaying = true;
                Invoke(nameof(PlaySound), activeThunder.soundDelay+Random.Range(-activeThunder.range, activeThunder.range));
            }
        }
        else
        {
            t2 += Time.deltaTime;
        }
    }

    void PlaySound()
    {
        player.Play();
    }
}
[System.Serializable]
public struct Lightning
{
    public float delay;
    public float time;
    public float delayRange;
    public float timeRange;
}
[System.Serializable]
public struct Thunder
{
    public Lightning[] lightnings;
    public AudioClip audio;
    public float soundDelay;
    public float range;
}