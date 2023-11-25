using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CameraFade : MonoBehaviour
{
    public float fadeTime = 5;
    public FadeMode mode = FadeMode.fadeIn;
    public bool updateVolume = false;
    float t = 0;
    Image image;
    public UnityEvent finished;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    float percentage;

    // Update is called once per frame
    void Update()
    {
        switch (mode)
        {
            case FadeMode.fadeIn:
                t += Time.deltaTime;
                percentage = 1 - t / fadeTime;
                image.color = new Color(image.color.r, image.color.g, image.color.b, percentage);
                if (updateVolume) AudioListener.volume = Settings.Volume * (1 - percentage);

                if (t >= fadeTime) { mode = FadeMode.none; t = 0; AudioListener.volume = Settings.Volume; finished.Invoke(); }
                break;
            case FadeMode.fadeOut:
                t += Time.deltaTime;
                percentage = t / fadeTime;
                image.color = new Color(image.color.r, image.color.g, image.color.b, percentage);
                if (updateVolume) AudioListener.volume = Settings.Volume * (1 - percentage);

                if (t >= fadeTime) { mode = FadeMode.none; t = 0; AudioListener.volume = 0; finished.Invoke(); }
                break;
        }
    }
}
public enum FadeMode
{
    none,
    fadeIn,
    fadeOut
}