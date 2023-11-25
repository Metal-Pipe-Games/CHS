using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientSoundPlayer : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip[] audioClips;
    public float minDelay = 10;
    public float maxDelay = 50;
    public float radiusFrom = 30;
    public float radiusTo = 50;
    public Transform center;
    public Vector3 axisMultipler = Vector3.one;
    public bool auto = true;

    public float currentDelay;
    public float t = 0;
    // Start is called before the first frame update
    void Start()
    {
        currentDelay = Random.Range(minDelay, maxDelay);
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!audioSource.isPlaying && auto)
        {
            t += Time.deltaTime;
            if(t >= currentDelay)
            {
                t = 0;
                Play();
            }
        }
    }

    public void Play()
    {
        currentDelay = Random.Range(minDelay, maxDelay);
        audioSource.clip = audioClips[Random.Range(0, audioClips.Length - 1)];
        Vector3 pointer = new(Random.Range(-1, 1) * axisMultipler.x, Random.Range(-1, 1) * axisMultipler.y, Random.Range(-1, 1) * axisMultipler.z);
        transform.localPosition = pointer * Random.Range(radiusFrom, radiusTo);
        audioSource.Play();
    }

    public void Play(AudioClip clip)
    {
        currentDelay = Random.Range(minDelay, maxDelay);
        audioSource.clip = clip;
        Vector3 pointer = new(Random.Range(-1, 1) * axisMultipler.x, Random.Range(-1, 1) * axisMultipler.y, Random.Range(-1, 1) * axisMultipler.z);
        transform.localPosition = pointer * Random.Range(radiusFrom, radiusTo);
        audioSource.Play();
    }
}
